using System;
using System.Collections;
using DG.Tweening;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace _Project.CameraControlling
{
    public class CameraController : NetworkBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private CinemachineCamera virtualCamera;
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

        public override void OnNetworkSpawn()
        {
            if (!IsLocalPlayer)
            {
                Destroy(mainCamera.gameObject);
                Destroy(virtualCamera.gameObject);
                return;
            }
            
            _globalVolume = FindAnyObjectByType<Volume>();
            virtualCamera.Follow = trackingTarget;

            if (_globalVolume.profile.TryGet<ChromaticAberration>(out var aberration))
            {
                _aberration = aberration;
                _aberration.intensity.value = 0f;
            }
        }

        private void Update()
        {
            if (!IsOwner)
                return;
            
            HandleZoom();
        }
        
        private void HandleZoom()
        {
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            
            if (scroll != 0)
            {
                var newSize = virtualCamera.Lens.OrthographicSize - scroll * zoomSpeed;
                virtualCamera.Lens.OrthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
            }
        }

        private void EnableFreeCameraEffects()
        {
            StartCoroutine(SmoothChangeFloat(
                () => _aberration.intensity.value,
                value => _aberration.intensity.value = value,
                0.5f,
                0.5f
            ));
        }

        private void DisableFreeCameraEffects()
        {
            StartCoroutine(SmoothChangeFloat(
                () => _aberration.intensity.value,           
                value => _aberration.intensity.value = value, 
                0f,
                0.5f
            ));
        }
        
        private IEnumerator SmoothChangeFloat(System.Func<float> getter, Action<float> setter, float target, float duration)
        {
            var startValue = getter();
            var elapsedTime = 0f;
        
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                var t = elapsedTime / duration;
            
                var currentValue = Mathf.Lerp(startValue, target, t);
                setter(currentValue);
            
                yield return null;
            }
        
            setter(target);
        }
    }
}