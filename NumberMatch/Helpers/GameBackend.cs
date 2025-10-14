using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Controls;
using static NumberMatch.Helpers.Tools;
using System.Linq.Expressions;
using NumberMatch.Data;

namespace NumberMatch.Helpers
{
    public class GameBackend
    {
        public GameData gameData { get; private set; } = new GameData();
        private Tuple<int, int> gridsize;
        private readonly MainPage page;

        public GameBackend(int columns, int rows, MainPage mainpage)
        {
            this.page = mainpage;

            gridsize = new Tuple<int, int>(columns, rows);

            LoadGameData(columns, rows);
        }

        // getter for the gamegrid used to sync with the grid in the mainpage
        public List<List<int>> GetGameGrid()
        {
            return gameData.GameGrid;
        }

        //  initialize the grid with random numbers
        public void InitializeGrid(int columns, int rows)
        {
            Random random = new Random();

            // Remove existing grid
            gameData.GameGrid = new List<List<int>>();

            for (int i = 0; i < 5; i++)
            {
                List<int> row = new List<int>();

                for (int j = 0; j < columns; j++)
                    row.Add(random.Next(1, 10));

                gameData.GameGrid.Add(row);
            }

            // removes a number from the grid if the amount of numbers isn't even
            int amountOfNumbers = gameData.GameGrid.SelectMany(x => x).Count(n => n != 0);

            if (amountOfNumbers % 2 != 0)
            {
                int row = random.Next(0, 5);
                int col = random.Next(0, columns);

                while (gameData.GameGrid[row][col] == 0)
                {
                    row = random.Next(0, 5);
                    col = random.Next(0, columns);
                }

                gameData.GameGrid[row][col] = 0;
                //amountOfNumbers--;
            }

            //ShowToast($"Amount of numbers: {amountOfNumbers}");
        }

        //  save game data
        public void SaveGameData()
        {
            Preferences.Set("numbersMatched", gameData.NumbersMatched);
            Preferences.Set("stage", gameData.Stage);
            Preferences.Set("gameGrid", ConvertGameGridToJsonString(gameData.GameGrid));
        }

        //  load game data
        public void LoadGameData(int columns, int rows)
        {
            gameData.NumbersMatched = Preferences.Get("numbersMatched", 0);
            gameData.Stage = Preferences.Get("stage", 1);
            string gameGridJson = Preferences.Get("gameGrid", "");

            if (string.IsNullOrEmpty(gameGridJson))
            {
                InitializeGrid(columns, rows);
            }
            else
            {
                gameData.GameGrid = ConvertJsonStringToGameGrid(gameGridJson);
            }
        }

        //  convert the game grid to a json string
        private static string ConvertGameGridToJsonString(List<List<int>> gameGrid)
        {
            return JsonConvert.SerializeObject(gameGrid);
        }

        //  convert the json string to a game grid
        private List<List<int>> ConvertJsonStringToGameGrid(string GridJsonString)
        {
            return JsonConvert.DeserializeObject<List<List<int>>>(GridJsonString);
        }

        //  check if the numbers match, gets the position of the numbers as arguments
        public async Task<bool> CheckMatch(int row1, int col1, int row2, int col2)
        {
            //  check if the numbers match
            if ((gameData.GameGrid[row1][col1] == gameData.GameGrid[row2][col2]) || (gameData.GameGrid[row1][col1] + gameData.GameGrid[row2][col2] == 10))
            {
                //  check if the numbers are at the right position to match
                if (HorizontalMatch(row1, col1, row2, col2) || VerticalMatch(row1, col1, row2, col2) || DiagonalMatch(row1, col1, row2, col2) || CheckForMatch(row1, col1, row2, col2))
                {
                    //  remove the numbers from the grid
                    gameData.GameGrid[row1][col1] = 0;
                    gameData.GameGrid[row2][col2] = 0;

                    gameData.NumbersMatched++;

                    await RemoveEmptyRows();
                    await CheckStageCompletion();
                    //await RemoveEmptyRows();

                    return true;
                }
            }

            return false;
        }

        private bool HorizontalMatch(int row1, int col1, int row2, int col2)
        {
            // Check if the tiles are on the same row
            if (row1 == row2)
            {
                // Ensure col1 is the leftmost column
                if (col2 < col1)
                    (col2, col1) = (col1, col2);

                // Check the tiles between the two given tiles
                for (int col = col1 + 1; col < col2; col++)
                    if (gameData.GameGrid[row1][col] != 0)
                        return false;

                return true;
            }

            return false;
        }

        private bool VerticalMatch(int row1, int col1, int row2, int col2)
        {
            // Check if the tiles are in the same column
            if (col1 == col2)
            {
                // Ensure row1 is the topmost row
                if (row2 < row1)
                    (row2, row1) = (row1, row2);

                // Check the tiles between the two given tiles
                for (int row = row1 + 1; row < row2; row++)
                    if (gameData.GameGrid[row][col1] != 0)
                        return false;

                return true;
            }

            return false;
        }

        private bool DiagonalMatch(int row1, int col1, int row2, int col2)
        {
            // Check if the tiles are on the same diagonal
            if (Math.Abs(row1 - row2) == Math.Abs(col1 - col2))
            {
                // Ensure row1 is the topmost row
                if (row2 < row1)
                {
                    (row2, row1) = (row1, row2);
                    (col2, col1) = (col1, col2);
                }

                // Check the tiles between the two given tiles
                int rowIncrement = (row2 > row1) ? 1 : -1;
                int colIncrement = (col2 > col1) ? 1 : -1;

                for (int i = 1; i < Math.Abs(row1 - row2); i++)
                {
                    if (gameData.GameGrid[row1 + i * rowIncrement][col1 + i * colIncrement] != 0)
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        private bool CheckForMatch(int row1, int col1, int row2, int col2)
        {
            //check if the rows are next to each other  and the numbers aren't on the same row
            if (Math.Abs(row1 - row2) == 1) // check if the rows are next to eachother
            {
                int higherRow = Math.Min(row1, row2);
                int lowerRow = Math.Max(row1, row2);
                int colHigherRow = (row1 > row2) ? col2 : col1;
                int colLowerRow = (row1 < row2) ? col2 : col1;

                // check if the numbers from the columns before the higher rows selected column are 0
                if (gameData.GameGrid[higherRow].Take(colHigherRow).All(x => x == 0))
                {
                    // check if the numbers from the columns after the lower rows selected column are 0
                    if (gameData.GameGrid[lowerRow].Skip(colLowerRow + 1).All(x => x == 0))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        //  remove empty rows and show an animation
        public async Task RemoveEmptyRows()
        {
            for (int i = gameData.GameGrid.Count - 1; i >= 0; i--)
            {
                if (gameData.GameGrid[i].All(x => x == 0))
                {
                    await page.AnimateWaveEffect(i);
                    gameData.GameGrid.RemoveAt(i);
                }
            }
        }

        // Check if the grid is empty and add new numbers to the grid and increment the stage
        public async Task CheckStageCompletion()
        {
            // check if every number is matched and every number in the grid is 0
            if (gameData.GameGrid.All(x => x.All(y => y == 0)) || !gameData.GameGrid.Any())
            {
                gameData.Stage++;
                ShowToast("Stage completed!");
                await page.AnimateStageCompletion();
                InitializeGrid(gridsize.Item1, gridsize.Item2);
            }
        }

        // needs work
        public void AddNumbersToGrid()
        {
            Random random = new Random();
            int maxColumns = gridsize.Item1;
            int maxRows = gridsize.Item2;
            int rowsToAdd = (maxRows - gameData.GameGrid.Count) / 3;

            for (int i = 0; i < rowsToAdd; i++)
            {
                List<int> row = new List<int>();

                for (int j = 0; j < maxColumns; j++)
                {
                    int number = random.Next(1, 10);

                    while (!gameData.GameGrid.SelectMany(x => x).Contains(number))
                        number = random.Next(1, 10);

                    row.Add(number);
                }

                // Ensure the row has an even number of elements but does not exceed maxColumns
                if (row.Count % 2 != 0 && row.Count < maxColumns)
                {
                    int number = random.Next(1, 10);

                    while (!gameData.GameGrid.SelectMany(x => x).Contains(number))
                        number = random.Next(1, 10);

                    row.Add(number);
                }

                if (i == rowsToAdd - 1)
                {
                    //int number = random.Next(1, 10);

                    //while (!gameData.GameGrid.SelectMany(x => x).Contains(number))
                    //number = random.Next(1, 10);

                    //row.Add(number);

                    // check if the total amount of numbers in the grid is even and remove a number from the row if it isn't
                    int amountOfNumbers = gameData.GameGrid.SelectMany(x => x).Count(n => n != 0);
                    int amountOfRowNumbers = row.Count(n => n != 0);

                    if ((amountOfNumbers % 2 != 0 && amountOfRowNumbers % 2 == 0) || (amountOfNumbers % 2 == 0 && amountOfRowNumbers % 2 != 0))
                    {
                        int col = random.Next(0, maxColumns);

                        while (row[col] == 0)
                            col = random.Next(0, maxColumns);

                        row[col] = 0;
                    }
                }

                gameData.GameGrid.Add(row);
            }
        }
    }
}