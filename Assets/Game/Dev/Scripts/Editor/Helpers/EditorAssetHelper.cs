using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Sdurlanik.Merge2.Editor.Helpers
{

    public static class EditorAssetHelper
    {
        /// <summary>
        /// Finds all assets of a specific type in the project.
        /// </summary>
        public static List<T> FindAllAssetsOfType<T>() where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            return guids
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<T>)
                .Where(asset => asset != null)
                .ToList();
        }

        /// <summary>
        ///  Finds a specific asset by its name.
        /// </summary>
        /// <param name="name">The name of the asset to find.</param>
        public static T FindAssetByName<T>(string name) where T : Object
        {
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name} {name}");
            if (guids.Length == 0)
            {
                return null;
            }

            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }
    }
}