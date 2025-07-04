using System.Collections.Generic;
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
    public class PlayerData
    {
        public int Coins;
        public List<ItemSaveData> GridItems;
        public List<string> ActiveOrderSONames; 

        public PlayerData()
        {
            Coins = 0;
            GridItems = new List<ItemSaveData>();
            ActiveOrderSONames = new List<string>();
        }
    }
}