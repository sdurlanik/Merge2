using DG.Tweening;
using UnityEngine;

namespace Sdurlanik.Merge2.Data
{
    [CreateAssetMenu(fileName = "ItemAnimationSettings", menuName = "Merge2/Settings/Item Animation Settings")]
    public class ItemAnimationSettingsSO : ScriptableObject
    {
        public float MoveDuration = 0.25f;
        public Ease MoveEase = Ease.OutQuad;
        public float SnapBackDuration = 0.3f;
        public Ease SnapBackEase = Ease.OutBack;
        public float MergeDuration = 0.2f;
        public Ease MergeEase = Ease.InBack;
        public float AppearDuration = 0.3f;
        public Ease AppearEase = Ease.OutBack;
        public float ConsumeDuration = 0.3f;
        public Ease ConsumeEase = Ease.InBack;
        
        public float MergePreviewScale = 1.2f; 
        public float MergePreviewDuration = 0.2f;
        public Ease MergePreviewEase = Ease.OutSine;
        
        public Vector3 TapPunchAmount = new Vector3(0.2f, 0.2f, 0f);
        public float TapAnimationDuration = 0.5f;
        public int TapVibrato = 10;
        public float TapElasticity = 0.5f;
    }
}