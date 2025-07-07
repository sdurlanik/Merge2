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

            var manualPlacementMap =
                levelDesign.ManualItemPlacements.ToDictionary(p => p.GridPosition, p => p.ItemToPlace);
            var unlockedPositions = new HashSet<Vector2Int>(levelDesign.InitiallyUnlockedCells);

            foreach (var cell in allCells)
            {
                cell.Init(cell.GridPos);
                ItemSO itemToPlace = null;

                if (manualPlacementMap.TryGetValue(cell.GridPos, out var manualItem))
                {
                    itemToPlace = manualItem;
                }

                else if (!unlockedPositions.Contains(cell.GridPos))
                {
                    if (levelDesign.ProceduralItemPool != null && levelDesign.ProceduralItemPool.Count > 0)
                    {
                        itemToPlace =
                            levelDesign.ProceduralItemPool[Random.Range(0, levelDesign.ProceduralItemPool.Count)];
                    }
                }

                if (itemToPlace != null)
                {
                    ItemFactoryService.Create(itemToPlace, cell);
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

            EventBus<BoardStateChangedEvent>.Publish(new BoardStateChangedEvent());
        }
    }
}