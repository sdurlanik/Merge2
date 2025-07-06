using System.Collections.Generic;
using UnityEngine;

namespace Sdurlanik.Merge2.Data
{
    [System.Serializable]
    public class InitialItemPlacement
    {
        public Vector2Int GridPosition;
        public ItemSO ItemToPlace;
    }

    [CreateAssetMenu(fileName = "Level01_Design", menuName = "Merge2/Settings/Level Design")]
    public class LevelDesignSettingsSO : ScriptableObject
    {
        public List<Vector2Int> InitiallyUnlockedCells;

        public List<InitialItemPlacement> InitialItems;
    }
}