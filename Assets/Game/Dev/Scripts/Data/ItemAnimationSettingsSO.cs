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
    }
}