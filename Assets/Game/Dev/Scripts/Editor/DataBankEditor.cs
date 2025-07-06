using UnityEngine;
using UnityEditor;
using System.Linq;
using Sdurlanik.Merge2.Data;
using Sdurlanik.Merge2.Editor.Helpers;

namespace Sdurlanik.Merge2.Editor
{

    [CustomEditor(typeof(DataManager))]
    public class DataBankEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawCustomEditorSection();
        }

        private void DrawCustomEditorSection()
        {
            EditorGUILayout.Space(15);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.LabelField("Auto-Fill Editor", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            var dataBank = (DataManager)target;
            if (GUILayout.Button("Find & Add All ItemSOs In Project", GUILayout.Height(40)))
            {
                PopulateAllItems(dataBank);
            }
        }

        private void PopulateAllItems(DataManager dataManager)
        {
            Undo.RecordObject(dataManager, "Populate ItemSO List");

            dataManager.AllItems = EditorAssetHelper.FindAllAssetsOfType<ItemSO>();
        
            dataManager.AllItems = dataManager.AllItems.OrderBy(item => item.Family).ThenBy(item => item.Level).ToList();

            EditorUtility.SetDirty(dataManager);
            Debug.Log($"DataBank populated with {dataManager.AllItems.Count} ItemSOs.");
        }
    }
}