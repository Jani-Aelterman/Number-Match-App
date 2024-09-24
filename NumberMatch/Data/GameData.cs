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
        public string gameGridJson { get; set; } = "";
        public int Stage { get; set; } = 0;
        public int NumbersMatched { get; set; } = 0;

        [Ignore]
        public List<List<int>> Gamegrid
        {
            get => JsonConvert.DeserializeObject<List<List<int>>>(gameGridJson);
            set => gameGridJson = JsonConvert.SerializeObject(value);
        }
    }

    /*public class Person
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }*/
}