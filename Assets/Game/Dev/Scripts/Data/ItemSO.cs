using System.Collections.Generic;
using Sdurlanik.Merge2.Items;
using UnityEngine;

namespace Sdurlanik.Merge2.Data
{
    public enum ItemFamily { G1, P1, P2 }
    public class ItemSO : ScriptableObject
    {
        [Header("Item Settings")]
        [SerializeField] private string _itemName;
        [SerializeField] private Item _itemPrefab; // We might want to use different prefabs for different items, 
                                                   // so it's better to keep this as a reference to the prefab.
        [SerializeField] private ItemFamily _family;
        [SerializeField] private int _level;
        [SerializeField] private Sprite _icon;

        // Public property'ler
        public string ItemName => _itemName;
        public Item ItemPrefab => _itemPrefab;
        public ItemFamily Family => _family;
        public int Level => _level;
        public Sprite Icon => _icon;
    }
}

