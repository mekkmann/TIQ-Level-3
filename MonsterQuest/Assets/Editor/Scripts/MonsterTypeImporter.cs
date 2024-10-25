using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using UnityEngine;

namespace MonsterQuest
{
    public static class MonsterTypeImporter
    {
        private static List<MonsterIndexEntry> _monsterIndexEntries;
        private static string[] _monsterIndexNames;
        public static string[] MonsterIndexNames { 
            get {

                if (_monsterIndexNames is null)
                {
                    LoadMonsterNames();
                }
                return _monsterIndexNames;
            } 
        }

        private static void LoadMonsterNames()
        {
            HttpClient httpClient = new();
            Task<string> requestTask = httpClient.GetStringAsync("https://www.dnd5eapi.co/api/monsters");
            string responseJson = requestTask.Result;

            AllMonstersResponse deserializedResponse = JsonConvert.DeserializeObject<AllMonstersResponse>(responseJson);
            
            _monsterIndexEntries = new(deserializedResponse.Monsters);

            // Initialize array to size of all monsters
            _monsterIndexNames = new string[_monsterIndexEntries.Count];
            // Fill array with monster names
            for(int i = 0; i< _monsterIndexEntries.Count; i++)
            {
                _monsterIndexNames[i] = _monsterIndexEntries[i].Name;
            }
        }

        public static void ImportData(string name, MonsterType monsterType)
        {
            // Get index from name
            string index = _monsterIndexEntries.First(entry => entry.Name == name).Index;

            // Http request and parsing
            HttpClient httpClient = new();
            Task<string> requestTask = httpClient.GetStringAsync($"https://www.dnd5eapi.co/api/monsters/{index}");
            string responseJson = requestTask.Result;
            JObject monsterData = JObject.Parse(responseJson);

            // Get data from monsterData
            string displayName = (string)monsterData["name"];
            SizeCategory sizeCategory = Enum.Parse<SizeCategory>((string)monsterData["size"], true);
            string alignment = (string)monsterData["alignment"];
            string hpRoll = (string)monsterData["hit_points_roll"];
            int strength = (int)monsterData["strength"];
            int dexterity = (int)monsterData["dexterity"];
            int constitution = (int)monsterData["constitution"];
            int intelligence = (int)monsterData["intelligence"];
            int wisdom = (int)monsterData["wisdom"];
            int charisma = (int)monsterData["charisma"];
            float challengeRating = (float)monsterData["challenge_rating"];

            // Add data to the scriptableObject (monsterType)
            monsterType.DisplayName = displayName;
            monsterType.SizeCategory = sizeCategory;
            monsterType.Alignment = alignment;
            monsterType.HpRoll = hpRoll;
            monsterType.AbilityScores = new(strength, dexterity, constitution, intelligence, wisdom, charisma);
            monsterType.ChallengeRating = challengeRating;

            Debug.Log($"Link to full Monster Data: [ https://www.dnd5eapi.co/api/monsters/{index} ]");
        }
        

    }
    public class AllMonstersResponse
    {
        [JsonProperty("count")]
        public int Count { get; set; }
        [JsonProperty("results")]
        public List<MonsterIndexEntry> Monsters { get; set; }
    }
    public class MonsterIndexEntry
    {
        [JsonProperty("index")]
        public string Index { get; set; }
        [JsonProperty("name")]
        public string Name {  get; set; }
    }
}
