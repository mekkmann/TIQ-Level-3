using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace MonsterQuest
{
    public static class SaveGameHelper
    {
        private static readonly string _saveFilePath = Application.persistentDataPath + "/MonsterQuestSaveFile.txt";
        public static bool SaveFileExists { 
            get { 
                return File.Exists(_saveFilePath);
            } 
        }
        private static readonly JsonSerializerSettings _settings = new()
        {
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented,
            ContractResolver = new DefaultContractResolver
            {
                IgnoreSerializableAttribute = false
            },
            Converters = new List<JsonConverter>
            {
                new UnityObjectConverter()
            }
        };

        public static void Save(GameState gameState)
        {
            string serializedGameState = JsonConvert.SerializeObject(gameState, _settings);
            File.WriteAllText(_saveFilePath, serializedGameState);
        }

        public static GameState Load()
        {
            Debug.Log(_saveFilePath);
            GameState gameStateFromSaveFile = JsonConvert.DeserializeObject<GameState>(File.ReadAllText(_saveFilePath), _settings);
            return gameStateFromSaveFile;
        }

        public static void Delete()
        {
            File.Delete(_saveFilePath);
        }
        private class UnityObjectConverter : JsonConverter<UnityEngine.Object>
        {
            public override UnityEngine.Object ReadJson(JsonReader reader, Type objectType, UnityEngine.Object existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                string primaryKey = (string)reader.Value;

                return Database.GetAssetForPrimaryKey<UnityEngine.Object>(primaryKey);
            }

            public override void WriteJson(JsonWriter writer, UnityEngine.Object asset, JsonSerializer serializer)
            {
                writer.WriteValue(Database.GetPrimaryKeyForAsset(asset));
            }
        }
    }
}
