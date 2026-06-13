using System.Collections;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace _Project.CameraControlling
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera mainCamera;
        [SerializeField] private CinemachineFollow cinemachineFollow;
        [SerializeField] private Transform trackingTarget;
        [SerializeField] private float cameraSpeed;
        [Header("Zoom Settings")]
        [SerializeField] private float zoomSpeed = 2f;
        [SerializeField] private float minZoom = 1f;
        [SerializeField] private float maxZoom = 20f;

        private ChromaticAberration _aberration;
        private Volume _globalVolume;
        
        public bool CanMove { get; private set; }

        private Tween _tween;

        private void Awake()
        {
            _globalVolume = FindAnyObjectByType<Volume>();
            mainCamera.Follow = trackingTarget;

            if (_globalVolume.profile.TryGet<ChromaticAberration>(out var aberration))
            {
                _aberration = aberration;
                _aberration.intensity.value = 0f;
            }
        }
        
        private void Update()
        {
            HandleZoom();
        }
        
        private void HandleZoom()
        {
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            
            if (scroll != 0)
            {
                var newSize = mainCamera.Lens.OrthographicSize - scroll * zoomSpeed;
                mainCamera.Lens.OrthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
            }
        }

        private void EnableFreeCameraEffects()
        {
            StartCoroutine(SmoothChangeFloat(
                () => _aberration.intensity.value,           // getter
                value => _aberration.intensity.value = value, // setter
                0.5f,
                0.5f
            ));
        }

        private void DisableFreeCameraEffects()
        {
            StartCoroutine(SmoothChangeFloat(
                () => _aberration.intensity.value,           // getter
                value => _aberration.intensity.value = value, // setter
                0f,
                0.5f
            ));
        }
        
        private IEnumerator SmoothChangeFloat(System.Func<float> getter, 
            System.Action<float> setter, 
            float target, 
            float duration)
        {
            float startValue = getter();
            float elapsedTime = 0;
        
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
            
                // Плавная интерполяция
                float currentValue = Mathf.Lerp(startValue, target, t);
                setter(currentValue);
            
                yield return null;
            }
        
            // Убеждаемся, что достигли точного целевого значения
            setter(target);
        }
    }
}