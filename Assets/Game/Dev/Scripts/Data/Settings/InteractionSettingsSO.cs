using UnityEngine;
using UnityEngine.Serialization;

namespace Sdurlanik.Merge2.Data
{
        [CreateAssetMenu(fileName = "InteractionSettings", menuName = "Merge2/Settings/Interaction Settings")]
        public class InteractionSettingsSO : ScriptableObject
        {
            [Header("Interaction Settings")]
            public LayerMask InteractableLayer;
            public float DragThreshold = 0.2f;

            [Header("Drop Settings")]
            public float DropDetectionRadius = .2f;
            public LayerMask DropZoneLayer;
            
            [Header("Rendering Settings")]
            public string DraggingSortingLayerName = "DraggingItem";
        } 
}