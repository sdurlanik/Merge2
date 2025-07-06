using DG.Tweening;
using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Data.Orders;
using Sdurlanik.Merge2.Managers;
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
        private OrderStatus _lastKnownStatus;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
            _completeButton.onClick.AddListener(OnCompleteButtonPressed);
        }

        public void InitializeAndAnimateIn(Order order, float delay)
        {
            PopulateData(order);
            ServiceLocator.Get<AnimationManager>().PlayUIEntryInAnimation(transform, _canvasGroup, delay);
        }

        public void UpdateDisplay(Order order)
        {
            
            var isNowReady = order.Status == OrderStatus.ReadyToComplete;
            if (isNowReady && _lastKnownStatus  != OrderStatus.ReadyToComplete)
            {
                Debug.Log("Order is now ready to complete: " + order.OrderData.OrderName);
                AnimateReadyStatePop();
            }
            
            PopulateData(order);
        }
        
        public void AnimateOut(System.Action onComplete = null)
        {
            ServiceLocator.Get<AnimationManager>().PlayUIEntryOutAnimation(transform, _canvasGroup, () =>
            {
                gameObject.SetActive(false);
                CurrentOrder = null;
                onComplete?.Invoke();
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

            var isActive = CurrentOrder.Status == OrderStatus.ReadyToComplete;
            _completeButton.gameObject.SetActive(isActive);
            _lastKnownStatus = CurrentOrder.Status;
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
            ServiceLocator.Get<AnimationManager>().PlayUIReadyStateAnimation(transform);
            
        }
    }
}