using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace _Project.WorldClicking.Scripts
{
    public class WorldClickingController : MonoBehaviour
    {
        [Inject] private IInstantiator _instantiator;
        [Inject] private DiContainer _diContainer;

        [SerializeField] private GUISkin skin;
        [SerializeField] private LayerMask squadLayer;
        [SerializeField] private LayerMask attackLayer;
        [SerializeField] private LayerMask defendLayer;
        [SerializeField] private LayerMask mainCharacterLayer;
        [SerializeField] private LayerMask destroyLayer;
        [SerializeField] private LayerMask interactableLayer;
        [Header("Debug")] 
        [SerializeField] private bool isTesting = false;
        
        private bool _draw;
        private Vector2 _startPos;
        private Vector2 _endPos;
        private Rect _rect;
        
        private Camera _camera;

        public void Initialize()
        {
            _camera = Camera.main;
        }
        
        private void OnDestroy()
        {
            
        }
        
        private void OnGUI()
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;
            
            GUI.skin = skin;
            GUI.depth = 99;

            if (Input.GetMouseButtonDown(0))
            {
                _startPos = Input.mousePosition;
                _draw = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _draw = false;
            }
		
            if (_draw)
            {
                _endPos = Input.mousePosition;
                
                if (_startPos == _endPos)
                    return;

                _rect = new Rect(Mathf.Min(_endPos.x, _startPos.x),
                    Screen.height - Mathf.Max(_endPos.y, _startPos.y),
                    Mathf.Max(_endPos.x, _startPos.x) - Mathf.Min(_endPos.x, _startPos.x),
                    Mathf.Max(_endPos.y, _startPos.y) - Mathf.Min(_endPos.y, _startPos.y)
                );
			
                GUI.Box(_rect, "");
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(1))
            {
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                    return;
                
                if (CheckForClickOnDefendLayer(out var defendable))
                {
                    
                    return;
                }

                if (CheckForClickOnDestroyLayer(out var destroyable))
                {
                    
                    return;
                }
                
                var mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                    return;
                
                if (_startPos != _endPos)
                    return;

                if (CheckForClickOnInteractable(out var interactable))
                {
                    interactable.Interact();
                    return;
                }
            }
        }
        
        private bool CheckForClickOnSquad()
        {
            var mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        
            var hit = Physics2D.Raycast(mousePosition, Vector2.zero, 
                Mathf.Infinity, squadLayer);
        
            if (hit.collider != null)
            {
                var clickable = hit.collider.GetComponent<IClickable>();
                
                if (clickable != null)
                {
                    clickable.OnClick();
                    return true;
                }
            }
            
            return false;
        }

        private bool CheckForClickOnDefendLayer(out IDefendable defendable)
        {
            defendable = null;
            var mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        
            var hit = Physics2D.Raycast(mousePosition, Vector2.zero, 
                Mathf.Infinity, defendLayer);
            
            var mainCharacterHit = Physics2D.Raycast(mousePosition, Vector2.zero, 
                Mathf.Infinity, mainCharacterLayer);
        
            if (hit.collider != null)
            {
                defendable = hit.collider.GetComponentInParent<IDefendable>();
                
                if (defendable != null)
                {
                    return true;
                }

                defendable = hit.collider.GetComponent<IDefendable>();

                if (defendable != null)
                {
                    return true;
                }
            }

            if (mainCharacterHit.collider != null)
            {
                defendable = hit.collider.GetComponentInParent<IDefendable>();
                
                if (defendable != null)
                {
                    return true;
                }

                defendable = hit.collider.GetComponent<IDefendable>();

                if (defendable != null)
                {
                    return true;
                }
            }
            
            return false;
        }
        
        private bool CheckForClickOnDestroyLayer(out IDestroyable destroyable)
        {
            destroyable = null;
            var mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        
            var hit = Physics2D.Raycast(mousePosition, Vector2.zero, 
                Mathf.Infinity, destroyLayer);
        
            if (hit.collider != null)
            {
                destroyable = hit.collider.GetComponentInParent<IDestroyable>();
                
                if (destroyable != null)
                {
                    return true;
                }

                destroyable = hit.collider.GetComponent<IDestroyable>();

                if (destroyable != null)
                {
                    return true;
                }
            }
            
            return false;
        }

        private bool CheckForClickOnInteractable(out IInteractable interactable)
        {
            interactable = null;
            var mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        
            var hit = Physics2D.Raycast(mousePosition, Vector2.zero, 
                Mathf.Infinity, interactableLayer);

            if (hit.collider != null)
            {
                interactable = hit.collider.GetComponentInParent<IInteractable>();
                
                if (interactable != null && Vector2.Distance(transform.position, interactable.GetTransform().position) <= 3f)
                    return true;
            }

            return false;
        }
    }
}
