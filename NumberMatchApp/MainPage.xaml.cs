/// <summary>
/// The front end of the game.
/// Made by: Jani Aelterman.
/// </summary>

using NumberMatchApp.Helpers;
using static NumberMatchApp.Helpers.Tools;

namespace NumberMatchApp
{
    public partial class MainPage : ContentPage
    {
        private GameBackend game;
        private const int COLUMNS = 9, ROWS = 13;
        //private int pressedNumber = 0;
        private Tuple<int, int> previousPressedButton = null; //  previous pressed button, row and column

        public MainPage()
        {
            InitializeComponent();

#if WINDOWS
            MakeNumberMatchGrid(ROWS + 8, COLUMNS + 5);
            game = new GameBackend(ROWS + 8, COLUMNS + 5);
#else
                MakeNumberMatchGrid(COLUMNS, ROWS);
                game = new GameBackend(COLUMNS, ROWS);
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
                        BorderWidth = 2,
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
                    button.Text = gameGrid[row][col].ToString();
                }
            }

            //set numbersmatched and stage
            LabelAmmountMatchedNumbers.Text = "Matched numbers: " + game.NumbersMatched;
            LabelStage.Text = "Stage: " + game.Stage;
        }

        ///  <TODO>: implement the game logic
        private void GridButtonClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (button.BackgroundColor == (Color)Application.Current.Resources["Primary"])
                button.BackgroundColor = default;

            else if (button.Text != null)
            {
                int row = Grid.GetRow(button);
                int col = Grid.GetColumn(button);

                button.BackgroundColor = (Color)Application.Current.Resources["Primary"];

                //temporarily
                if (game.CheckMatch(previousPressedButton.Item1, previousPressedButton.Item2, row, col))
                {
                    // remove the values from the grid
                }

                game.SaveData();

                //if (pressedNumber == 0)
                //{
                //    pressedNumber++;

                //    button.BackgroundColor = (Color)Application.Current.Resources["Primary"];

                //    Tools.ShowToast($"Button clicked at row {row}, column {col}, {pressedNumber}");
                //}

                //if (pressedNumber == 1)
                //{
                //    button.BackgroundColor = (Color)Application.Current.Resources["Primary"];

                //    Thread.Sleep(1000);///////////////////////

                //    pressedNumber++;

                //    foreach (Button btn in numberMatchGrid)
                //    {
                //        btn.BackgroundColor = default;
                //    }

                //    //button.BackgroundColor = (Color)Application.Current.Resources["Primary"];

                //    pressedNumber = 0;

                //    Tools.ShowToast($"Button clicked at row {row}, column {col}, {pressedNumber}");
                //}

                //if (pressedNumber == 1)
                //{
                //    pressedNumber++;

                //    button.BackgroundColor = (Color)Application.Current.Resources["Primary"];

                //    Tools.ShowToast($"Button clicked at row {row}, column {col}, {pressedNumber}");
                //}
            }
        }

        private void AddButtonClicked(object sender, EventArgs e)
        {
            ShowToast("DEBUG: Add button clicked");
        }

        private void HelpButtonClicked(object sender, EventArgs e)
        {
            ShowToast("DEBUG: Help button clicked");
        }
    }
}