using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Controls;

namespace NumberMatch.Helpers
{
    public class GameBackend
    {
        private List<List<int>> gameGrid = new List<List<int>>();
        public int Stage { get; private set; } = 0;
        public int NumbersMatched { get; private set; } = 0;

        private MainPage page;

        public GameBackend(int columns, int rows, MainPage mainpage)
        {
            this.page = mainpage;

            InitializeGrid(columns, rows);
            //check if there is saved data

            if (Preferences.ContainsKey("gameData"))
            {
                Tools.ShowToast("DEBUG: GameData found");
                LoadData();
            }
        }

        //private void InitializeGrid(int columns, int rows)
        //{
        //    Random random = new Random();
        //    int randomNumber = random.Next(1, 11);

        //    for (int i = 0; i < 5; i++)
        //    {
        //        for(int j = 0; j < columns; j++)
        //        {
        //            gameGrid[i][j] = random.Next(0, 9);
        //        }
        //    }
        //}

        private void InitializeGrid(int columns, int rows)
        {
            Random random = new Random();

            for (int i = 0; i < 5; i++)
            {
                List<int> row = new List<int>();

                for (int j = 0; j < columns; j++)
                {
                    row.Add(random.Next(1, 9));
                }

                gameGrid.Add(row);
            }
        }

        public List<List<int>> GetGameGrid()
        {
            return gameGrid;
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


            string gameDataJson = ConvertToJsonString(gameGrid, Stage, NumbersMatched);
            Preferences.Set("gameData", gameDataJson);



            Tools.ShowToast("DEBUG: Game saved");
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



        public void LoadData()
        {
            // Retrieve the JSON string
            string retrievedJson = Preferences.Get("gameData", "");

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
                    else
                        page.ShowPopup($"GameGrid not found as List<List<int>>");
                }
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




        private string ConvertToJsonString(List<List<int>> gameGrid, int Stage, int NumbersMatched)
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
                    page.ShowPopup("Matched by adjecent");

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
        private static bool CheckAdjacent(int row1, int col1, int row2, int col2)
        {
            bool adjecent = (row1 == row2 && Math.Abs(col1 - col2) == 1) || (col1 == col2 && Math.Abs(row1 - row2) == 1);
            bool diagonal = (Math.Abs(row1 - row2) == 1 && Math.Abs(col1 - col2) == 1);
            
            if (adjecent || diagonal)
                    return true;

            return false;
        }
    }
}