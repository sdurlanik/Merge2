using System.Collections.Generic;
using Sdurlanik.Merge2.Services;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sdurlanik.Merge2.GridSystem
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance; // Singleton
    
        public Cell CellPrefab;
        public int Size = 5;
        public float CellSpace = 3f;

        private readonly List<Cell> _cells = new List<Cell>();

        private void Awake() { Instance = this; }

        private void Start()
        {
            CreateGrid();
            BoardSetupService.SetupInitialBoard();
        }

        //TODO: I will call this method from gamemanager later
        public void CreateGrid()
        {
            for (int y = 0; y < Size; y++)
            for (int x = 0; x < Size; x++)
            {
                var pos = new Vector2(x * CellSpace, y * CellSpace);
                var cell = Instantiate(CellPrefab, pos, Quaternion.identity, transform);
                cell.gameObject.name = $"Cell_{x}_{y}";
                _cells.Add(cell);
            }
        }
    
        public bool TryGetEmptyCell(out Cell emptyCell)
        {
            emptyCell = _cells.Find(c => c.IsEmpty);
            return emptyCell != null;
        }
    }
}
