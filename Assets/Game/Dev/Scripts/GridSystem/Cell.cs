using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Items;
using UnityEngine;

namespace Sdurlanik.Merge2.GridSystem
{
    public class Cell : MonoBehaviour
    {
        public Item OccupiedItem { get; private set; }
        public bool IsEmpty => OccupiedItem == null;

        public void PlaceItem(Item item)
        {
            OccupiedItem = item;
            item.transform.SetParent(transform);
            item.transform.localPosition = Vector3.zero;
            item.SetCurrentCell(this);
        }

        public void ClearItem()
        {
            OccupiedItem = null;
        }

        public void DestroyItem()
        {
            if (OccupiedItem == null) return;
            
            var poolTag = OccupiedItem.ItemDataSO.ItemPrefab.name;
            ObjectPooler.Instance.ReturnObjectToPool(poolTag, OccupiedItem.gameObject);
                
            OccupiedItem = null;
        }
    }
}