using Newtonsoft.Json;
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
                    Task.Run(() => LoadMonsterNames()).Wait();
                }
                return _monsterIndexNames;
            } 
        }

        private static async Task<string[]> LoadMonsterNames()
        {
            HttpClient httpClient = new();
            HttpRequestMessage request = new(HttpMethod.Get, "https://www.dnd5eapi.co/api/monsters");
            request.Headers.Add("Accept", "application/json");
            HttpResponseMessage response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string responseJson = await response.Content.ReadAsStringAsync();

            AllMonstersResponse deserializedResponse = JsonConvert.DeserializeObject<AllMonstersResponse>(responseJson);
            
            _monsterIndexEntries = new(deserializedResponse.Monsters);

            // Initialize array to size of all monsters
            _monsterIndexNames = new string[_monsterIndexEntries.Count];
            // Fill array with monster names
            for(int i = 0; i< _monsterIndexEntries.Count; i++)
            {
                _monsterIndexNames[i] = _monsterIndexEntries[i].Name;
            }
            
            return _monsterIndexNames;
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
