using System.Collections.Generic;
using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Data.Orders;
using Sdurlanik.Merge2.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sdurlanik.Merge2.UI
{
    public class OrderUIEntry : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _rewardText;
        [SerializeField] private Transform _requirementsContainer;
        [SerializeField] private Button _completeButton;
        [SerializeField] private GameObject _readyIndicator;

        [Header("Prefabs")]
        [SerializeField] private RequirementIconUI _requirementIconPrefab;

        private Order _currentOrder;
        private readonly List<GameObject> _spawnedRequirementIcons = new();

        private void Awake()
        {
            _completeButton.onClick.AddListener(OnCompleteButtonPressed);
        }

        public void Populate(Order order)
        {
            _currentOrder = order;

            _rewardText.text = order.CalculatedReward.ToString();

            foreach (var icon in _spawnedRequirementIcons)
            {
                Destroy(icon);
            }
            _spawnedRequirementIcons.Clear();
            
            foreach (var requirement in order.Requirements)
            {
                var iconInstance = Instantiate(_requirementIconPrefab, _requirementsContainer);
                iconInstance.SetRequirement(requirement.RequiredItem, requirement.Amount);
                _spawnedRequirementIcons.Add(iconInstance.gameObject);
            }

            var isReady = order.Status == OrderStatus.ReadyToComplete;
            _completeButton.interactable = isReady;
            _readyIndicator.SetActive(isReady);
        }

        private void OnCompleteButtonPressed()
        {
            if (_currentOrder is { Status: OrderStatus.ReadyToComplete })
            {
                ServiceLocator.Get<OrderManager>().CompleteOrder(_currentOrder);
            }
        }
    }
}