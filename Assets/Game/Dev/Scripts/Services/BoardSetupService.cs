using System.Collections.Generic;
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
           var itemsToSpawn = new List<ItemSO>
           {
               DataBank.Instance.GetSO(ItemFamily.G1, 1),
               DataBank.Instance.GetSO(ItemFamily.G1, 1),
               DataBank.Instance.GetSO(ItemFamily.G1, 2),
               DataBank.Instance.GetSO(ItemFamily.G1, 3),
               DataBank.Instance.GetSO(ItemFamily.G1, 4)
           };
           
           foreach (var so in itemsToSpawn)
           {
               if (GridManager.Instance.TryGetEmptyCell(out var cell))
               {
                   ItemFactory.Create(so, cell);
               }
           }
       }
   }
}