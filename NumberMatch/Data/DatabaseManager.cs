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
        private readonly string dbPath = Path.Combine(FileSystem.AppDataDirectory, "gamedata.db3");

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