using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using Sdurlanik.Merge2.Data;

namespace Sdurlanik.Merge2.Editor
{
    public class LevelDesignerWindow : EditorWindow
    {
        private LevelDesignSettingsSO _selectedLevelDesign;
        private Vector2 _gridScrollPosition;
        private Vector2 _paletteScrollPosition;

        private List<ItemSO> _allItemSOs = new List<ItemSO>();
        private List<ItemSO> _filteredItemSOs = new List<ItemSO>();
        private string _searchQuery = "";
        
        private readonly float _buttonSize = 100;
        private readonly float _paletteWidth = 250f;
        
        [MenuItem("Merge2/Level Designer Tool")]
        public static void ShowWindow()
        {
            GetWindow<LevelDesignerWindow>("Level Designer");
        }

        private void OnEnable()
        {
            LoadAllItemSOs();
            FilterItems();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Level Design Editor", EditorStyles.boldLabel);
            
            var selection = Selection.activeObject;
            if (selection is LevelDesignSettingsSO selectedSO)
            {
                _selectedLevelDesign = selectedSO;
            }

            if (_selectedLevelDesign == null)
            {
                EditorGUILayout.HelpBox("Please select a LevelDesignSettingsSO asset to edit.", MessageType.Warning);
                return;
            }
            
            EditorGUILayout.LabelField("Editing:", _selectedLevelDesign.name, EditorStyles.miniBoldLabel);
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();

            DrawItemPalette();
            DrawGridArea();

            EditorGUILayout.EndHorizontal();
        }

        private void OnSelectionChange()
        {
            Repaint();
        }
        
        private void LoadAllItemSOs()
        {
            _allItemSOs.Clear();
            var guids = AssetDatabase.FindAssets("t:ItemSO");
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var so = AssetDatabase.LoadAssetAtPath<ItemSO>(path);
                _allItemSOs.Add(so);
            }
            
            _allItemSOs = _allItemSOs.OrderBy(so => so.name).ToList();
        }

        private void FilterItems()
        {
            if (string.IsNullOrEmpty(_searchQuery))
            {
                _filteredItemSOs = new List<ItemSO>(_allItemSOs);
            }
            else
            {
                var query = _searchQuery.ToLower();
                _filteredItemSOs = _allItemSOs.Where(so => so.name.ToLower().Contains(query)).ToList();
            }
        }
        
         private void DrawItemPalette()
        {
            EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(_paletteWidth), GUILayout.ExpandHeight(true));
            
            EditorGUILayout.LabelField("Item Palette", EditorStyles.boldLabel);
            string newQuery = EditorGUILayout.TextField("Search", _searchQuery);
            if (newQuery != _searchQuery)
            {
                _searchQuery = newQuery;
                FilterItems();
            }
            
            EditorGUILayout.Space();
            
            _paletteScrollPosition = EditorGUILayout.BeginScrollView(_paletteScrollPosition);
            
            foreach (var itemSO in _filteredItemSOs)
            {
                DrawPaletteItem(itemSO);
            }
            
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void DrawPaletteItem(ItemSO itemSO)
        {
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            
            GUILayout.Label(itemSO.Icon != null ? itemSO.Icon.texture : Texture2D.grayTexture, 
                GUILayout.Width(40), GUILayout.Height(40));
            EditorGUILayout.LabelField(itemSO.name, GUILayout.ExpandWidth(true));
            
            EditorGUILayout.EndHorizontal();

            var itemRect = GUILayoutUtility.GetLastRect();
            var e = Event.current;

            if (e.type == EventType.MouseDown && itemRect.Contains(e.mousePosition))
            {
                DragAndDrop.PrepareStartDrag();
                DragAndDrop.objectReferences = new UnityEngine.Object[] { itemSO };
                DragAndDrop.StartDrag($"Dragging {itemSO.name}");
                e.Use();
            }
        }
        
        private void DrawGridArea()
        {
            _gridScrollPosition = EditorGUILayout.BeginScrollView(_gridScrollPosition);
            DrawGrid();
            EditorGUILayout.EndScrollView();
        }
        
       private void DrawGrid()
        {
            EditorGUILayout.LabelField("Grid Layout", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Left Click: Toggle Lock | Right Click: Clear Item | Drag & Drop: Assign Item", MessageType.Info);
            
            var gridSize = 5;
            var originalGuiColor = GUI.backgroundColor;
            
            for (int y = gridSize - 1; y >= 0; y--)
            {
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < gridSize; x++)
                {
                    var pos = new Vector2Int(x, y);
                    var placement = _selectedLevelDesign.ManualItemPlacements.FirstOrDefault(p => p.GridPosition == pos);
                    var isUnlocked = _selectedLevelDesign.InitiallyUnlockedCells.Contains(pos);
                    var hasItem = placement?.ItemToPlace != null;

                    GUI.backgroundColor = (isUnlocked, hasItem) switch
                    {
                        (false, _)    => new Color(0.4f, 0.4f, 0.4f, 1f), 

                        (true, true)  => new Color(0.5f, 0.8f, 1f, 1f),

                        (true, false) => new Color(0.6f, 1f, 0.6f, 1f)
                    };
    
                    var buttonContent = new GUIContent(hasItem ? "" : (isUnlocked ? "Empty" : "Locked"), 
                        hasItem ? placement.ItemToPlace.Icon.texture : null);

                    var buttonStyle = new GUIStyle(GUI.skin.button);
                    buttonStyle.fontStyle = isUnlocked ? FontStyle.Bold : FontStyle.Normal;

                    GUILayout.Box(buttonContent, buttonStyle, GUILayout.Width(_buttonSize), GUILayout.Height(_buttonSize));
                    
                    var buttonRect = GUILayoutUtility.GetLastRect();
                    HandleMouseEvents(pos, buttonRect, isUnlocked, placement);
                    HandleDragAndDrop(pos, buttonRect);
                }
                EditorGUILayout.EndHorizontal();
            }
            
            GUI.backgroundColor = originalGuiColor;
        }

        private void HandleMouseEvents(Vector2Int gridPos, Rect buttonRect, bool isUnlocked, InitialItemPlacement placement)
        {
            var currentEvent = Event.current;

            if (buttonRect.Contains(currentEvent.mousePosition) && currentEvent.type == EventType.MouseDown)
            {
                if (currentEvent.button == 0)
                {
                    Undo.RecordObject(_selectedLevelDesign, "Toggle Unlocked Cell");
                    if (isUnlocked)
                        _selectedLevelDesign.InitiallyUnlockedCells.Remove(gridPos);
                    else
                        _selectedLevelDesign.InitiallyUnlockedCells.Add(gridPos);
                    
                    EditorUtility.SetDirty(_selectedLevelDesign);
                    currentEvent.Use(); 
                }
                else if (currentEvent.button == 1)
                {
                    if (placement != null)
                    {
                        Undo.RecordObject(_selectedLevelDesign, "Clear Item from Cell");
                        _selectedLevelDesign.ManualItemPlacements.Remove(placement);
                        EditorUtility.SetDirty(_selectedLevelDesign);
                    }
                    currentEvent.Use();
                }
            }
        }
        
        private void HandleDragAndDrop(Vector2Int gridPos, Rect dropArea)
        {
            Event currentEvent = Event.current;
            
            if (!dropArea.Contains(currentEvent.mousePosition)) return;

            switch (currentEvent.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (DragAndDrop.objectReferences.Any(obj => obj is ItemSO))
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    }

                    if (currentEvent.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();
                        var droppedItemSO = DragAndDrop.objectReferences.First(obj => obj is ItemSO) as ItemSO;
                        
                        Undo.RecordObject(_selectedLevelDesign, "Assign Item to Cell");

                        var placement = _selectedLevelDesign.ManualItemPlacements.FirstOrDefault(p => p.GridPosition == gridPos);
                        if (placement != null)
                        {
                            placement.ItemToPlace = droppedItemSO;
                        }
                        else
                        {
                            _selectedLevelDesign.ManualItemPlacements.Add(new InitialItemPlacement
                            {
                                GridPosition = gridPos,
                                ItemToPlace = droppedItemSO
                            });
                        }
                        EditorUtility.SetDirty(_selectedLevelDesign);
                    }
                    currentEvent.Use();
                    break;
            }
        }
    }
}