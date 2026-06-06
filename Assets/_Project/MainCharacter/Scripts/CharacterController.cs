using Unity.Netcode;
using UnityEngine;

namespace _Project.MainCharacter.Scripts
{
    public class CharacterController : NetworkBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 360f;
        
        [Header("Visual")]
        [SerializeField] private SpriteRenderer bodyRenderer;
        
        [Header("Animation")]
        [SerializeField] private Animator animator;
        [SerializeField] private string moveAnimParam = "IsMoving";
        
        private Rigidbody2D rb;
        private Vector2 movementInput;
        private Camera mainCamera;
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            mainCamera = Camera.main;
            
            if (bodyRenderer == null)
                bodyRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        
        private void Update()
        {
            if (!IsOwner) return;
            
            HandleInput();
            HandleCamera();
        }
        
        private void FixedUpdate()
        {
            if (!IsOwner) return;
            
            HandleMovement();
        }
        
        private void HandleInput()
        {
            movementInput = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            ).normalized;
        }
        
        private void HandleMovement()
        {
            if (movementInput.magnitude > 0.1f)
            {
                Vector2 newPosition = rb.position + movementInput * moveSpeed * Time.fixedDeltaTime;
                rb.MovePosition(newPosition);
                
                // Анимация
                if (animator != null)
                    animator.SetBool(moveAnimParam, true);
                
                // Отправляем позицию на сервер
                UpdatePositionServerRpc(transform.position);
            }
            else
            {
                // Остановка анимации
                if (animator != null)
                    animator.SetBool(moveAnimParam, false);
            }
        }
        
        private void HandleCamera()
        {
            if (mainCamera != null)
            {
                Vector3 targetPosition = new Vector3(
                    transform.position.x,
                    transform.position.y,
                    mainCamera.transform.position.z
                );
                mainCamera.transform.position = targetPosition;
            }
        }
        
        [ServerRpc]
        private void UpdatePositionServerRpc(Vector2 position)
        {
            transform.position = position;
        }
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            if (IsOwner)
            {
                // Настройка камеры
                if (mainCamera != null)
                {
                    mainCamera.transform.position = new Vector3(
                        transform.position.x,
                        transform.position.y,
                        mainCamera.transform.position.z
                    );
                }
            }
        }
    }
}