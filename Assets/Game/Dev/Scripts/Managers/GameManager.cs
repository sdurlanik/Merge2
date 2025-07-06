using System;
using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Data;
using Sdurlanik.Merge2.GridSystem;
using Sdurlanik.Merge2.Items;
using Sdurlanik.Merge2.Services;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sdurlanik.Merge2.Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Manager References")]
        [SerializeField] private GridManager _gridManager;
        [SerializeField] private OrderManager _orderManager;
        [SerializeField] private CurrencyManager _currencyManager;
        [SerializeField] private DataBank _dataBank;
        [SerializeField] private ItemInteractionManager _itemInteractionManager;
        [SerializeField] private ObjectPooler _objectPooler;
        [SerializeField] private AnimationManager _animationManager;
        [SerializeField] private OrderHighlightManager _orderHighlightManager;
        
        [SerializeField] private PoolSettingsSO _poolSettings;
        [FormerlySerializedAs("_currentLevelDesign")] [SerializeField] private LevelDesignSettingsSO _currentLevelDesignSettings;
        
        private void Awake()
        {
            ServiceLocator.Register(_animationManager);
            ServiceLocator.Register(_orderHighlightManager);
            ServiceLocator.Register(_orderManager);
            ServiceLocator.Register(_gridManager);
            ServiceLocator.Register(_currencyManager);
            ServiceLocator.Register(_dataBank);
            ServiceLocator.Register(_itemInteractionManager);
            ServiceLocator.Register(_objectPooler);
            
            Application.targetFrameRate = 60;
        }
        private void Start()
        {
            CreateItemPools();
            LoadGameState();
        }
        
        private void CreateItemPools()
        { 
           ServiceLocator.Get<ObjectPooler>().CreatePool(_poolSettings);
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
            Debug.Log("Saving game state...");
            var (items, cells) = ServiceLocator.Get<GridManager>().GetItemsForSaving();
        
            var data = new PlayerData
            {
                Coins = ServiceLocator.Get<CurrencyManager>().CurrentCoins,
                GridItems = items,
                CellStates = cells,
                ActiveOrderSONames = ServiceLocator.Get<OrderManager>().GetOrdersForSaving()
            };
        
            SaveLoadService.SaveGame(data);
        }
        
        private void LoadGameState()
        {
            Debug.Log("Loading game state...");
            var data = SaveLoadService.LoadGame();
            
            ServiceLocator.Get<CurrencyManager>().LoadCurrency(data.Coins);
            ServiceLocator.Get<GridManager>().CreateGrid();
            
            if (data.CellStates.Count > 0)
            {
                ServiceLocator.Get<GridManager>().LoadItemsFromSave(data.GridItems, data.CellStates);
                ServiceLocator.Get<OrderManager>().LoadOrdersFromSave(data.ActiveOrderSONames);
            }
            else 
            {
                BoardSetupService.SetupInitialBoard(_currentLevelDesignSettings);
                ServiceLocator.Get<OrderManager>().TryToGenerateNewOrders();
            }
        }
    }
}