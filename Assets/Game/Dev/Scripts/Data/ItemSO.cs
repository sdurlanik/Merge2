using System.Collections.Generic;
using Sdurlanik.Merge2.Items;
using UnityEngine;

namespace Sdurlanik.Merge2.Data
{
    [CreateAssetMenu(menuName = "Merge2/ItemSO")]
    public class ItemSO : ScriptableObject
    {
        [Header("Item Settings")]
        [SerializeField] private string _itemName;
        [SerializeField] private Item _itemPrefab;
        [SerializeField] private ItemFamily _family;
        [SerializeField] private int _level;
        [SerializeField] private Sprite _icon;
        
        [Header("Producer Settings")]
        [SerializeField] private bool _canProduce;
        [SerializeField] private List<ProductionChance> _productionChances;

        public string ItemName => _itemName;
        public Item ItemPrefab => _itemPrefab;
        public ItemFamily Family => _family;
        public int Level => _level;
        public Sprite Icon => _icon;
        public bool CanProduce => _canProduce;
        public IReadOnlyList<ProductionChance> ProductionChances => _productionChances;
    }
    public enum ItemFamily { G1, P1, P2 }
}

