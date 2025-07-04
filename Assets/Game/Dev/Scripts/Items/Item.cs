using Sdurlanik.Merge2.GridSystem;
using Sdurlanik.Merge2.Items.Behaviours;
using Sdurlanik.Merge2.Data;
using UnityEngine;

namespace Sdurlanik.Merge2.Items
{

    [RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
    public class Item : MonoBehaviour, IDraggable, ITappable
    {
        public InteractionSettingsSO InteractionSettings => _interactionSettings;
        public ItemSO ItemDataSO { get; private set; }
        public Cell CurrentCell { get; private set; }
        
        [SerializeField] private InteractionSettingsSO _interactionSettings;
        private ProducerBehavior _producerBehavior;
        private DraggableBehavior _draggableBehavior;

        public void Init(ItemSO so)
        {
            ItemDataSO = so;
            GetComponent<SpriteRenderer>().sprite = so.Icon;

            if (ItemDataSO is GeneratorSO { CanProduce: true } generatorData)
            {
                _producerBehavior = new ProducerBehavior(this, generatorData);
            }
            _draggableBehavior = new DraggableBehavior(this);
        }
    
        public void SetCurrentCell(Cell cell)
        {
            CurrentCell = cell;
        }

        #region Interface Implementations
        public void OnTap()
        {
            _producerBehavior?.OnTap();
        }

        public void OnBeginDrag()
        {
            _draggableBehavior.OnBeginDrag();
        }

        public void OnDrag(Vector2 position)
        {
            _draggableBehavior.OnDrag(position);
        }

        public void OnEndDrag(DropZone successDropZone)
        {
            _draggableBehavior.OnEndDrag(successDropZone);
        }
        #endregion
    }
}