using System.Collections.Generic;
using Sdurlanik.Merge2.Data;
using UnityEngine;

namespace Sdurlanik.Merge2.Core
{
    public class ObjectPooler : MonoBehaviour
    {
        private readonly Dictionary<string, Queue<GameObject>> _poolDictionary = new();
        private readonly Dictionary<string, GameObject> _prefabCache = new();
        
        private Transform _pooledObjectsParent;

        private void Start()
        {
            _pooledObjectsParent = transform;
        }

        /// <summary>
        /// Create a new object pool for a given prefab.
        /// </summary>
        /// <param name="prefab">Prefab to pool.</param>
        /// <param name="initialSize">Initial number of objects to create in the pool.</param>
        public void CreatePool(PoolSettingsSO poolSettings)
        {
            var poolTag = poolSettings.DefaultItemPrefab.name;
            if (_poolDictionary.ContainsKey(poolTag))
            {
                Debug.LogWarning($"Pool with tag '{poolTag}' already exists.");
                return;
            }

            var objectPool = new Queue<GameObject>();
            for (var i = 0; i <  poolSettings.InitialPoolSize; i++)
            {
                var obj = Instantiate( poolSettings.DefaultItemPrefab, _pooledObjectsParent);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            _poolDictionary.Add(poolTag, objectPool);
            _prefabCache.Add(tag,  poolSettings.DefaultItemPrefab);
        }

        /// <summary>
        /// Gets an object from the pool by its tag.
        /// </summary>
        /// <param name="poolTag">Tag of the prefab to retrieve.</param>
        public GameObject GetObjectFromPool(string poolTag)
        {
            if (!_poolDictionary.ContainsKey(poolTag))
            {
                Debug.LogError($"Pool with tag '{poolTag}' doesn't exist.");
                return null;
            }

            if (_poolDictionary[poolTag].Count > 0)
            {
                var objectToSpawn = _poolDictionary[poolTag].Dequeue();
                return objectToSpawn;
            }
    
            Debug.LogWarning($"Pool with tag '{poolTag}' is empty. Creating a new instance from cache.");
    
            if (_prefabCache.TryGetValue(poolTag, out var prefabToInstantiate))
            {
                var newObj = Instantiate(prefabToInstantiate, _pooledObjectsParent);
                return newObj;
            }
    
            Debug.LogError($"Prefab for tag '{poolTag}' not found in cache. Cannot create new instance.");
            return null;
        }

        /// <summary>
        /// Returns an object to the pool.
        /// </summary>
        /// <param name="poolTag"> Tag of the pool to return the object to.</param>
        /// <param name="objectToReturn"> Object to return to the pool.</param>
        public void ReturnObjectToPool(string poolTag, GameObject objectToReturn)
        {
            if (!_poolDictionary.ContainsKey(poolTag))
            {
                Debug.LogWarning($"Trying to return object to a non-existent pool with tag '{poolTag}'. Destroying object instead.");
                Destroy(objectToReturn);
                return;
            }
            
            objectToReturn.SetActive(false);
            objectToReturn.transform.SetParent(_pooledObjectsParent);
            _poolDictionary[poolTag].Enqueue(objectToReturn);
        }
    }
}