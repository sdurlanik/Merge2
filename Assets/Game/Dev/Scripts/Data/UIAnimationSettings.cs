using DG.Tweening;
using UnityEngine;

namespace Sdurlanik.Merge2.Data
{
    [CreateAssetMenu(fileName = "UIAnimationSettings", menuName = "Merge2/Settings/UI Animation Settings")]
    public class UIAnimationSettings : ScriptableObject
    {
       public float PopScale = 1.1f;
       public float ReadyVibrato = 10f;
       public float AnimationDuration = 0.3f;
       public Ease AppearEase = Ease.OutBack;
       public Ease DisappearEase = Ease.InBack;
       public Ease ReadyEase = Ease.OutElastic;
    }
}