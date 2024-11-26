﻿/// <summary>
/// The front end of the game.
/// Made by: Jani Aelterman.
/// </summary>

using CommunityToolkit.Maui.Views;
using Newtonsoft.Json.Linq;
using NumberMatch.Helpers;
using System.Runtime.CompilerServices;
using static NumberMatch.Helpers.Tools;

namespace NumberMatch
{
    public partial class MainPage : ContentPage
    {
        public GameBackend game { private set; get; }
        private const int COLUMNS = 9, ROWS = 13;
        private Tuple<int, int> previousPressedButton = null; //  previous pressed button, row and column
        private readonly Color dynamicBackgroundColor = (Color)(Application.Current.Resources["Background"] ?? Colors.Black);
        private readonly Color dynamicPrimaryColor = (Color)Application.Current.Resources["Primary"];
        private readonly Color dynamicSecondaryColor = (Color)Application.Current.Resources["Secondary"];
        private readonly Color dynamicTertiaryColor = (Color)Application.Current.Resources["Tertiary"];
        private bool hapticFeedbackEnabled = true;
        //button.SetDynamicResource(Button.BackgroundProperty, "Primary");

        public MainPage()
        {
            InitializeComponent();

            // Show popup of the build
            //this.ShowPopup(new Pages.AlphaPopup());

            //this.ShowPopup(new Pages.TutorialPopup());

#if WINDOWS
    //COLUMNS = 21;
MakeNumberMatchGrid(ROWS + 8, COLUMNS + 5);
            game = new GameBackend(ROWS + 8, COLUMNS + 5, this);
#else
            MakeNumberMatchGrid(COLUMNS, ROWS);
            game = new GameBackend(COLUMNS, ROWS, this);

            game.CheckStageCompletion();


            /*this.Behaviors.Add(new StatusBarBehavior
            {
                StatusBarColor = Colors.Red,
                StatusBarStyle = StatusBarStyle.LightContent
            });*/
#endif

            SynchronizeGrid(game.gameData.GameGrid);
            LoadSettings();
            //AddBtn.Padding = new Thickness(0);
        }

        // Load and enable the app settings
        public void LoadSettings()
        {
            //  set the app theme
            if (Preferences.Get("OledDarkmode", false))
                this.SetAppTheme(BackgroundColorProperty, dynamicBackgroundColor, Colors.Black);
            else
                this.SetAppTheme(BackgroundColorProperty, dynamicBackgroundColor, dynamicBackgroundColor);
            
            //  set the vibration
            if (Preferences.ContainsKey("Vibration"))
                hapticFeedbackEnabled = Preferences.Get("Vibration", true);
        }

        private void MakeNumberMatchGrid(int columns, int rows) //  make the grid with buttons
        {
            for (int i = 0; i < rows; i++)
                NumberMatchGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            for (int i = 0; i < columns; i++)
                NumberMatchGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    Button tile = new Button
                    {
                        //CornerRadius = 0, //  maybe more modern with rounded corners
                        //BorderColor = dynamicPrimaryColor,
                        //BackgroundColor = dynamicBackgroundColor,
                        //TextColor = dynamicPrimaryColor,
                        BorderWidth = 2,
                        // set the fontsize to large
                        FontSize = 30,
                        Padding = new Thickness(0), // Remove padding
                    };

                    tile.SetDynamicResource(Button.BorderColorProperty, "Primary");
                    tile.SetDynamicResource(Button.TextColorProperty, "Primary");
                    tile.SetDynamicResource(Button.BackgroundColorProperty, "Background");

                    tile.Clicked += GridButtonClicked;

                    NumberMatchGrid.Children.Add(tile);

                    Grid.SetRow(tile, row);
                    Grid.SetColumn(tile, col);
                }
            }
        }

        //  finished
        private void SynchronizeGrid(List<List<int>> gameGrid)  //  synchronize the grid from the backend with the gameGrid from frontend, show the values
        {
            for (int row = 0; row < gameGrid.Count; row++)
            {
                for (int col = 0; col < gameGrid[row].Count; col++)
                {
                    Button tile = (Button)NumberMatchGrid.Children[row * COLUMNS + col];

                    if (gameGrid[row][col] == 0)
                        tile.Text = null;
                    else
                        tile.Text = gameGrid[row][col].ToString();
                }
            }

            // set the tiles under the active grid to the value of null
            for (int row = gameGrid.Count; row < ROWS; row++)
            {
                for (int col = 0; col < COLUMNS; col++)
                {
                    Button tile = (Button)NumberMatchGrid.Children[row * COLUMNS + col];
                    tile.Text = null;
                }
            }

            //set numbersmatched and stage
            LabelAmmountMatchedNumbers.Text = "Matched numbers: " + game.gameData.NumbersMatched;
            LabelStage.Text = "Stage: " + game.gameData.Stage;
        }

        private async void GridButtonClicked(object sender, EventArgs e)
        {
            try
            {
                Button tile = (Button)sender;

                // Uncheck button if checked, don't check for match
                if (tile.BackgroundColor == dynamicPrimaryColor)
                {
                    Tools.HapticClick(hapticFeedbackEnabled);
                    
                    tile.BackgroundColor = dynamicBackgroundColor;
                    tile.TextColor = dynamicPrimaryColor;

                    if (Grid.GetRow(tile) == previousPressedButton.Item1 && Grid.GetColumn(tile) == previousPressedButton.Item2)
                    {
                        previousPressedButton = null;
                    }
                }

                // Check for match if the button is initialized
                else if (tile.Text != null)
                {
                    Tools.HapticClick(hapticFeedbackEnabled);
                    
                    int row = Grid.GetRow(tile);
                    int col = Grid.GetColumn(tile);

                    tile.BackgroundColor = dynamicPrimaryColor;
                    tile.TextColor = dynamicBackgroundColor;

                    if (previousPressedButton == null) // first tile pressed
                    {
                        previousPressedButton = new Tuple<int, int>(row, col);
                    }
                    else // second tile pressed
                    {
                        if (await game.CheckMatch(previousPressedButton.Item1, previousPressedButton.Item2, row, col))
                        {
                            SynchronizeGrid(game.GetGameGrid());

                            LabelAmmountMatchedNumbers.Text = "Matched numbers: " + game.gameData.NumbersMatched;

                            ///////////ShowToast("Matched");
                        }
                        else
                        {
                            // shake the buttons that are not a match
                            await shakeUnmatchedButtons(previousPressedButton, new Tuple<int, int>(row, col));
                        }

                        previousPressedButton = null;

                        // uncheck the selected tiles
                        foreach (Button b in NumberMatchGrid.Children)
                        {
                            if (b.BackgroundColor == dynamicPrimaryColor)
                            {
                                b.BackgroundColor = dynamicBackgroundColor;
                                b.TextColor = dynamicPrimaryColor;
                            }
                        }
                    }
                }
                
                game.SaveGameData();
            }
            catch (Exception ex)
            {
                ShowToast(ex.Message);
            }
        }
        
        public void RefreshGridColors()
        {
            foreach (Button tile in NumberMatchGrid.Children.Cast<Button>())
            {
                if (tile.BackgroundColor == dynamicPrimaryColor)
                    tile.BackgroundColor = dynamicPrimaryColor;
            }
        }

        private void AddButtonClicked(object sender, EventArgs e)
        {
            Tools.HapticClick(hapticFeedbackEnabled);
            game.AddNumbersToGrid();
            SynchronizeGrid(game.GetGameGrid());
        }

        private void HelpButtonClicked(object sender, EventArgs e)
        {
            Tools.HapticClick(hapticFeedbackEnabled);
            this.ShowPopup(new Pages.TutorialPopup());
        }

        private void ResetButtonClicked(object sender, EventArgs e)
        {
            Tools.HapticClick(hapticFeedbackEnabled);
            
#if WINDOWS
            game.InitializeGrid(ROWS + 8, COLUMNS + 5);
#else
            game.InitializeGrid(COLUMNS, ROWS);
#endif
            game.gameData.NumbersMatched = 0;
            game.gameData.Stage = 0;
            SynchronizeGrid(game.gameData.GameGrid);
            game.SaveGameData();
        }

        private void SettingsButtonClicked(object sender, EventArgs e)
        {
            Tools.HapticClick(hapticFeedbackEnabled);
            this.ShowPopup(new Pages.Popups.SettingsPopup(this));
        }
        
        private async Task shakeUnmatchedButtons(Tuple<int, int> previousPressedButton, Tuple<int, int> currentPressedButton)
        {
            // shake the buttons that are not a match horizontally
            //var row = NumberMatchGrid[rowIndex];
            Tools.ErrorHaptic(hapticFeedbackEnabled);
            for (int col = 0; col < 3; col++)
            {
                // Assuming you have a method to get the UI element for a specific cell
                var cell1 = GetCellUIElement(previousPressedButton.Item1, previousPressedButton.Item2);
                var cell2 = GetCellUIElement(currentPressedButton.Item1, currentPressedButton.Item2);
                if (cell1 != null && cell2 != null)
                {
                    await cell1.TranslateTo(0 - 5, 0, 20); // Move up
                    await cell2.TranslateTo(0 - 5, 0, 20); // Move up
                    await cell1.TranslateTo(5, 0, 20); // Move down
                    await cell2.TranslateTo(5, 0, 20); // Move down
                    await cell2.TranslateTo(0, 0, 20); // Move back to original position
                    await cell1.TranslateTo(0, 0, 20); // Move back to original position
                    //HapticClick();
                }
            }
        }

        public async Task AnimateWaveEffect(int rowIndex)
        {
            int centerCol = COLUMNS / 2;

            for (int distance = 0; distance <= centerCol; distance++)
            {
                List<Task> animationTasks = new List<Task>();

                if (centerCol - distance >= 0)
                {
                    var leftCell = GetCellUIElement(rowIndex, centerCol - distance);
                    if (leftCell != null)
                    {
                        animationTasks.Add(AnimateCell(leftCell));
                    }
                }

                if (centerCol + distance < COLUMNS)
                {
                    var rightCell = GetCellUIElement(rowIndex, centerCol + distance);
                    if (rightCell != null)
                    {
                        animationTasks.Add(AnimateCell(rightCell));
                    }
                }

                await Task.WhenAll(animationTasks);
            }
        }

        private async Task AnimateCell(View cell)
        {
            await cell.TranslateTo(0, -10, 25); // Move up
            await cell.TranslateTo(0, 10, 25);  // Move down
            await cell.TranslateTo(0, 0, 25);   // Move back to original position
            await Tools.HapticClick(hapticFeedbackEnabled);
        }

        // Example method to get the UI element for a specific cell
        private View GetCellUIElement(int row, int col)
        {
            // Assuming NumberMatchGrid is a Grid defined in MainPage.xaml
            // and each cell is added with a specific name or tag to identify its position
            foreach (Microsoft.Maui.Controls.View child in NumberMatchGrid.Children)
            {
                if (NumberMatchGrid.GetRow(child) == row && NumberMatchGrid.GetColumn(child) == col)
                {
                    return child;
                }
            }
            return null;
        }

        /*private async Task HapticClick()
        {
#if __MOBILE__
            if(hapticFeedbackEnabled)
                HapticFeedback.Default.Perform(HapticFeedbackType.Click);
#endif
        }

        private async Task ErrorHaptic()
        {
#if __MOBILE__
            if (hapticFeedbackEnabled)
            {
                HapticFeedback.Default.Perform(HapticFeedbackType.Click);
                await Task.Delay(100);
                HapticFeedback.Default.Perform(HapticFeedbackType.Click);
                await Task.Delay(150);
                HapticFeedback.Default.Perform(HapticFeedbackType.Click);
            }
#endif
        }*/
        
        
        
        public async Task AnimateStageCompletion()
        {
            int centerRow = ROWS / 2;
            int centerCol = COLUMNS / 2;

            for (int distance = 0; distance <= Math.Max(centerRow, centerCol); distance++)
            {
                List<Task> animationTasks = new List<Task>();

                for (int row = Math.Max(0, centerRow - distance); row <= Math.Min(ROWS - 1, centerRow + distance); row++)
                {
                    for (int col = Math.Max(0, centerCol - distance); col <= Math.Min(COLUMNS - 1, centerCol + distance); col++)
                    {
                        if (Math.Abs(centerRow - row) == distance || Math.Abs(centerCol - col) == distance)
                        {
                            var cell = GetCellUIElement(row, col);
                            if (cell != null)
                            {
                                animationTasks.Add(AnimateCell(cell));
                            }
                        }
                    }
                }

                await Task.WhenAll(animationTasks);
            }
        }

        /*private async Task AnimateCell(View cell)
        {
            await cell.TranslateTo(0, -10, 50); // Move up
            await cell.TranslateTo(0, 10, 50);  // Move down
            await cell.TranslateTo(0, 0, 50);   // Move back to original position
        }*/
        
        
    }
}