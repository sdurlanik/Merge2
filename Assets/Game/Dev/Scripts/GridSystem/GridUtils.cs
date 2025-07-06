using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sdurlanik.Merge2.GridSystem
{
    public static class GridUtils
    {
        public static Cell GetCellAt(Vector2Int gridPos, IReadOnlyList<Cell> allCells)
        {
            return allCells.FirstOrDefault(c => c.GridPos == gridPos);
        }
        
        public static List<Vector2Int> GetNeighborPositions(Vector2Int originPos, int gridSize)
        {
            var neighborPositions = new List<Vector2Int>();
            Vector2Int[] directions = 
            {
                new Vector2Int(0, 1),  // Up
                new Vector2Int(0, -1), // Down
                new Vector2Int(1, 0),  // Right
                new Vector2Int(-1, 0)  // Left
            };

            foreach (var dir in directions)
            {
                var neighborPos = originPos + dir;
                
                if (neighborPos.x >= 0 && neighborPos.x < gridSize && 
                    neighborPos.y >= 0 && neighborPos.y < gridSize)
                {
                    neighborPositions.Add(neighborPos);
                }
            }
            return neighborPositions;
        }
    }
}