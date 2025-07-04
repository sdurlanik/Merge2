using UnityEngine;
using UnityEditor; 
using Sdurlanik.Merge2.Items;
using Sdurlanik.Merge2.Data;
using Sdurlanik.Merge2.Editor.Helpers;
using Sdurlanik.Merge2.Editor.Settings; // Ayar SO'muzu kullanmak için

namespace Sdurlanik.Merge2.Editor
{
    [CustomEditor(typeof(ItemSO), true)] 
    public class ItemSOEditor : UnityEditor.Editor
    {
        private const string FamilyPropertyName = "_family";
        private const string LevelPropertyName = "_level";
        private const string ItemNamePropertyName = "_itemName";
        private const string IconPropertyName = "_icon";
        private const string ItemPrefabPropertyName = "_itemPrefab";

        private SerializedProperty _familyProperty;
        private SerializedProperty _levelProperty;
        private ItemSOEditorSettings _editorSettings;

        private void OnEnable()
        {
            _familyProperty = serializedObject.FindProperty(FamilyPropertyName);
            _levelProperty = serializedObject.FindProperty(LevelPropertyName);
            
            _editorSettings = FindEditorSettings();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawPropertiesExcluding(serializedObject, FamilyPropertyName, LevelPropertyName);
            DrawCustomEditorSection();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawCustomEditorSection()
        {
            EditorGUILayout.Space(15);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.LabelField("Auto-Fill Editor", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            if (_editorSettings == null)
            {
                EditorGUILayout.HelpBox("ItemSOEditorSettings not found!", MessageType.Error);
                return;
            }

            EditorGUILayout.PropertyField(_familyProperty);
            EditorGUILayout.PropertyField(_levelProperty);
            EditorGUILayout.Space(10);

            GUIContent buttonContent = new GUIContent(
                "Generate Data & Rename Asset",
                "Fills data based on Family/Level and renames this asset file."
            );
            
            using (new EditorGUI.DisabledScope(_editorSettings.DefaultItemPrefab == null))
            {
                if (GUILayout.Button(buttonContent, GUILayout.Height(30)))
                {
                    AutoFillData((ItemSO)target);
                }
            }
        }

        private void AutoFillData(ItemSO itemSO)
        {
            var generatedName = $"{itemSO.Family}_{itemSO.Level}";
            
            var foundIcon = EditorAssetHelper.FindAssetByName<Sprite>(generatedName);
            if(foundIcon == null)
            {
                Debug.LogWarning($"Icon not found for: '{generatedName}'");
            }
            
            var foundPrefab = _editorSettings.DefaultItemPrefab;
            
            serializedObject.Update();
            serializedObject.FindProperty("_itemName").stringValue = generatedName;
            serializedObject.FindProperty("_icon").objectReferenceValue = foundIcon;
            serializedObject.FindProperty("_itemPrefab").objectReferenceValue = foundPrefab;
            serializedObject.ApplyModifiedProperties();

            string assetPath = AssetDatabase.GetAssetPath(itemSO);
            AssetDatabase.RenameAsset(assetPath, generatedName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"'{itemSO.name}' asset data was filled and file was renamed to '{generatedName}.asset'");
        }
        
        private ItemSOEditorSettings FindEditorSettings()
        {
            var settingsList = EditorAssetHelper.FindAllAssetsOfType<ItemSOEditorSettings>();
            return settingsList.Count > 0 ? settingsList[0] : null;
        }
    }
}