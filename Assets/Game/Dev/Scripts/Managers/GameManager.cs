using System;
using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Data;
using Sdurlanik.Merge2.GridSystem;
using Sdurlanik.Merge2.Items;
using Sdurlanik.Merge2.Services;
using UnityEngine;

namespace Sdurlanik.Merge2.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private PoolSettingsSO _poolSettings;
        private void Start()
        {
            GridManager.Instance.CreateGrid();
            CreateItemPools();
            BoardSetupService.SetupInitialBoard();
            OrderManager.Instance.TryToGenerateNewOrders();
        }
        
        private void CreateItemPools()
        { 
            ObjectPooler.Instance.CreatePool(_poolSettings);
        }
    }
}