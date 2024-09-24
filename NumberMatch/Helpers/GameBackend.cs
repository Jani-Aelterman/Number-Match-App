using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Controls;
using SQLite;
using NumberMatch.Data;
using static NumberMatch.Helpers.Tools;
using static SQLite.TableMapping;

namespace NumberMatch.Helpers
{
    public class GameBackend
    {
        private List<List<int>> gameGrid = [];
        public int Stage { get; private set; } = 0;
        public int NumbersMatched { get; private set; } = 0;

        private DatabaseManager manager = new DatabaseManager();

        private GameData gameData;

        private MainPage page;

        public GameBackend(int columns, int rows, MainPage mainpage)
        {
            this.page = mainpage;

            //InitializeGrid(columns, rows);

            LoadData(columns, rows);

            /*gameData = manager.LoadGameData();

            if (manager.LoadGameData() != null)
            {
                gameData = manager.LoadGameData();

                if (gameData.Gamegrid != null)
                {
                    gameGrid = gameData.Gamegrid;
                }
                else
                {
                    InitializeGrid(columns, rows);
                }

                Stage = gameData.Stage;
                NumbersMatched = gameData.NumbersMatched;

                ShowToast("DEBUG: Game loaded");
            }
            else
            {
                InitializeGrid(columns, rows);

                ShowToast("DEBUG: Game not loaded");
            }*/


            //check if there is saved data
            /*if (Preferences.ContainsKey("gameData"))
            {
                ///////////Tools.ShowToast("DEBUG: GameData found");
                LoadData();
            }*/

            //manager.SaveGameData(new GameData { Stage = this.Stage, NumbersMatched = this.NumbersMatched, Gamegrid = this.gameGrid});
        }

        public List<List<int>> GetGameGrid()
        {
            return gameGrid;
        }

        private void InitializeGrid(int columns, int rows)
        {
            Random random = new Random();

            for (int i = 0; i < 5; i++)
            {
                List<int> row = new List<int>();

                for (int j = 0; j < columns; j++)
                    row.Add(random.Next(1, 10));

                gameGrid.Add(row);
            }
        }

        //  save game data
        public void SaveData()
        {
            // Create a dictionary to store the game data
            //////////////Dictionary<string, object> gameData = new Dictionary<string, object>();
            ///////////////gameData["gameGrid"] = gameGrid;
            ////////////gameData["stage"] = Stage;
            ////////////gameData["numbersMatched"] = NumbersMatched;

            // Serialise the game data to a JSON string
            ////string gameDataJson = JsonConvert.SerializeObject(gameData);

            // Save the JSON string
            ////////////Preferences.Set("gameData", gameDataJson);
            //////////////Preferences.Set("gameGrid", gameGrid);
            //////////Preferences.Set("stage", Stage);
            //////////////Preferences.Set("numbersMatched", NumbersMatched);


            //string gameDataJson = ConvertToJsonString(gameGrid, Stage, NumbersMatched);
            //Preferences.Set("gameData", gameDataJson);



            // Save the game data to the database
            manager.SaveGameData(new GameData { Stage = this.Stage, NumbersMatched = this.NumbersMatched, Gamegrid = this.gameGrid });



            ShowToast("DEBUG: Game saved");
        }

        //  load saved game data
        /*public void LoadData()
        {
            // Retrieve the JSON string
            string retrievedJson = Preferences.Get("gameData", "");

            // Deserialize the JSON string back into game data dictionary
            Dictionary<string, object> retrievedGameData = JsonConvert.DeserializeObject<Dictionary<string, object>>(retrievedJson);

            // Retrieve the game grid, stage, and numbers matched from the dictionary
            try
            {
                var temp = retrievedGameData.ContainsKey("gameGrid");
                //gameGrid = temp ? (List<List<int>>)retrievedGameData["gameGrid"] : new List<List<int>>();
            }
            catch (Exception e)
            {
                Tools.ShowToast("DEBUG: " + e.Message);
            }
            //////////////////Tools.ShowToast("DEBUG: " + retrievedGameData["gameGrid"]);
            //////////////////////string gameGridString = retrievedGameData["gameGrid"].ToString();
            ///////////////////////////////Tools.ShowToast(gameGridString);
            //////////////Stage = (int)retrievedGameData["stage"];
            ///////////////NumbersMatched = (int)retrievedGameData["numbersMatched"];

            ///////////Tools.ShowToast("DEBUG: Game loaded");
        }*/



        public void LoadData(int columns, int rows)
        {
            // Retrieve the JSON string
            /*string retrievedJson = Preferences.Get("gameData", "");

            /////////Shell.Current.DisplayAlert("DEBUG", $"Game loaded: {retrievedJson}", "OK");

            // Deserialize the JSON string back into game data dictionary
            Dictionary<string, object> retrievedGameData = JsonConvert.DeserializeObject<Dictionary<string, object>>(retrievedJson);

            StringBuilder sb = new StringBuilder();

            foreach (var item in retrievedGameData)
            {
                //sb.Append($"{item.Key}: {item.Value}\n");
                sb.Append(item.Key);

                if (item.Key == "gameGrid")
                {
                    //Shell.Current.DisplayAlert("DEBUG", $"GameGrid found: {item.Value}", "OK");

                    //page.ShowPopup = new Pages.PopupPage(item.Value.ToString());
                    /////////////page.ShowPopup($"GameGrid found: {item.Value}");

                    if (item.Value is List<List<int>>)
                        gameGrid = item.Value as List<List<int>>;
                    ///////////else
                        ////////////////Tools.ShowToast($"GameGrid not found as List<List<int>>");
                }
            }*/




            gameData = manager.LoadGameData();

            if (manager.LoadGameData() != null)
            {
                gameData = manager.LoadGameData();

                if (gameData.Gamegrid != null)
                {
                    gameGrid = gameData.Gamegrid;
                }
                else
                {
                    InitializeGrid(columns, rows);
                }

                Stage = gameData.Stage/* ?? 0*/;
                NumbersMatched = gameData.NumbersMatched/* ?? 0*/;

                ShowToast("DEBUG: Game loaded");
            }
            else
            {
                InitializeGrid(columns, rows);

                ShowToast("DEBUG: Game not loaded");
            }



            //Shell.Current.DisplayAlert("DEBUG", $"Game loaded: {sb.ToString()}", "OK");
            ////////////////page.ShowPopup($"Game loaded: {sb.ToString()}");

            // Retrieve the game grid, stage, and numbers matched from the dictionary
            /*var retrievedGrid = retrievedGameData["gameGrid"] as List<List<int>>;
            if (retrievedGrid != null)
            {
                gameGrid = retrievedGrid;
            }
            else
            {
                // Handle the case where no game data is found (e.g., initialize a new grid)
                //Tools.ShowToast($"DEBUG: GameGrid not found: {retrievedGrid}");
                Shell.Current.DisplayAlert("DEBUG", $"GameGrid not found: {retrievedGrid}", "OK");
            }*/
            //Stage = (int)retrievedGameData["stage"];
            //NumbersMatched = (int)retrievedGameData["numbersMatched"];

            //Tools.ShowToast($"DEBUG: Game loaded: {(int)retrievedGameData["stage"]}, {(int)retrievedGameData["numbersMatched"]}");




            //bool saveLoginDetails = ...;
            //...
            //Preferences.Set("SaveLogin", saveLoginDetails);
            //...
            //var savedPreference = Preferences.Get("SaveLogin", false);
        }




        private static string ConvertToJsonString(List<List<int>> gameGrid, int Stage, int NumbersMatched)
        {
            // Construct the JSON object.
            var saveData = new Dictionary<string, object>
            {
                { "gameGrid", gameGrid },
                { "stage", Stage },
                { "NumbersMatched", NumbersMatched },
            };

            // Convert the JSON object to a string.
            string jsonSaveData = JsonConvert.SerializeObject(saveData);

            return jsonSaveData;
        }






        /*async Task SaveData()
        {
            // Convert game data to JSON
            string gameDataJson = ConvertToJsonString(gameGrid, Stage, NumbersMatched);

            // Get a storage file object
            IStorageFile file = await FileSystem.Current.GetFileAsync("gameData.json");

            // Write JSON data to the file
            await file.WriteAllTextAsync(gameDataJson);
        }*/







        //  check if the numbers match, gets the position of the numbers as arguments
        public bool CheckMatch(int row1, int col1, int row2, int col2)
        {
            //  check if the numbers match
            if ((gameGrid[row1][col1] == gameGrid[row2][col2]) || (gameGrid[row1][col1] + gameGrid[row2][col2] == 10))
            {
                //  check if the numbers are at the right position to match
                if (CheckAdjacent(row1, col1, row2, col2))
                {
                    //  remove the numbers from the grid
                    gameGrid[row1][col1] = 0;
                    gameGrid[row2][col2] = 0;

                    NumbersMatched ++;

                    return true;
                }
            }

            return false;
        }

        //  check if the numbers are next to eachother
        /*private static bool CheckAdjacent(int row1, int col1, int row2, int col2)
        {
            bool adjecent = (row1 == row2 && Math.Abs(col1 - col2) == 1) || (col1 == col2 && Math.Abs(row1 - row2) == 1);
            bool diagonal = (Math.Abs(row1 - row2) == 1 && Math.Abs(col1 - col2) == 1);
            
            if (adjecent || diagonal)
                    return true;

            return false;
        }*/




        private bool CheckAdjacent(int row1, int col1, int row2, int col2)
        {
            // Check if the tiles are on the same row
            if (row1 == row2)
            {
                // Ensure col1 is the leftmost column
                if (col2 < col1)
                    (col2, col1) = (col1, col2);

                // Check the tiles between the two given tiles
                for (int col = col1 + 1; col < col2; col++)
                    if (gameGrid[row1][col] != 0)
                        return false;
            }
            // Check if the tiles are in the same column
            else if (col1 == col2)
            {
                // Ensure row1 is the topmost row
                if (row2 < row1)
                    (row2, row1) = (row1, row2);

                // Check the tiles between the two given tiles
                for (int row = row1 + 1; row < row2; row++)
                    if (gameGrid[row][col1] != 0)
                        return false;
            }
            // Check if the tiles are on the same diagonal
            else if (Math.Abs(row1 - row2) == Math.Abs(col1 - col2))
            {
                // Ensure row1 is the topmost row
                if (row2 < row1)
                {
                    (row2, row1) = (row1, row2);
                    (col2, col1) = (col1, col2);
                }

                // Check the tiles between the two given tiles
                for (int i = 1; i < Math.Abs(row1 - row2); i++)
                    if (gameGrid[row1 + i][col1 + i] != 0) // can give a index out of range error
                        return false;
            }
            else
            {
                // The tiles are not on the same row, column, or diagonal
                return false;
            }

            // All tiles between the two given tiles are empty
            return true;
        }

        public void RemoveEmptyRowsAndShiftUp()
        {
            for (int i = 0; i < gameGrid.Count; i++)
                if (gameGrid[i].All(x => x == 0))
                    gameGrid.RemoveAt(i);
        }

        // add numbers to the grid containing only numbers that are already in the grid
        public void AddNumbersToGrid()
        {
            Random random = new Random();

            int rowsToAdd = 5 - gameGrid.Count;

            // If rowsToAdd is odd, increment it by 1 to make it even
            /*if (rowsToAdd % 2 != 0)
            {
                rowsToAdd++;
            }*/

            for (int i = 0; i < rowsToAdd; i++)
            {
                List<int> row = new List<int>();

                for (int j = 0; j < gameGrid[0].Count; j++)
                {
                    int number = random.Next(1, 10);

                    while (!gameGrid.SelectMany(x => x).Contains(number))
                        number = random.Next(1, 10);

                    row.Add(number);
                }

                // If the row contains an odd number of elements, add one more
                if (row.Count % 2 != 0)
                {
                    int number = random.Next(1, 10);

                    while (!gameGrid.SelectMany(x => x).Contains(number))
                        number = random.Next(1, 10);

                    row.Add(number);
                }

                gameGrid.Insert(0, row);
            }
        }
    }
}