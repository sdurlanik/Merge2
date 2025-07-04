using UnityEngine;

namespace Sdurlanik.Merge2.Core
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private bool _dontDestroyOnLoad = false;

        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning($"Duplicate Singleton of type {typeof(T)} found. Destroying this one.");
                Destroy(gameObject);
                return;
            }

            Instance = this as T;

            if (_dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}