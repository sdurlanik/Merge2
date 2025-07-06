using UnityEngine;

namespace Sdurlanik.Merge2.Data
{
    [CreateAssetMenu(fileName = "CameraSettings", menuName = "Merge2/Settings/Camera Settings")]
    public class CameraSettingsSO : ScriptableObject
    {
        public float ReferenceWidth = 1080f;
        
        public float ReferenceHeight = 1920f;
        
        public float ReferenceOrthographicSize = 14f;
    }
}