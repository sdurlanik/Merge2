using System.Collections.Generic;
using UnityEngine;

namespace Sdurlanik.Merge2.Data
{
    [CreateAssetMenu(fileName = "Generator", menuName = "Merge2/Items/Generator")]
    public class GeneratorSO : ItemSO // ItemSO'dan miras alıyor
    {
        [Header("Producer Settings")]
        [SerializeField] private bool _canProduce;
        [SerializeField] private List<ProductionChance> _productionChances;

        public bool CanProduce => _canProduce;
        public IReadOnlyList<ProductionChance> ProductionChances => _productionChances;
    }
}