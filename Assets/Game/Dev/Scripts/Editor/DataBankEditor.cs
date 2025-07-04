using UnityEngine;
using UnityEditor;
using System.Linq;
using Sdurlanik.Merge2.Data;
using Sdurlanik.Merge2.Editor.Helpers;

namespace Sdurlanik.Merge2.Editor
{

    [CustomEditor(typeof(DataBank))]
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

            var dataBank = (DataBank)target;
            if (GUILayout.Button("Find & Add All ItemSOs In Project", GUILayout.Height(40)))
            {
                PopulateAllItems(dataBank);
            }
        }

        private void PopulateAllItems(DataBank dataBank)
        {
            Undo.RecordObject(dataBank, "Populate ItemSO List");

            dataBank.AllItems = EditorAssetHelper.FindAllAssetsOfType<ItemSO>();
        
            dataBank.AllItems = dataBank.AllItems.OrderBy(item => item.Family).ThenBy(item => item.Level).ToList();

            EditorUtility.SetDirty(dataBank);
            Debug.Log($"DataBank populated with {dataBank.AllItems.Count} ItemSOs.");
        }
    }
}