using System;
using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Events;
using Sdurlanik.Merge2.Managers;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Sequence = DG.Tweening.Sequence;

namespace Sdurlanik.Merge2.UI
{
    public class CurrencyUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _coinText;
        
        [Header("Settings")]
        [SerializeField] private string _currencyFormat = "{0}";

        [Header("Animation Settings")]
        [SerializeField] private float _popScale = 1.2f;
        [SerializeField] private float _popDuration = 0.1f;
        [SerializeField] private float _returnDuration = 0.2f;

        private Sequence _popAnimationSequence;

        private void OnEnable()
        {
            EventBus<PlayerCurrencyUpdatedEvent>.OnEvent += HandleCurrencyUpdated;
        }

        private void Start()
        {
            UpdateText(ServiceLocator.Get<CurrencyManager>().CurrentCoins);
        }

        private void OnDisable()
        {
            EventBus<PlayerCurrencyUpdatedEvent>.OnEvent -= HandleCurrencyUpdated;
            _popAnimationSequence?.Kill();
        }

        private void HandleCurrencyUpdated(PlayerCurrencyUpdatedEvent e)
        {
            UpdateText(e.NewTotalAmount);
            
            if (e.AmountChanged > 0)
            {
                PlayPopAnimation();
            }
        }

        private void UpdateText(int amount)
        {
            _coinText.text = string.Format(_currencyFormat, amount);
        }
        
        private void PlayPopAnimation()
        {
            _popAnimationSequence?.Kill();
            
            _popAnimationSequence = DOTween.Sequence();
            
            var originalScale = _coinText.transform.localScale;

            _popAnimationSequence
                .Append(_coinText.transform.DOScale(originalScale * _popScale, _popDuration)
                    .SetEase(Ease.OutQuad)) 
                
                .Append(_coinText.transform.DOScale(originalScale, _returnDuration)
                    .SetEase(Ease.OutBounce));
        }
    }
}