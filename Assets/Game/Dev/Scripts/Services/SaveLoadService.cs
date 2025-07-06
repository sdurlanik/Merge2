using Sdurlanik.Merge2.Data;
using UnityEngine;
using System.IO;

namespace Sdurlanik.Merge2.Services
{
    public static class SaveLoadService
    {
        private static readonly string SaveFileName = "playerdata.json";

        public static string GetSavePath()
        {
            return Path.Combine(Application.persistentDataPath, SaveFileName);
        }

        public static void SaveGame(PlayerData data)
        {
            var path = GetSavePath();
            try
            {
                var json = JsonUtility.ToJson(data, true);
                File.WriteAllText(path, json);
                Debug.Log($"Game saved successfully: {path}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Save failed: {e.Message}");
            }
        }

        public static PlayerData LoadGame()
        {
            var path = GetSavePath();
            if (File.Exists(path))
            {
                try
                {
                    var json = File.ReadAllText(path);
                    var data = JsonUtility.FromJson<PlayerData>(json);
                    Debug.Log($"Game loaded successfully: {path}");
                    return data;
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Load failed: {e.Message}");
                    return new PlayerData();
                }
            }
            else
            {
                Debug.Log("Save file not found, returning new PlayerData.");
                return new PlayerData();
            }
        }
    }
}