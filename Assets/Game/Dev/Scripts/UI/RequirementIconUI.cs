using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sdurlanik.Merge2.Data;

namespace Sdurlanik.Merge2.UI
{
    public class RequirementIconUI : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _amountText;

        public void SetRequirement(ItemSO requiredItem, int amount)
        {
            _iconImage.sprite = requiredItem.Icon;
            _amountText.text = $"x{amount}";
        }
    }
}