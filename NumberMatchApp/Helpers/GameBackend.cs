using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Maui.Storage;

namespace NumberMatchApp.Helpers
{
    public class GameBackend
    {
        private List<List<int>> gameGrid = new List<List<int>>();
        public int Stage { get; private set; } = 0;
        public int NumbersMatched { get; private set; } = 0;

        public GameBackend(int columns, int rows)
        {
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
                    row.Add(random.Next(0, 9));
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
            Dictionary<string, object> gameData = new Dictionary<string, object>();
            gameData["gameGrid"] = gameGrid;
            gameData["stage"] = Stage;
            gameData["numbersMatched"] = NumbersMatched;

            // Serialise the game data to a JSON string
            string gameDataJson = JsonConvert.SerializeObject(gameData);

            // Save the JSON string
            Preferences.Set("gameData", gameDataJson);

            Tools.ShowToast("DEBUG: Game saved");
        }

        //  load saved game data
        public void LoadData()
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
        }

        //  check if the numbers match, gets the position of the numbers as arguments
        public bool CheckMatch(int row1, int col1, int row2, int col2)
        {
            //  check if the numbers match
            if ((gameGrid[row1][col1] == gameGrid[row2][col2]) || (gameGrid[row1][col1] + gameGrid[row2][col2] == 10))
            {
                //  check if the numbers are at the right position to match
                if (CheckAdjacent(row1, col1, row2, col2))
                    return true;
            }

            return false;
        }

        //  check if the numbers are next to eachother
        private static bool CheckAdjacent(int row1, int col1, int row2, int col2)
        {
            return (row1 == row2 && Math.Abs(col1 - col2) == 1) || (col1 == col2 && Math.Abs(row1 - row2) == 1);
        }
    }
}