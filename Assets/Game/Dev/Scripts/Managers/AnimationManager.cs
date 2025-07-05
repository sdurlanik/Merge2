using DG.Tweening;
using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Items;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sdurlanik.Merge2.Data;

namespace Sdurlanik.Merge2.Managers
{
    public class AnimationManager : MonoBehaviour
    {

        
        [SerializeField] private ItemAnimationSettingsSO _itemAnimationSettings;
        [SerializeField] private UIAnimationSettings _uiAnimationSettings;
        
        #region UI Animations

        public void PlayUIReadyStateAnimation(Transform target)
        {
            target.DOPunchScale(Vector3.one * (_uiAnimationSettings.PopScale - 1f), _uiAnimationSettings.AnimationDuration * 1.5f, (int)_uiAnimationSettings.ReadyVibrato, 1)
                  .SetEase(_uiAnimationSettings.ReadyEase);
        }

        public void PlayUIEntryInAnimation(Transform target, CanvasGroup canvasGroup, float delay)
        {
            target.DOKill();
            canvasGroup.DOKill();

            target.gameObject.SetActive(true);
            canvasGroup.alpha = 0;
            target.localScale = Vector3.one * 0.8f;
            
            DOTween.Sequence()
                .SetDelay(delay)
                .Append(target.DOScale(Vector3.one, _uiAnimationSettings.AnimationDuration * 1.5f).SetEase(_uiAnimationSettings.AppearEase))
                .Join(canvasGroup.DOFade(1f, _uiAnimationSettings.AnimationDuration));
        }

        public void PlayUIEntryOutAnimation(Transform target, CanvasGroup canvasGroup, Action onComplete)
        {
            target.DOKill();
            canvasGroup.DOKill();
            
            DOTween.Sequence()
                .Append(target.DOScale(Vector3.zero, _uiAnimationSettings.AnimationDuration).SetEase(_uiAnimationSettings.DisappearEase))
                .Join(canvasGroup.DOFade(0f, _uiAnimationSettings.AnimationDuration))
                .OnComplete(() => onComplete?.Invoke());
        }

        #endregion

        #region Item Animations

        public void PlayItemMoveAnimation(Transform target, Vector3 targetPosition)
        {
            target.DOKill();
            target.DOMove(targetPosition, _itemAnimationSettings.MoveDuration).SetEase(_itemAnimationSettings.MoveEase);
        }

        public void PlayItemSnapBackAnimation(Transform target)
        {
            target.DOKill();
            target.DOLocalMove(Vector3.zero, _itemAnimationSettings.SnapBackDuration).SetEase(_itemAnimationSettings.SnapBackEase);
        }

        public void PlayItemAppearAnimation(Transform target)
        {
            target.DOKill();
            target.localScale = Vector3.zero;
            target.DOScale(Vector3.one, _itemAnimationSettings.AppearDuration).SetEase(_itemAnimationSettings.AppearEase);
        }
        
        public void PlayItemConsumptionAnimation(Item itemToConsume)
        {
            var itemTransform = itemToConsume.transform;
            var spriteRenderer = itemToConsume.GetComponent<SpriteRenderer>();
            string poolTag = itemToConsume.ItemDataSO.ItemPrefab.name;
            
            itemTransform.DOKill();
            
            DOTween.Sequence()
                .Join(itemTransform.DOScale(Vector3.zero, _itemAnimationSettings.ConsumeDuration).SetEase(_itemAnimationSettings.ConsumeEase))
                .Join(spriteRenderer.DOFade(0f, _itemAnimationSettings.ConsumeDuration))
                .OnComplete(() =>
                {
                    ServiceLocator.Get<ObjectPooler>().ReturnObjectToPool(poolTag, itemToConsume.gameObject);
                    itemTransform.localScale = Vector3.one;
                    spriteRenderer.color = new Color(1, 1, 1, 1);
                });
        }
        
        public void PlayTwoItemMergeAnimation(Item itemA, Item itemB, Vector3 targetPoint, Action onComplete)
        {
            itemA.transform.DOKill();
            itemB.transform.DOKill();

            int animationsCompleted = 0;
            Action onOneAnimationComplete = () =>
            {
                animationsCompleted++;
                if (animationsCompleted == 2)
                {
                    onComplete?.Invoke();
                }
            };

            AnimateSingleItemMerge(itemA, targetPoint, onOneAnimationComplete);
            AnimateSingleItemMerge(itemB, targetPoint, onOneAnimationComplete);
        }

        private void AnimateSingleItemMerge(Item item, Vector3 targetPoint, Action onComplete)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(item.transform.DOMove(targetPoint, _itemAnimationSettings.MergeDuration).SetEase(_itemAnimationSettings.MergeEase))
                .Join(item.transform.DOScale(Vector3.zero, _itemAnimationSettings.MergeDuration))
                .OnComplete(() => onComplete?.Invoke());
        }
        
        public void PlayMergePreviewGrowAnimation(Transform target)
        {
            target.DOKill();
            target.DOScale(Vector3.one * _itemAnimationSettings.MergePreviewScale, _itemAnimationSettings.MergePreviewDuration)
                .SetEase(_itemAnimationSettings.MergePreviewEase);
        }

        public void PlayMergePreviewShrinkAnimation(Transform target)
        {
            target.DOKill();
            target.DOScale(Vector3.one, _itemAnimationSettings.MergePreviewDuration)
                .SetEase(_itemAnimationSettings.MergePreviewEase);
        }
        #endregion
    }
}