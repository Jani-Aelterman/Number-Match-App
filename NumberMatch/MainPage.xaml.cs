﻿/// <summary>
/// The front end of the game.
/// Made by: Jani Aelterman.
/// </summary>

using CommunityToolkit.Maui.Views;
using NumberMatch.Helpers;
using System.Runtime.CompilerServices;
using static NumberMatch.Helpers.Tools;

namespace NumberMatch
{
    public partial class MainPage : ContentPage
    {
        private GameBackend game;
        private const int COLUMNS = 9, ROWS = 13;
        private Tuple<int, int> previousPressedButton = null; //  previous pressed button, row and column

        public MainPage()
        {
            InitializeComponent();

            // Show popup of the build
            this.ShowPopup(new Pages.AlphaPopup());

            //this.ShowPopup(new Pages.TutorialPopup());

#if WINDOWS
            MakeNumberMatchGrid(ROWS + 8, COLUMNS + 5);
            game = new GameBackend(ROWS + 8, COLUMNS + 5, this);
#else
            MakeNumberMatchGrid(COLUMNS, ROWS);
            game = new GameBackend(COLUMNS, ROWS, this);


            /*this.Behaviors.Add(new StatusBarBehavior
            {
                StatusBarColor = Colors.Red,
                StatusBarStyle = StatusBarStyle.LightContent
            });*/
#endif

            SynchronizeGrid(game.GetGameGrid());
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
                    Button button = new Button
                    {
                        //Text = $"Button {row * 14 + col + 1}",
                        //Text = col.ToString(),
                        //CornerRadius = 0, //  maybe more modern with rounded corners
                        BorderColor = (Color)Application.Current.Resources["Primary"],
                        BackgroundColor = (Color)Application.Current.Resources["Background"],
                        TextColor = (Color)Application.Current.Resources["Primary"],
                        BorderWidth = 2,
                        // set the fontsize to large
                        FontSize = 30,
                        Padding = new Thickness(0), // Remove padding
                    };

                    button.Clicked += GridButtonClicked;

                    NumberMatchGrid.Children.Add(button);

                    Grid.SetRow(button, row);
                    Grid.SetColumn(button, col);
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
                    Button button = (Button)NumberMatchGrid.Children[row * COLUMNS + col];

                    if (gameGrid[row][col] == 0)
                    {
                        button.Text = null;
                        button.BackgroundColor = (Color)Application.Current.Resources["Background"];
                    }
                    else
                        button.Text = gameGrid[row][col].ToString();
                }
            }


            // make every button under the gamegrid list background color
            /*for (int row = gameGrid.Count; row < ROWS; row++)
            {
                for (int col = 0; col < COLUMNS; col++)
                {
                    Button button = (Button)NumberMatchGrid.Children[row * COLUMNS + col];
                    button.BackgroundColor = (Color)Application.Current.Resources["Background"];
                    button.Text = null;
                }
            }*/

            for (int row = gameGrid.Count; row < ROWS; row++)
            {
                for (int col = 0; col < COLUMNS; col++)
                {
                    Button button = (Button)NumberMatchGrid.Children[row * COLUMNS + col];
                    button.BackgroundColor = (Color)Application.Current.Resources["Background"];
                    button.Text = null;
                }
            }



            //set numbersmatched and stage
            LabelAmmountMatchedNumbers.Text = "Matched numbers: " + game.NumbersMatched;
            LabelStage.Text = "Stage: " + game.Stage;
        }

        private void GridButtonClicked(object sender, EventArgs e)
        {
            //this.ShowPopup(new Pages.PopupPage());

            Button button = (Button)sender;

            // Uncheck button if checked, don't check for match
            if (button.BackgroundColor == (Color)Application.Current.Resources["Primary"])
            {
                button.BackgroundColor = (Color)Application.Current.Resources["Background"];
                button.TextColor = (Color)Application.Current.Resources["Primary"];

                if(Grid.GetRow(button) == previousPressedButton.Item1 && Grid.GetColumn(button) == previousPressedButton.Item2)
                {
                    previousPressedButton = null;
                }
            }

            // Check for match if the button is initialized
            else if (button.Text != null)
            {
                int row = Grid.GetRow(button);
                int col = Grid.GetColumn(button);

                button.BackgroundColor = (Color)Application.Current.Resources["Primary"];
                button.TextColor = (Color)Application.Current.Resources["Background"];

                if(previousPressedButton == null)
                {
                    previousPressedButton = new Tuple<int, int>(row, col);
                }
                else
                {
                    if (game.CheckMatch(previousPressedButton.Item1, previousPressedButton.Item2, row, col))
                    {
                        previousPressedButton = null;



                        // temp
                        foreach (Button b in NumberMatchGrid.Children)
                        {
                            if (b.BackgroundColor == (Color)Application.Current.Resources["Primary"])
                            {
                                b.BackgroundColor = (Color)Application.Current.Resources["Background"];
                                b.TextColor = (Color)Application.Current.Resources["Primary"];
                            }
                        }



                        game.RemoveEmptyRowsAndShiftUp();

                        SynchronizeGrid(game.GetGameGrid());

                        LabelAmmountMatchedNumbers.Text = "Matched numbers: " + game.NumbersMatched;

                        ShowToast("Matched");
                    }
                    else
                    {
                        previousPressedButton = null;

                        // uncheck the selected tiles
                        foreach (Button b in NumberMatchGrid.Children)
                        {
                            if (b.BackgroundColor == (Color)Application.Current.Resources["Primary"])
                            {
                                b.BackgroundColor = (Color)Application.Current.Resources["Background"];
                                b.TextColor = (Color)Application.Current.Resources["Primary"];
                            }
                        }

                        ShowToast("No match found");
                    }
                }

                game.SaveData();
            }
        }

        public void RefreshGridColors()
        {
            foreach (Button tile in NumberMatchGrid.Children)
            {
                if (tile.BackgroundColor == (Color)Application.Current.Resources["Primary"])
                    tile.BackgroundColor = (Color)Application.Current.Resources["Primary"];
            }
        }

        private void AddButtonClicked(object sender, EventArgs e)
        {
            //////////////this.ShowPopup(new Pages.NotImplementedPopup());

            game.AddNumbersToGrid();
            SynchronizeGrid(game.GetGameGrid());
        }

        private void HelpButtonClicked(object sender, EventArgs e)
        {
            //ShowToast("DEBUG: Help button clicked");
            //ShowPopup("Not implemented yet");
            this.ShowPopup(new Pages.TutorialPopup());
        }






        /*public void ShowPopup(String text)
        {
            this.ShowPopup(new Pages.PopupPage(text));
        }*/
    }
}