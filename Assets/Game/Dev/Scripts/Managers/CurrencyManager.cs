using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Events;
using UnityEngine;

namespace Sdurlanik.Merge2.Managers
{
    public class CurrencyManager : MonoBehaviour
    {
        public int CurrentCoins { get; private set; }

        private void OnEnable()
        {
            EventBus<GrantRewardEvent>.OnEvent += OnRewardGranted;
        }

        private void OnDisable()
        {
            EventBus<GrantRewardEvent>.OnEvent -= OnRewardGranted;
        }

        private void OnRewardGranted(GrantRewardEvent e)
        {
            AddCoins(e.CoinAmount);
        }

        private void AddCoins(int amount)
        {
            if (amount <= 0) return;
            
            CurrentCoins += amount;
            
            EventBus<PlayerCurrencyUpdatedEvent>.Publish(new PlayerCurrencyUpdatedEvent
            {
                NewTotalAmount = CurrentCoins,
                AmountChanged = amount
            });
        }
        
        public void LoadCurrency(int loadedCoins)
        {
            CurrentCoins = loadedCoins;
            EventBus<PlayerCurrencyUpdatedEvent>.Publish(new PlayerCurrencyUpdatedEvent
            {
                NewTotalAmount = CurrentCoins,
                AmountChanged = 0
            });
        }
    }
}