using DG.Tweening;
using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Data.Orders;
using Sdurlanik.Merge2.Managers;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sdurlanik.Merge2.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class OrderUIEntry : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _rewardText;
        [SerializeField] private Transform _requirementsContainer;
        [SerializeField] private Button _completeButton;
        [SerializeField] private Image _avatarImage;

        [Header("Prefabs")]
        [SerializeField] private RequirementIconUI _requirementIconPrefab;
        
        public Order CurrentOrder { get; private set; }
        private CanvasGroup _canvasGroup;
        private Sequence _currentSequence;
        private OrderStatus _lastKnownStatus;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
            _completeButton.onClick.AddListener(OnCompleteButtonPressed);
        }

        private void OnDisable()
        {
            _currentSequence?.Kill();
            transform.DOKill();
        }

        public void SetData(Order newOrder, int index)
        {
            _currentSequence?.Kill();

            if (CurrentOrder != null && CurrentOrder.Id == newOrder.Id)
            {
                CurrentOrder = newOrder; 
                UpdateDisplayStateOnly();
                return;
            }
            
            if (gameObject.activeSelf && CurrentOrder != null)
            {
                AnimateOutAndRepopulate(newOrder, index); 
            }
            else
            {
                InitializeAndAnimateIn(newOrder, index);
            }
        }

        private void InitializeAndAnimateIn(Order order, int index)
        {
            gameObject.SetActive(true);
            PopulateData(order);
            var delay = index * 0.1f;
            ServiceLocator.Get<AnimationManager>().PlayUIEntryInAnimation(transform, _canvasGroup, delay);
        }

        private void AnimateOutAndRepopulate(Order newOrder, int index)
        {
            _completeButton.gameObject.SetActive(false);

            ServiceLocator.Get<AnimationManager>().PlayUIEntryOutAnimation(transform, _canvasGroup, OnOutComplete);
            return;

            void OnOutComplete()
            {
                InitializeAndAnimateIn(newOrder, index);
            }
        }

        private void PopulateData(Order order)
        {
            _lastKnownStatus = CurrentOrder?.Status ?? OrderStatus.Completed;
            CurrentOrder = order;

            _rewardText.text = order.CalculatedReward.ToString();
            if (order.AvatarSprite != null) _avatarImage.sprite = order.AvatarSprite;

            foreach (Transform child in _requirementsContainer) Destroy(child.gameObject);
            foreach (var requirement in order.Requirements)
            {
                var iconInstance = Instantiate(_requirementIconPrefab, _requirementsContainer);
                iconInstance.SetRequirement(requirement.RequiredItem);
            }
            UpdateDisplayStateOnly();
        }
        
        private void UpdateDisplayStateOnly()
        {
            if (CurrentOrder == null) return;

            var isNowReady = CurrentOrder.Status == OrderStatus.ReadyToComplete;
            if (isNowReady && _lastKnownStatus != OrderStatus.ReadyToComplete)
            {
                ServiceLocator.Get<AnimationManager>().PlayUIReadyStateAnimation(transform);
            }
            _completeButton.gameObject.SetActive(isNowReady);
            
            _lastKnownStatus = CurrentOrder.Status;
        }

        public void Hide()
        {
            _currentSequence?.Kill();
            gameObject.SetActive(false);
            CurrentOrder = null;
        }

        private void OnCompleteButtonPressed()
        {
            if (CurrentOrder != null && CurrentOrder.Status == OrderStatus.ReadyToComplete)
            {
                _completeButton.gameObject.SetActive(false);
                ServiceLocator.Get<OrderManager>().CompleteOrder(CurrentOrder);
            }
        }
    }
}