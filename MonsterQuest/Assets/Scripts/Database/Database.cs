using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;

namespace MonsterQuest
{
    public class Database
    {
        private static readonly List<MonsterType> _monsterTypes = new();
        private static readonly List<ItemType> _itemTypes = new();

        public static IEnumerable<MonsterType> monsterTypes => _monsterTypes;
        public static IEnumerable<ItemType> itemTypes => _itemTypes;

        public static IEnumerator Initialize()
        {
            yield return Addressables.InitializeAsync();

            // Load all assets.
            yield return LoadAssets(_monsterTypes);
            yield return LoadAssets(_itemTypes);
        }

        public static MonsterType GetMonsterType(string displayName)
        {
            return _monsterTypes.First(monster => monster.displayName == displayName);
        }

        public static T GetItemType<T>(string displayName) where T : class
        {
            return _itemTypes.First(item => item.displayName == displayName && item is T) as T;
        }

        private static IEnumerator LoadAssets<TObject>(List<TObject> list)
        {
            yield return Addressables.LoadAssetsAsync<TObject>("database", list.Add);
        }
    }
}
