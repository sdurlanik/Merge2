using System.Collections.Generic;
using Sdurlanik.Merge2.GridSystem.CellStates;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sdurlanik.Merge2.Data
{
    [System.Serializable]
    public struct ItemSaveData
    {
        public string ItemSOName;
        public Vector2Int CellGridPosition;
    }

    [System.Serializable]
    public struct CellSaveData
    {
        public Vector2Int GridPosition;
        public CellState State; 
    }
    
    [System.Serializable]
    public class PlayerData
    {
        public int Coins;
        public List<ItemSaveData> GridItems;
        public List<string> ActiveOrderSONames; 
        public List<CellSaveData> CellStates;

        public PlayerData()
        {
            Coins = 0;
            GridItems = new List<ItemSaveData>();
            ActiveOrderSONames = new List<string>();
            CellStates = new List<CellSaveData>();
        }
    }
}