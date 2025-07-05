using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sdurlanik.Merge2.Data;

namespace Sdurlanik.Merge2.UI
{
    public class RequirementIconUI : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;

        public void SetRequirement(ItemSO requiredItem)
        {
            _iconImage.sprite = requiredItem.Icon;
        }
    }
}