using System;
using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Data;
using Sdurlanik.Merge2.GridSystem;
using Sdurlanik.Merge2.Items;
using Sdurlanik.Merge2.Services;
using UnityEngine;

namespace Sdurlanik.Merge2.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private PoolSettingsSO _poolSettings;
        
        protected override void Awake()
        {
            base.Awake();
            Application.targetFrameRate = 60;
        }
        private void Start()
        {
            CreateItemPools();
            LoadGameState();
        }
        
        private void CreateItemPools()
        { 
            ObjectPooler.Instance.CreatePool(_poolSettings);
        }
        
        private void OnApplicationQuit()
        {
            SaveGameState();
        }
        
        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SaveGameState();
            }
        }
        
        private void SaveGameState()
        {
            var data = new PlayerData
            {
                Coins = CurrencyManager.Instance.CurrentCoins,
                GridItems = GridManager.Instance.GetItemsForSaving(),
                ActiveOrderSONames = OrderManager.Instance.GetOrdersForSaving()
            };
            
            SaveLoadService.SaveGame(data);
        }
        
        private void LoadGameState()
        {
            Debug.Log("Loading game state...");
            var data = SaveLoadService.LoadGame();
            
            CurrencyManager.Instance.LoadCurrency(data.Coins);
            
            GridManager.Instance.CreateGrid();
            
            if (data.GridItems.Count > 0)
            {
                GridManager.Instance.LoadItemsFromSave(data.GridItems);
                OrderManager.Instance.LoadOrdersFromSave(data.ActiveOrderSONames);
            }
            else
            {
                BoardSetupService.SetupInitialBoard();
                OrderManager.Instance.TryToGenerateNewOrders();
            }
        }
    }
}