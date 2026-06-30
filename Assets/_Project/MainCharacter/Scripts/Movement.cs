using System;
using _Project.Main.Animations;
using _Project.Stats;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.MainCharacter.Scripts
{
    public class Movement : NetworkBehaviour
    {
        public event Action OnMovementPerformed;
        public event Action OnCameraStateChanged;
        public event Action OnSprintPerformed;

        [SerializeField] private AnimationsController animController;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float speed;
        [SerializeField] private float runSpeed;
        [SerializeField] private float staminaDrainRate;
        [SerializeField] private float staminaRegenRate;
        [SerializeField] private float regenDelay;

        private int _cameraStateChangingLockersCount;
        private int _movementLockersCount;
        private int _runningLockersCount;
        private bool _canChangeCameraState;
        private bool _isRunning;
        private Stamina _stamina;
        
        public PlayerInputActions InputActions { get; private set; }
        public float CurrentSpeed { get; private set; }
        public bool CanMove { get; private set; }
        public bool CanSprint { get; private set; }

        public void Initialize(PlayerInputActions inputActions, Stamina stamina)
        {
            UnlockCameraStateChanging();

            CanSprint = true;
            CanMove = true;
            
            InputActions = inputActions;
            CurrentSpeed = speed;
            _stamina = stamina;
            
            InputActions.Player.Sprint.performed += IncreaseCurrentSpeed;
            InputActions.Player.Sprint.canceled += DecreaseCurrentSpeed;

            InputActions.Player.Move.performed += MovementPerformed;
            
            _stamina.OnStaminaChanged += CheckStamina;
        }

        public override void OnDestroy()
        {
            InputActions.Player.Sprint.performed -= IncreaseCurrentSpeed;
            InputActions.Player.Sprint.canceled -= DecreaseCurrentSpeed;
            
            InputActions.Player.Move.performed -= MovementPerformed;
            
            _stamina.OnStaminaChanged -= CheckStamina;
            
            base.OnDestroy();
        }
        
        private void MovementPerformed(InputAction.CallbackContext context)
        {
            OnMovementPerformed?.Invoke();
        }
        
        private void Update()
        {
            if (!IsOwner)
                return;
            
            if (!CanMove)
            {
                animController.UpdateSpeed(0f);
                return;
            }
            
            UpdateAnimation();
        }

        private void FixedUpdate()
        {
            if (!IsOwner)
                return;
            
            if (!CanMove)
                return;
            
            if (_isRunning && InputActions.Player.Move.ReadValue<Vector2>() != Vector2.zero)
            {
                _stamina.DecreaseStamina(staminaDrainRate * Time.deltaTime);
            }
            else if (CanRegenerateStamina())
            {
                _stamina.IncreaseStamina(staminaRegenRate * Time.deltaTime);
            }
            
            rb.linearVelocity = InputActions.Player.Move.ReadValue<Vector2>() * CurrentSpeed;
            
            UpdatePositionServerRpc(transform.position);
        }
        
        [ServerRpc]
        private void UpdatePositionServerRpc(Vector2 position)
        {
            transform.position = position;
        }

        private void UpdateAnimation()
        {
            animController.UpdateSpeed(InputActions.Player.Move.ReadValue<Vector2>().normalized.magnitude);
            
            if (InputActions.Player.Move.ReadValue<Vector2>().x != 0f)
            {
                animController.UpdateDirection(InputActions.Player.Move.ReadValue<Vector2>().x);
            }
        }

        private void CheckStamina()
        {
            if (_stamina.CurrentStamina <= 0)
            {
                DecreaseCurrentSpeed();
            }
        }
        
        public void LockCameraStateChanging()
        {
            _cameraStateChangingLockersCount++;
            _canChangeCameraState = false;
        }
        
        public void UnlockCameraStateChanging()
        {
            _cameraStateChangingLockersCount--;

            if (_cameraStateChangingLockersCount <= 0)
            {
                _cameraStateChangingLockersCount = 0;
                _canChangeCameraState = true;
            }
        }

        public void LockMovement()
        {
            _movementLockersCount++;
            CanMove = false;
            rb.linearVelocity = Vector2.zero;
        }

        public void UnlockMovement()
        {
            _movementLockersCount--;

            if (_movementLockersCount <= 0)
            {
                _movementLockersCount = 0;
                CanMove = true;
            }
        }
        
        public void LockRunning()
        {
            _runningLockersCount++;
            
            if (_isRunning)
                DecreaseCurrentSpeed();
            
            CanSprint = false;
        }

        public void UnlockRunning()
        {
            _runningLockersCount--;

            if (_runningLockersCount <= 0)
            {
                _runningLockersCount = 0;
                CanSprint = true;
            }
        }

        private void IncreaseCurrentSpeed(InputAction.CallbackContext context)
        {
            if (!CanSprint)
                return;
            
            IncreaseCurrentSpeed();
            OnSprintPerformed?.Invoke();
        }
        
        private void DecreaseCurrentSpeed(InputAction.CallbackContext obj)
        {
            if (!CanSprint)
                return;
            
            DecreaseCurrentSpeed();
        }

        private void IncreaseCurrentSpeed()
        {
            if (_stamina.CurrentStamina <= 0)
                return;
            
            if (_isRunning)
                return;
            
            _isRunning = true;
            animController.UpdateRun(_isRunning);
            CurrentSpeed += runSpeed;
        }

        private void DecreaseCurrentSpeed()
        {
            if (!_isRunning)
                return;
            
            _isRunning = false;
            animController.UpdateRun(_isRunning);
            CurrentSpeed -= runSpeed;
        }
        
        private bool CanRegenerateStamina()
        {
            return !_isRunning && 
                   Time.time - _stamina.LastStamineDecTime >= regenDelay &&
                   _stamina.CurrentStamina < _stamina.MaxStamina;
        }
    }
}