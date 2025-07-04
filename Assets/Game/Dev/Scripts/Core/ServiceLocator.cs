using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sdurlanik.Merge2.Core
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _Services = new Dictionary<Type, object>();
        
        public static void Register<T>(T service)
        {
            var type = typeof(T);
            if (!_Services.TryAdd(type, service))
            {
                Debug.LogWarning($"Service of type {type.Name} is already registered.");
                return;
            }

            Debug.Log($"Service registered: {type.Name}");
        }

        public static void Unregister<T>()
        {
            var type = typeof(T);
            if (!_Services.ContainsKey(type))
            {
                Debug.LogWarning($"Trying to unregister a non-existent service: {type.Name}");
                return;
            }
            _Services.Remove(type);
        }
        
        public static T Get<T>()
        {
            var type = typeof(T);
            if (!_Services.TryGetValue(type, out var service))
            {
                throw new InvalidOperationException($"Service of type {type.Name} not registered.");
            }
            return (T)service;
        }
    }
}