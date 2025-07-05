using System.Collections.Generic;
using UnityEngine;

namespace Sdurlanik.Merge2.Data.Orders
{
    [CreateAssetMenu(fileName = "OrderSettings", menuName = "Merge2/Settings/OrderSettings", order = 1)]
    public class OrderSettingsSO : ScriptableObject
    {
        public int MaxActiveOrders = 3;
        public List<Sprite> CustomerAvatars = new();
    }
}