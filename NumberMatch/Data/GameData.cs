using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberMatch.Data
{
    public class GameData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string GameGridJson { get; set; } = "";
        public int Stage { get; set; } = 0;
        public int NumbersMatched { get; set; } = 0;

        /*[Ignore]
        public List<List<int>> GameGrid
        {
            get => JsonConvert.DeserializeObject<List<List<int>>>(GameGridJson);
            set => GameGridJson = JsonConvert.SerializeObject(value);
        }*/

        public List<List<int>> GameGrid;
    }
}