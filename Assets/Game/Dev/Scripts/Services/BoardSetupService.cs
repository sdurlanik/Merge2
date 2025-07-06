using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Data;
using Sdurlanik.Merge2.GridSystem;
using System.Collections.Generic;
using System.Linq;
using Sdurlanik.Merge2.Events;
using UnityEngine;

namespace Sdurlanik.Merge2.Services
{
    public static class BoardSetupService
    {
      public static void SetupInitialBoard(LevelDesignSettingsSO levelDesign)
        {
            if (levelDesign == null)
            {
                Debug.LogError("LevelDesignSettingsSO is null! Cannot setup initial board.");
                return;
            }

            var gridManager = ServiceLocator.Get<GridManager>();
            var allCells = gridManager.GetAllCells().ToList();
            
            var placementMap = levelDesign.InitialItems.ToDictionary(p => p.GridPosition, p => p.ItemToPlace);
            var unlockedPositions = new HashSet<Vector2Int>(levelDesign.InitiallyUnlockedCells);

            foreach (var cell in allCells)
            {
                cell.Init(cell.GridPos);

                if (placementMap.TryGetValue(cell.GridPos, out ItemSO itemToPlace))
                {
                    ItemFactory.Create(itemToPlace, cell);
                }

                if (unlockedPositions.Contains(cell.GridPos))
                {
                    cell.TransitionTo(Cell.Unlocked);
                }
                else
                {
                    cell.TransitionTo(Cell.LockedHidden);
                }
            }
            
            PlaceStartingItems();
            EventBus<BoardStateChangedEvent>.Publish(new BoardStateChangedEvent());
        }

        private static void PlaceStartingItems()
        {
            var gridManager = ServiceLocator.Get<GridManager>();
            var dataBank = ServiceLocator.Get<DataBank>();
            
            var itemsToSpawn = new List<ItemSO>
            {
                dataBank.GetSO(ItemFamily.G1, 1),
                dataBank.GetSO(ItemFamily.G1, 1),
                dataBank.GetSO(ItemFamily.G1, 2),
                dataBank.GetSO(ItemFamily.G1, 3),
                dataBank.GetSO(ItemFamily.G1, 4)
            };

            var availableCells = gridManager.GetAvailableCells().ToList();

            Debug.Log("Available cells count: " + availableCells.Count);
            foreach (var so in itemsToSpawn)
            {
                if (availableCells.Count > 0)
                {
                    var cellToPlaceIn = availableCells[0];
                    ItemFactory.Create(so, cellToPlaceIn);
                    availableCells.RemoveAt(0);
                }
                else
                {
                    Debug.LogWarning("Not enough available cells to place all starting items.");
                    break;
                }
            }
        }
    }
}