using System;
using _Project.CameraControlling;
using _Project.Main.Animations;
using _Project.MainCharacter.Scripts.Leveling;
using _Project.Stats;
using _Project.Tutorial.ShortTutorial.Scripts;
using _Project.WorldClicking.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace _Project.MainCharacter.Scripts
{
    public class MainCharacterController : MonoBehaviour, IDefendable, IAttackable
    {
        [Inject] private ParametersConfig _parametersConfig;
        [Inject] private MainCharacterData _mainCharacterData;
        
        public event Action<IAttackable> OnVisibled;
        public event Action<IAttackable> OnHided;
        public event Action<IAttackable> OnKilled;
        public event Action<IAttackable> OnDestroyed;
        
        [SerializeField] private AnimationsController animationsController;
        [SerializeField] private Movement movement;
        [SerializeField] private CameraController cameraController;
        [FormerlySerializedAs("undeadSquadsController")] [SerializeField] private WorldClickingController worldClickingController;
        [SerializeField] private ShortGameTypeTutorial shortGameTypeTutorial;
        [Space] 
        [SerializeField] private float healthRegenDelay;
        [SerializeField] private float healthRegenRate;
        [SerializeField] private float diseaseRegenDelay;
        [SerializeField] private float diseaseRegenRate;
        [Space]
        [SerializeField] protected Vector2 positionOffset;
        [Header("Colliders and rigidbody")] 
        [SerializeField] private Collider2D physicsCollider;
        [SerializeField] private Collider2D triggerCollider;
        [SerializeField] private Rigidbody2D rb;

        private bool _isImmortal;
        
        public SpriteRenderer SpriteRenderer { get; private set; }
        public PlayerInputActions InputActions { get; private set; }
        public Health Health { get; private set; }
        public Stamina Stamina { get; private set; }
        
        public Movement Movement => movement;
        public AnimationsController AnimationsController => animationsController;
        public Collider2D PhysicsCollider => physicsCollider;
        public Collider2D TriggerCollider => triggerCollider;
        public Rigidbody2D Rb => rb;
        public ShortGameTypeTutorial ShortGameTypeTutorial => shortGameTypeTutorial;

        private void Awake()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();

            var maxHealth = _parametersConfig.healthConfig.GetHealthForLevel(_mainCharacterData.CurrentHealthLevel);
            var maxStamina = _parametersConfig.staminaConfig.GetStaminaForLevel(_mainCharacterData.CurrentStaminaLevel);
            
            Health = new Health(maxHealth);
            Health.OnZeroHealth += OnZeroHealth;
            
            Stamina = new Stamina(maxStamina);
            
            InputActions = new PlayerInputActions();
            InputActions.Enable();
            
            movement.Initialize(InputActions, Stamina);
            worldClickingController.Initialize();
        }

        private void Start()
        {
            
        }

        private void OnDestroy()
        {
            InputActions.Disable();
            Health.OnZeroHealth -= OnZeroHealth;
        }

        public void LockRunning()
        {
            movement.LockRunning();
        }

        public void UnlockRunning()
        {
            movement.UnlockRunning();
        }

        private void OnZeroHealth()
        {
            OnKilled?.Invoke(this);
        }

        public void UsePotionAnimation()
        {
            animationsController.TriggerAnimation("Drink");
            LockRunning();
        }

        public void DrinkAnimationEnded()
        {
            UnlockRunning();
        }

        public bool IsVisible()
        {
            return true;
        }

        public void ApplyDamage(float damage)
        {
            Health.Damage(damage);
        }
        
        public bool CanApplyDamage()
        {
            return Health.CurrentHealth > 0 && !_isImmortal;
        }

        public void EnableImmortality()
        {
            _isImmortal = true;
        }

        public void DisableImmortality()
        {
            _isImmortal = false;
        }

        public void Revive()
        {
            transform.position = Vector2.zero;
            
            var maxHealth = _parametersConfig.healthConfig.GetHealthForLevel(_mainCharacterData.CurrentHealthLevel);
            var maxStamina = _parametersConfig.staminaConfig.GetStaminaForLevel(_mainCharacterData.CurrentStaminaLevel);
            
            Health.OnZeroHealth -= OnZeroHealth;
            Health = new Health(maxHealth);
            Health.OnZeroHealth += OnZeroHealth;
            
            Stamina = new Stamina(maxStamina);
            
            movement.UnlockMovement();
        }

        public Vector2 GetPosition()
        {
            return transform.position;
        }
        
        public Vector2 GetPositionWithOffset()
        {
            return (Vector2)transform.position + positionOffset;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
        
        public bool IsKilled() => Health.CurrentHealth <= 0;

        private void FixedUpdate()
        {
            if (CanRegenerateHealth())
            {
                Health.Heal(healthRegenRate * Time.deltaTime);
            }

            if (CanHealDisease())
            {
                Health.HealDisease(diseaseRegenRate * Time.deltaTime);
            }
        }

        private bool CanRegenerateHealth()
        {
            return Time.time - Health.LastHealthDecTime >= healthRegenDelay &&
                   Health.CurrentHealth < Health.MaxHealth - Health.CurrentDiseaseLevel;
        }

        private bool CanHealDisease()
        {
            return Time.time - Health.LastDiseaseTime >= diseaseRegenDelay &&
                   Health.CurrentDiseaseLevel > 0;
        }
    }
}
