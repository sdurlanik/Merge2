using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Data;
using Sdurlanik.Merge2.GridSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sdurlanik.Merge2.Services
{
    public static class BoardSetupService
    {
        public static void SetupInitialBoard(LevelDesignSettingsSO levelDesignSettings)
        {
            if (levelDesignSettings == null)
            {
                Debug.LogError("LevelDesignSettingsSO is null. Cannot setup initial board.");
                return;
            }

            var gridManager = ServiceLocator.Get<GridManager>();
            var allCells = gridManager.GetAllCells().ToList();

            foreach (var placement in levelDesignSettings.InitialItems)
            {
                var cell = gridManager.GetCellAt(placement.GridPosition);
                if (cell != null && cell.IsEmpty)
                {
                    ItemFactory.Create(placement.ItemToPlace, cell);
                }
            }

            var unlockedPositions = new HashSet<Vector2Int>(levelDesignSettings.InitiallyUnlockedCells);

            foreach (var cell in allCells)
            {
                if (unlockedPositions.Contains(cell.GridPos))
                {
                    cell.TransitionTo(Cell.Unlocked);
                }
                else
                {
                    cell.TransitionTo(Cell.LockedHidden);
                }
            }
            
            FillCustomCells();
        }

        private static void FillCustomCells()
        {
            var dataBank = ServiceLocator.Get<DataBank>();
            var itemsToSpawn = new List<ItemSO>
            {
                dataBank.GetSO(ItemFamily.G1, 1),
                dataBank.GetSO(ItemFamily.G1, 1),
                dataBank.GetSO(ItemFamily.G1, 2),
                dataBank.GetSO(ItemFamily.G1, 3),
                dataBank.GetSO(ItemFamily.G1, 4)
            };
           
            foreach (var so in itemsToSpawn)
            {
                if (ServiceLocator.Get<GridManager>().TryGetEmptyCell(out var cell))
                {
                    ItemFactory.Create(so, cell);
                }
            }
        }
    }
}