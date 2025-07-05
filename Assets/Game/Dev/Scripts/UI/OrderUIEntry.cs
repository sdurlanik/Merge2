// Konum: Sdurlanik.Merge2/Scripts/UI/OrderUIEntry.cs

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
        [SerializeField] private GameObject _readyIndicator;

        [Header("Prefabs")]
        [SerializeField] private RequirementIconUI _requirementIconPrefab;

        [Header("Animation Settings")]
        [SerializeField] private float _popScale = 1.1f;
        [SerializeField] private float _animationDuration = 0.3f;

        public Order CurrentOrder { get; private set; }
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _completeButton.onClick.AddListener(OnCompleteButtonPressed);
        }

        private void OnDisable()
        {
            transform.DOKill();
        }

        public void InitializeAndAnimateIn(Order order, float delay)
        {
            transform.DOKill();
            PopulateData(order);
            gameObject.SetActive(true);
            _canvasGroup.alpha = 0;
            transform.localScale = Vector3.one * 0.8f;
            DOTween.Sequence()
                .SetDelay(delay)
                .Append(transform.DOScale(Vector3.one, _animationDuration * 1.5f).SetEase(Ease.OutBack))
                .Join(_canvasGroup.DOFade(1f, _animationDuration));
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
            _completeButton.interactable = false;
            transform.DOKill();
            DOTween.Sequence()
                .Append(transform.DOScale(Vector3.zero, _animationDuration).SetEase(Ease.InBack))
                .Join(_canvasGroup.DOFade(0f, _animationDuration))
                .OnComplete(() =>
                {
                    InitializeAndAnimateIn(newOrder, 0f);
                });
        }
        
        private void PopulateData(Order order)
        {
            CurrentOrder = order;
            _rewardText.text = CurrentOrder.CalculatedReward.ToString();
            foreach (Transform child in _requirementsContainer) Destroy(child.gameObject);
            foreach (var requirement in CurrentOrder.Requirements)
            {
                var iconInstance = Instantiate(_requirementIconPrefab, _requirementsContainer);
                iconInstance.SetRequirement(requirement.RequiredItem, requirement.Amount);
            }
            _completeButton.interactable = CurrentOrder.Status == OrderStatus.ReadyToComplete;
            _readyIndicator.SetActive(CurrentOrder.Status == OrderStatus.ReadyToComplete);
        }

        private void OnCompleteButtonPressed()
        {
            if (CurrentOrder != null && CurrentOrder.Status == OrderStatus.ReadyToComplete)
            {
                _completeButton.interactable = false; // Tekrar basılmasını önle
                ServiceLocator.Get<OrderManager>().CompleteOrder(CurrentOrder);
            }
        }
        
        private void AnimateReadyStatePop()
        {
            transform.DOPunchScale(Vector3.one * (_popScale - 1f), 0.5f, 10, 1);
        }
    }
}