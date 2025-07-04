using Sdurlanik.Merge2.GridSystem;
using UnityEngine;

namespace Sdurlanik.Merge2.Data
{
    [CreateAssetMenu(fileName = "GridSettings", menuName = "Merge2/Settings/GridSettings", order = 1)]
    public class GridSettingsSO : ScriptableObject
    {
        public Cell CellPrefab;
        public int Size = 5;
        public float CellSpace = 2.8f;
    }
}