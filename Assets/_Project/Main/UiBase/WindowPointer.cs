using System;
using CodeMonkey.Utils;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Main.UiBase
{
    public class WindowPointer : MonoBehaviour
    {
        [Inject] private Camera _uiCamera;
        
        [SerializeField] private RectTransform pointerRect;
        [SerializeField] private Image pointerImage;
        [SerializeField] private Sprite arrowSprite;
        [SerializeField] private Sprite crossSprite;
        
        private Transform _target;
        private bool _needRotation = true;
        
        private Sprite _arrowSprite;
        private Sprite _crossSprite;
        
        public string ID { get; private set; }
        public bool IsEnabled { get; private set; }

        public void EnablePointer()
        {
            IsEnabled = true;   
            gameObject.SetActive(true);
        }

        public void DisablePointer()
        {
            _arrowSprite = null;
            _crossSprite = null;
            IsEnabled = false;
            gameObject.SetActive(false);
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public void SetTarget(Transform target, Sprite newArrowSprite, Sprite newCrossSprite, bool needRotation = true)
        {
            _needRotation = needRotation;
            _target = target;
            _arrowSprite = newArrowSprite;
            _crossSprite = newCrossSprite;
        }

        public void SetId()
        {
            ID = Guid.NewGuid().ToString();
        }

        private void Update()
        {
            if (!IsEnabled)
                return;
            
            var borderSize = 200f;
            var targetPositionScreenPoint = _uiCamera.WorldToScreenPoint(_target.position);
            var isOffScreen = targetPositionScreenPoint.x <= borderSize 
                              || targetPositionScreenPoint.x >= Screen.width - borderSize 
                              || targetPositionScreenPoint.y <= borderSize 
                              || targetPositionScreenPoint.y >= Screen.height - borderSize;

            if (isOffScreen)
            {
                if (_needRotation)
                    RotatePointerTowardsTarget();
                
                pointerImage.sprite = _arrowSprite == null ? arrowSprite : _arrowSprite;
                var cappedTargetScreenPosition = targetPositionScreenPoint;

                if (cappedTargetScreenPosition.x <= borderSize)
                    cappedTargetScreenPosition.x = borderSize;
                
                if (cappedTargetScreenPosition.x >= Screen.width - borderSize)
                    cappedTargetScreenPosition.x = Screen.width - borderSize;
                    
                if (cappedTargetScreenPosition.y <= borderSize)
                    cappedTargetScreenPosition.y = borderSize;
                
                if (cappedTargetScreenPosition.y >= Screen.height - borderSize)
                    cappedTargetScreenPosition.y = Screen.height - borderSize;

                var pointerWorldPosition = _uiCamera.ScreenToWorldPoint(cappedTargetScreenPosition);
                pointerRect.position = pointerWorldPosition;
                pointerRect.localPosition = new Vector3(pointerRect.localPosition.x, pointerRect.localPosition.y, 0f);
            }
            else
            {
                pointerRect.localEulerAngles = Vector3.zero;
                pointerImage.sprite = _crossSprite == null ? crossSprite : _crossSprite;
                var pointerWorldPosition = _uiCamera.ScreenToWorldPoint(targetPositionScreenPoint);
                pointerRect.position = pointerWorldPosition;
                pointerRect.localPosition = new Vector3(pointerRect.localPosition.x, pointerRect.localPosition.y, 0f);
            }
        }

        private void RotatePointerTowardsTarget()
        {
            var toPosition = _target.position;
            var fromPosition = _uiCamera.transform.position;

            fromPosition.z = 0f;
            var dir = (toPosition - fromPosition).normalized;
            var angle = UtilsClass.GetAngleFromVectorFloat(dir);
            pointerRect.localEulerAngles = new Vector3(0, 0, angle);
        }
    }
}