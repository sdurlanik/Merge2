using System.Collections.Generic;
using Sdurlanik.Merge2.Core;
using Sdurlanik.Merge2.Data;
using Sdurlanik.Merge2.GridSystem;
using Sdurlanik.Merge2.Managers;
using UnityEngine;

namespace Sdurlanik.Merge2.Services
{
   public static class BoardSetupService
   {
       public static void SetupInitialBoard()
       {
           var dataBank = ServiceLocator.Get<DataBank>();
           var itemsToSpawn = new List<ItemSO>
           {
               dataBank.GetSO(ItemFamily.G1, 1),
               dataBank.GetSO(ItemFamily.G1, 1),
               dataBank.GetSO(ItemFamily.G1, 2),
               dataBank.GetSO(ItemFamily.G1, 3),
               dataBank.GetSO(ItemFamily.G1, 4)
           };
           
           foreach (var so in itemsToSpawn)
           {
               if (ServiceLocator.Get<GridManager>().TryGetEmptyCell(out var cell))
               {
                   ItemFactory.Create(so, cell);
               }
           }
       }
   }
}