using System.Collections.Generic;
using System.Linq;
using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Data;
using Sdurlanik.Merge2.Data.Orders;
using Sdurlanik.Merge2.Services;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sdurlanik.Merge2.GridSystem
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private GridSettingsSO _gridSettings;

        private readonly List<Cell> _cells = new List<Cell>();
        public IReadOnlyList<Cell> GetAllCells() => _cells;

        public void CreateGrid()
        {
            for (int y = 0; y < _gridSettings.Size; y++)
            for (int x = 0; x < _gridSettings.Size; x++)
            {
                var pos = new Vector2(x * _gridSettings.CellSpace, y * _gridSettings.CellSpace);
                var cell = Instantiate(_gridSettings.CellPrefab, pos, Quaternion.identity, transform);
                cell.gameObject.name = $"Cell_{x}_{y}";
                cell.Init(new Vector2Int(x, y));
                _cells.Add(cell);
            }
        }
    
        public bool TryGetEmptyCell(out Cell emptyCell)
        {
            emptyCell = _cells.Find(c => c.IsEmpty);
            return emptyCell != null;
        }
        
        public int GetHighestGeneratorLevelOnBoard()
        {
            int maxLevel = 0;
            foreach (var cell in _cells)
            {
                if (!cell.IsEmpty && cell.OccupiedItem.ItemDataSO is GeneratorSO generatorSO)
                {
                    if (generatorSO.Level > maxLevel)
                    {
                        maxLevel = generatorSO.Level;
                    }
                }
            }
            return maxLevel;
        }
        
        public Dictionary<ItemSO, int> GetCurrentItemCountsOnBoard()
        {
            var itemCounts = new Dictionary<ItemSO, int>();
            for (var index = 0; index < _cells.Count; index++)
            {
                var cell = _cells[index];
                if (!cell.IsEmpty)
                {
                    var itemSO = cell.OccupiedItem.ItemDataSO;
                    if (!itemCounts.TryAdd(itemSO, 1))
                    {
                        itemCounts[itemSO]++;
                    }
                }
            }

            return itemCounts;
        }
        
        public void ConsumeItems(List<OrderRequirement> requirements)
        {
            foreach (var requirement in requirements)
            {
                var cellToConsume = _cells.FirstOrDefault(c => 
                    !c.IsEmpty && c.OccupiedItem.ItemDataSO == requirement.RequiredItem);

                if (cellToConsume != null)
                {
                    cellToConsume.ConsumeItemWithAnimation();
                }
                else
                {
                    Debug.LogWarning($"Could not find required item: {requirement.RequiredItem.name} to consume for order.");
                    break; 
                }
            }
        }
        
        public List<ItemSaveData> GetItemsForSaving()
        {
            var itemsToSave = new List<ItemSaveData>();
            foreach (var cell in _cells)
            {
                if (!cell.IsEmpty)
                {
                    itemsToSave.Add(new ItemSaveData
                    {
                        ItemSOName = cell.OccupiedItem.ItemDataSO.name,
                        CellGridPosition = cell.GridPos
                    });
                }
            }
            return itemsToSave;
        }

        public void LoadItemsFromSave(List<ItemSaveData> savedItems)
        {
            foreach (var cell in _cells)
            {
                if (!cell.IsEmpty) cell.DestroyItem();
            }
            
            foreach (var itemData in savedItems)
            {
                var cell = GetCellAt(itemData.CellGridPosition);
                var itemSO = ServiceLocator.Get<DataBank>().GetSOByName(itemData.ItemSOName);
                if (cell != null && itemSO != null && cell.IsEmpty)
                {
                    ItemFactory.Create(itemSO, cell);
                }
            }
        }
        
        private Cell GetCellAt(Vector2Int gridPos)
        {
            return _cells.FirstOrDefault(c => c.GridPos == gridPos);
        }
    }
}
