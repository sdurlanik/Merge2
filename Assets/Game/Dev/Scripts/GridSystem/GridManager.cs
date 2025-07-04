using System.Collections.Generic;
using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Data;
using Sdurlanik.Merge2.Services;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sdurlanik.Merge2.GridSystem
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance; // Singleton
        [SerializeField] private GridSettingsSO _gridSettings;

        private readonly List<Cell> _cells = new List<Cell>();

        private void Awake()
        {
            Instance = this;
        }

        public void CreateGrid()
        {
            for (int y = 0; y < _gridSettings.Size; y++)
            for (int x = 0; x < _gridSettings.Size; x++)
            {
                var pos = new Vector2(x * _gridSettings.CellSpace, y * _gridSettings.CellSpace);
                var cell = Instantiate(_gridSettings.CellPrefab, pos, Quaternion.identity, transform);
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
