using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.CameraControlling
{
    public class MainMenuCamera : MonoBehaviour
    {
        [Header("Cinemachine")]
        [SerializeField] private CinemachineCamera virtualCamera;
        [SerializeField] private CinemachineConfiner2D confiner;
        
        [Header("Drag Settings")]
        [SerializeField] private float dragSpeed = 0.5f;
        [SerializeField] private float smoothTime = 0.1f;
        [SerializeField] private Transform cameraTarget;
        [SerializeField] private float minX;
        [SerializeField] private float maxX;
        [SerializeField] private float minY;
        [SerializeField] private float maxY;
        
        [Header("Zoom Settings")]
        [SerializeField] private float zoomSpeed = 2f;
        [SerializeField] private float minZoom = 1f;
        [SerializeField] private float maxZoom = 20f;
        
        private Vector3 _dragStartMouseScreen;
        private Vector3 _dragStartCameraWorld;
        private Vector3 _targetPosition;
        private Vector3 _velocity;
        private bool _isDragging;

        private bool _isLocked;

        private void Start()
        {
            if (virtualCamera == null)
                virtualCamera = GetComponent<CinemachineCamera>();
            
            cameraTarget.transform.position = virtualCamera.transform.position;
            virtualCamera.Follow = cameraTarget;
            
            _targetPosition = cameraTarget.position;
        }

        private void Update()
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;
            
            if (_isLocked)
                return;
            
            HandleMouseInput();
            UpdateCamera();
        }

        private void HandleMouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _dragStartMouseScreen = Input.mousePosition;
                _dragStartCameraWorld = cameraTarget.position;
                _isDragging = true;
                _velocity = Vector3.zero;
            }
            
            if (_isDragging && Input.GetMouseButton(0))
            {
                var currentMouseScreen = Input.mousePosition;
                var screenDelta = _dragStartMouseScreen - currentMouseScreen;
                
                var pixelsPerUnit = Screen.height / (virtualCamera.Lens.OrthographicSize * 2f);
                var worldDelta = screenDelta / pixelsPerUnit;
                
                var newPosition = _dragStartCameraWorld + worldDelta * dragSpeed;
        
                _targetPosition = ClampCameraPosition(newPosition);
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
            }
            
            HandleZoom();
            
            _targetPosition = ClampCameraPosition(_targetPosition);
        }

        private void HandleZoom()
        {
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            
            if (scroll != 0)
            {
                var newSize = virtualCamera.Lens.OrthographicSize - scroll * zoomSpeed;
                virtualCamera.Lens.OrthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
                confiner.InvalidateLensCache();
            }
        }

        private void UpdateCamera()
        {
            cameraTarget.position = Vector3.SmoothDamp(
                cameraTarget.position,
                _targetPosition,
                ref _velocity,
                smoothTime
            );
        }
        
        private Vector3 ClampCameraPosition(Vector3 position)
        {
            var orthoSize = virtualCamera.Lens.OrthographicSize;
            var aspectRatio = (float)Screen.width / Screen.height;
    
            var cameraHeight = orthoSize;
            var cameraWidth = cameraHeight * aspectRatio;
    
            var adjustedMinX = minX + cameraWidth;
            var adjustedMaxX = maxX - cameraWidth;
            var adjustedMinY = minY + cameraHeight;
            var adjustedMaxY = maxY - cameraHeight;
            
            if (adjustedMinX > adjustedMaxX)
            {
                adjustedMinX = adjustedMaxX = (minX + maxX) / 2f;
            }
    
            if (adjustedMinY > adjustedMaxY)
            {
                adjustedMinY = adjustedMaxY = (minY + maxY) / 2f;
            }
    
            var clampedX = Mathf.Clamp(position.x, adjustedMinX, adjustedMaxX);
            var clampedY = Mathf.Clamp(position.y, adjustedMinY, adjustedMaxY);
    
            return new Vector3(clampedX, clampedY, position.z);
        }

        public void UnlockCamera()
        {
            _isLocked = false;
        }

        public void LockCamera()
        {
            _isLocked = true;
        }
        
        // Для отладки
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying || cameraTarget == null) return;
            
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(cameraTarget.position, 0.3f);
        }
    }
}