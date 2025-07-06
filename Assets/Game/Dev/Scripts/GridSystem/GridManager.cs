using System.Collections.Generic;
using System.Linq;
using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Data;
using Sdurlanik.Merge2.Data.Orders;
using Sdurlanik.Merge2.GridSystem.CellStates;
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
            emptyCell = _cells.Find(c => c.CurrentState == Cell.Unlocked && c.IsEmpty);
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
            for (var i = 0; i < _cells.Count; i++)
            {
                var cell = _cells[i];
                if (!cell.IsEmpty && cell.CurrentState == Cell.Unlocked)
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
                    !c.IsEmpty && 
                    c.CurrentState == Cell.Unlocked &&
                    c.OccupiedItem.ItemDataSO == requirement.RequiredItem);

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
        
        public (List<ItemSaveData> items, List<CellSaveData> cells) GetItemsForSaving()
        {
            var itemsToSave = new List<ItemSaveData>();
            var cellsToSave = new List<CellSaveData>();
            foreach (var cell in _cells)
            {
                cellsToSave.Add(new CellSaveData
                {
                    GridPosition = cell.GridPos,
                    State = cell.CurrentState.StateType,
                });

                if (!cell.IsEmpty)
                {
                    itemsToSave.Add(new ItemSaveData
                    {
                        ItemSOName = cell.OccupiedItem.ItemDataSO.name,
                        CellGridPosition = cell.GridPos
                    });
                }
            }
            return (itemsToSave, cellsToSave);
        }

        public void LoadItemsFromSave(List<ItemSaveData> savedItems, List<CellSaveData> savedCellStates)
        {
            foreach (var cellData in savedCellStates)
            {
                var cell = GetCellAt(cellData.GridPosition);
                if (cell != null)
                {
                    switch (cellData.State)
                    {
                        case CellState.LockedHidden:
                            cell.TransitionTo(Cell.LockedHidden);
                            break;
                        case CellState.LockedRevealed:
                            cell.TransitionTo(Cell.LockedRevealed);
                            break;
                        case CellState.Unlocked:
                            cell.TransitionTo(Cell.Unlocked);
                            break;
                    }
                }
            }
            
            foreach (var cell in _cells)
            {
                if (!cell.IsEmpty) cell.DestroyItem();
            }
            
            foreach (var itemData in savedItems)
            {
                var cell = GetCellAt(itemData.CellGridPosition);
                var itemSO = ServiceLocator.Get<DataManager>().GetSOByName(itemData.ItemSOName);
                if (cell != null && itemSO != null && cell.IsEmpty)
                {
                    //TODO: first load maybe should create item without animation?
                    ItemFactory.Create(itemSO, cell);
                }
            }
        }
        
        public Cell GetCellAt(Vector2Int gridPos)
        {
            return _cells.FirstOrDefault(c => c.GridPos == gridPos);
        }

        public List<Cell> GetAvailableCells()
        {
            return _cells.Where(c => c.CurrentState == Cell.Unlocked && c.IsEmpty).ToList();
        }

        private List<Cell> GetNeighbors(Cell cell) //TODO: GridUtils
        {
            var neighbors = new List<Cell>();
            var pos = cell.GridPos;

            int[] dx = { 0, 0, 1, -1 };
            int[] dy = { 1, -1, 0, 0 };

            for (int i = 0; i < 4; i++)
            {
                var neighborPos = new Vector2Int(pos.x + dx[i], pos.y + dy[i]);

                if (neighborPos.x >= 0 && neighborPos.x < _gridSettings.Size && neighborPos.y >= 0 && neighborPos.y < _gridSettings.Size)
                {
                    neighbors.Add(GetCellAt(neighborPos));
                }
            }

            return neighbors;
        }

        public void RevealNeighborsOf(Cell cell)
        {
            var neighbors = GetNeighbors(cell);
            foreach(var neighbor in neighbors)
            {
                if (neighbor.CurrentState == Cell.LockedHidden)
                {
                    neighbor.TransitionTo(neighbor.IsEmpty ? Cell.Unlocked : Cell.LockedRevealed);
                }
            }
        }
    }
}
