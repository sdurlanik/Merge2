using UnityEngine;
using UnityEditor;
using System.IO; 
using Sdurlanik.Merge2.Services;

namespace Sdurlanik.Merge2.Editor
{
    public static class SaveDataEditorTool
    {
        [MenuItem("Merge2/Save Data/Clear Save File")]
        private static void ClearSaveFile()
        {
            var path = SaveLoadService.GetSavePath();

            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                    
                    EditorUtility.DisplayDialog("Success", 
                        "Save file successfully deleted from:\n" + path, "OK");
                    
                    Debug.Log("Save file deleted: " + path);
                }
                catch (System.Exception e)
                {
                    EditorUtility.DisplayDialog("Error", 
                        "Could not delete save file. Error: " + e.Message, "OK");
                        
                    Debug.LogError("Error deleting save file: " + e.Message);
                }
            }
            else
            {
                EditorUtility.DisplayDialog("File Not Found", 
                    "No save file found to delete at:\n" + path, "OK");
                    
                Debug.LogWarning("Attempted to delete save file, but it was not found.");
            }
        }
    }
}