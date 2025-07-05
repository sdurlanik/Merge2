using System;
using DG.Tweening;
using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Data.Orders;
using Sdurlanik.Merge2.Managers;
using System.Collections.Generic;
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
        private AnimationManager _animationManager;
        private OrderStatus _previousStatus;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _completeButton.onClick.AddListener(OnCompleteButtonPressed);
        }

        private void Start()
        {
            _animationManager = ServiceLocator.Get<AnimationManager>();
        }

        private void OnDisable()
        {
            transform.DOKill();
        }

        public void InitializeAndAnimateIn(Order order, float delay)
        {
            PopulateData(order);
            ServiceLocator.Get<AnimationManager>().PlayUIEntryInAnimation(transform, _canvasGroup, delay);
        }

        public void UpdateDisplay(Order order)
        {
            var oldStatus = CurrentOrder?.Status;
            CurrentOrder = order; 
            PopulateData(order);
            
            var newStatus = CurrentOrder.Status;
            if (newStatus == OrderStatus.ReadyToComplete && oldStatus != OrderStatus.ReadyToComplete)
            {
                AnimateReadyStatePop();
            }
        }

        public void AnimateOutAndRepopulate(Order newOrder)
        {
            _completeButton.gameObject.SetActive(false);
            _animationManager.PlayUIEntryOutAnimation(transform, _canvasGroup, () =>
            {
                InitializeAndAnimateIn(newOrder, 0f);
            });
        }
        
        private void PopulateData(Order order)
        {
            CurrentOrder = order;
            _rewardText.text = CurrentOrder.CalculatedReward.ToString();
            _avatarImage.sprite = CurrentOrder.AvatarSprite;
            foreach (Transform child in _requirementsContainer) Destroy(child.gameObject);
            foreach (var requirement in CurrentOrder.Requirements)
            {
                var iconInstance = Instantiate(_requirementIconPrefab, _requirementsContainer);
                iconInstance.SetRequirement(requirement.RequiredItem);
            }

            var isNowReady = CurrentOrder.Status == OrderStatus.ReadyToComplete;
            _completeButton.gameObject.SetActive(isNowReady);
            
            if (isNowReady && _previousStatus != OrderStatus.ReadyToComplete)
            {
                ServiceLocator.Get<AnimationManager>().PlayUIReadyStateAnimation(_completeButton.transform);
            }
            
            _previousStatus = CurrentOrder.Status;
        }

        private void OnCompleteButtonPressed()
        {
            if (CurrentOrder != null && CurrentOrder.Status == OrderStatus.ReadyToComplete)
            {
                _completeButton.gameObject.SetActive(false);
                ServiceLocator.Get<OrderManager>().CompleteOrder(CurrentOrder);
            }
        }
        
        private void AnimateReadyStatePop()
        {
            _animationManager.PlayUIReadyStateAnimation(transform);
        }
    }
}