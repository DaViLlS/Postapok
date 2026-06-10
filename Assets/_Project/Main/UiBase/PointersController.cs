using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace _Project.Main.UiBase
{
    public class PointersController : MonoBehaviour
    {
        [Inject] private IInstantiator _instantiator;
        
        [SerializeField] private WindowPointer windowPointerPrefab;
        [SerializeField] private Transform pointersContainer;
        [SerializeField] private WindowPointer firstPointer;
        
        private List<WindowPointer> _pointers = new List<WindowPointer>();

        private bool _needRotation;

        private void Awake()
        {
            firstPointer.SetId();
            _pointers.Add(firstPointer);
        }

        public void DisablePointer(string id)
        {
            var pointer = _pointers.First(x => x.ID == id);
            pointer.DisablePointer();
        }

        public string SetupPointer(Transform targetPosition)
        {
            var freePointer = _pointers.FirstOrDefault(x => !x.IsEnabled);

            if (freePointer == null)
            {
                freePointer = _instantiator.InstantiatePrefabForComponent<WindowPointer>(windowPointerPrefab, pointersContainer);
                freePointer.SetId();
                _pointers.Add(freePointer);
            }
            
            freePointer.SetTarget(targetPosition);
            freePointer.EnablePointer();

            return freePointer.ID;
        }

        public string SetupPointer(Transform targetPosition, Sprite arrowSprite, Sprite pointerSprite,
            bool needRotation = true)

        {
            _needRotation = needRotation;
            var freePointer = _pointers.FirstOrDefault(x => !x.IsEnabled);

            if (freePointer == null)
            {
                freePointer =
                    _instantiator.InstantiatePrefabForComponent<WindowPointer>(windowPointerPrefab, pointersContainer);
                freePointer.SetId();
                _pointers.Add(freePointer);
            }

            freePointer.SetTarget(targetPosition, arrowSprite, pointerSprite, _needRotation);
            freePointer.EnablePointer();

            return freePointer.ID;
        }
    }
}