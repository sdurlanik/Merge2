using Sdurlanik.Merge2.Data;
using UnityEngine;

namespace Sdurlanik.Merge2.Core
{
    [RequireComponent(typeof(Camera))]
    public class CameraResizer : MonoBehaviour
    {
        [SerializeField] private CameraSettingsSO _cameraSettings;

        private Camera _camera;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            AdjustCameraSize();
        }

        private void AdjustCameraSize()
        {
            var referenceAspect = _cameraSettings.ReferenceWidth / _cameraSettings.ReferenceHeight;
            
            var currentAspect = (float)Screen.width / Screen.height;
            
            var newOrthographicSize = _cameraSettings.ReferenceOrthographicSize * (referenceAspect / currentAspect);
            
            _camera.orthographicSize = newOrthographicSize;
        }
    }
}