using NumberMatch.Helpers;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NumberMatch.Helpers.Tools;

namespace NumberMatch.Data
{
    public class DatabaseManager
    {
        private string dbPath = Path.Combine(FileSystem.AppDataDirectory, "gamedata.db3");

        public DatabaseManager()
        {
            GameData gameData = new GameData();

            // Opslaan
            //string dbPath = Path.Combine(FileSystem.AppDataDirectory, "gamedata.db3");
            //////ShowToast(dbPath);

            /*using (var db = new SQLiteConnection(dbPath))
            {
                //db.CreateTable<Person>(); // Creëer de tabel als deze nog niet bestaat
                //db.InsertAll(people);

                db.CreateTable<GameData>(); // Creëer de tabel als deze nog niet bestaat
                db.Insert(new GameData { Stage = 0, NumbersMatched = 0 });

            }*/

            // Ophalen
            /*using (var db = new SQLiteConnection(dbPath))
            {
                var gamedata = db.Table<GameData>().ToList();
                foreach (var data in gamedata)
                {
                    Console.WriteLine($"Name: {data.Stage}, Age: {data.NumbersMatched}");
                    ShowToast($"Stage: {data.Stage}, NumbersMatched: {data.NumbersMatched}");
                }
            }*/
        }

        public void SaveGameData(GameData gameData)
        {
            using (var db = new SQLiteConnection(dbPath))
            {
                var existingData = db.Table<GameData>().FirstOrDefault();
                if (existingData == null)
                {
                    db.Insert(gameData);
                }
                else
                {
                    db.Update(gameData);
                }
            }
        }


        public GameData LoadGameData()
        {
            using (var db = new SQLiteConnection(dbPath))
            {
                return db.Table<GameData>().FirstOrDefault();
            }
        }
    }
}