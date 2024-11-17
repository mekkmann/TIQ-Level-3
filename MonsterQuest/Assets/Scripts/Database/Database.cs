using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace MonsterQuest
{
    public static class Database
    {
        private static readonly List<MonsterType> _monsterTypes = new();
        private static readonly List<ItemType> _itemTypes = new();
        private static readonly List<ClassType> _classTypes = new();
        private static readonly List<RaceType> _raceTypes = new();

        private static readonly List<Sprite> _sprites = new();
        private static readonly List<Object> _allObjects = new();

        private static readonly Dictionary<Object, string> _primaryKeysByAssets = new();
        private static readonly Dictionary<string, Object> _assetsByPrimaryKey = new();

        public static IEnumerable<MonsterType> MonsterTypes => _monsterTypes;
        public static IEnumerable<ItemType> ItemTypes => _itemTypes;
        public static IEnumerable<ClassType> ClassTypes => _classTypes;
        public static IEnumerable<RaceType> RaceTypes => _raceTypes;

        public static IEnumerator Initialize()
        {
            yield return Addressables.InitializeAsync();

            // Load all assets.
            yield return LoadAssets(_monsterTypes);
            yield return LoadAssets(_itemTypes);
            yield return LoadAssets(_classTypes);
            yield return LoadAssets(_raceTypes);

            // We also load all Unity objects so they get their instanceIDs indexed. We need to load the
            // sprites first so they get registered before their textures (which have the same primary key).
            yield return LoadAssets(_sprites);
            yield return LoadAssets(_allObjects);
        }

        public static MonsterType GetMonsterType(string displayName)
        {
            return _monsterTypes.First(monster => monster.DisplayName == displayName);
        }

        public static T GetItemType<T>(string displayName) where T : class
        {
            return _itemTypes.First(item => item.DisplayName == displayName && item is T) as T;
        }

        public static ClassType GetClassType(string displayName)
        {
            return _classTypes.First(characterClass => characterClass.DisplayName == displayName);
        }
        public static RaceType GetRaceType(string displayName)
        {
            return _raceTypes.First(race => race.DisplayName == displayName);
        }

        public static string GetPrimaryKeyForAsset(Object asset)
        {
            if (!_primaryKeysByAssets.ContainsKey(asset))
            {
                Debug.LogError($"Referenced Unity Object ({asset.name} of type {asset.GetType().Name}) is not part of the database.");

                return null;
            }

            return _primaryKeysByAssets[asset];
        }

        public static T GetAssetForPrimaryKey<T>(string primaryKey) where T : Object
        {
            if (primaryKey == null) return null;

            if (!_assetsByPrimaryKey.ContainsKey(primaryKey))
            {
                Debug.LogError($"Referenced addressable ({primaryKey}) is not part of the database.");

                return null;
            }

            return _assetsByPrimaryKey[primaryKey] as T;
        }

        private static IEnumerator LoadAssets<TObject>(List<TObject> list) where TObject : Object
        {
            AsyncOperationHandle<IList<IResourceLocation>> loadResourceLocationsHandle = Addressables.LoadResourceLocationsAsync("database", typeof(TObject));

            if (!loadResourceLocationsHandle.IsDone) yield return loadResourceLocationsHandle;

            // Load referenced resources.
            List<AsyncOperationHandle> loadAssetHandles = new();

            foreach (IResourceLocation location in loadResourceLocationsHandle.Result)
            {
                AsyncOperationHandle<TObject> loadAssetHandle = Addressables.LoadAssetAsync<TObject>(location);

                loadAssetHandle.Completed += loadedAssetHandle =>
                {
                    TObject asset = loadedAssetHandle.Result;
                    list.Add(asset);

                    int instanceId = asset.GetInstanceID();

                    if (_primaryKeysByAssets.ContainsKey(asset))
                    {
                        if (_primaryKeysByAssets[asset] != location.PrimaryKey)
                        {
                            Debug.LogError($"Multiple assets with the same instance ID. {location.PrimaryKey} - {instanceId}");
                        }
                    }
                    else
                    {
                        _primaryKeysByAssets[asset] = location.PrimaryKey;
                    }

                    if (_assetsByPrimaryKey.ContainsKey(location.PrimaryKey))
                    {
                        if (_assetsByPrimaryKey[location.PrimaryKey] != asset)
                        {
                            // If we're loading a texture asset, the collision is probably with the associated sprite, so we don't raise an error.
                            if (asset is not Texture)
                            {
                                Debug.LogError($"Multiple assets with the same primary key. {location.PrimaryKey} - {instanceId}");
                            }
                        }
                    }
                    else
                    {
                        _assetsByPrimaryKey[location.PrimaryKey] = asset;
                    }
                };

                loadAssetHandles.Add(loadAssetHandle);
            }

            //create a GroupOperation to wait on all the above loads at once. 
            AsyncOperationHandle<IList<AsyncOperationHandle>> loadAssetsOperationHandle = Addressables.ResourceManager.CreateGenericGroupOperation(loadAssetHandles);

            if (!loadAssetsOperationHandle.IsDone) yield return loadAssetsOperationHandle;

            Addressables.Release(loadResourceLocationsHandle);
        }
    }
}
