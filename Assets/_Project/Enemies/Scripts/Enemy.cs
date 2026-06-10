using System;
using System.Collections;
using _Project.Enemies.Scripts.EnemyStates;
using _Project.Levels.Scripts.EnemyLevels;
using _Project.Main.Animations;
using _Project.NPCs.Scripts;
using _Project.WorldClicking.Scripts;
using NavMeshPlus.Extensions;
using UnityEngine;

namespace _Project.Enemies.Scripts
{
    public class Enemy : MonoBehaviour, IAttackable
    {
        public event Action<IAttackable> OnVisibled;
        public event Action<IAttackable> OnHided;
        public event Action<IAttackable> OnKilled;
        public event Action<IAttackable> OnDestroyed;

        [SerializeField] private EnemyType enemyType;
        [SerializeField] private AgentOverride2d navMeshAgent;
        [SerializeField] private EnemyVision enemyVision;
        [SerializeField] private HumanEffects humanEffects;
        [SerializeField] protected EnemyStateMachine enemyStateMachine;
        [SerializeField] protected Vector2 positionOffset;
        [SerializeField] private float retreatSpeed;
        [SerializeField] private float circleSpeed = 3f;
        
        [SerializeField] protected AnimationsController animationsController;
        
        private float _currentHealth;
        private int _currentPointIndex;
        
        protected Coroutine AttackCoroutine;
        protected bool IsDestroyed;
        protected bool Killed;

        public EnemyType EnemyType => enemyType;
        public AgentOverride2d NavMeshAgent => navMeshAgent;
        public EnemyVision EnemyVision => enemyVision;
        public AnimationsController AnimationsController => animationsController;
        public float RetreatSpeed => retreatSpeed;
        public float CircleSpeed => circleSpeed;
        
        public float Health { get; private set; }
        public float DistanceToAttack { get; private set; }
        public float Damage { get; private set; }
        public float AttackCooldownInSec { get; private set; }
        public float ChanceToSpawnSoul { get; private set; }
        public ulong Experience { get; private set; }
        
        public IAttackable TargetAttackable { get; private set; }
        public Transform MainCharacterPosition { get; private set; }
        public bool CanAttack { get; protected set; }
        public EnemyLevelConfig LevelConfig { get; private set; }
        
        public void Initialize(Transform mainCharacterPosition, EnemyLevelConfig enemyLevelConfig, ulong randomLevel)
        {
            CanAttack = true;
            MainCharacterPosition = mainCharacterPosition;
            LevelConfig = enemyLevelConfig;
            
            Initialize(enemyLevelConfig, randomLevel);
            
            enemyStateMachine.ChangeStateByType(EnemyStateType.PlayerChase);
        }

        public void Initialize(EnemyLevelConfig enemyLevelConfig, ulong randomLevel)
        {
            var stats = enemyLevelConfig.GetStatsForLevel(randomLevel);
            var experience = enemyLevelConfig.GetExpForLevel(randomLevel);
            LevelConfig = enemyLevelConfig;
            
            CanAttack = true;
            
            Health = stats.hp;
            _currentHealth = Health;

            DistanceToAttack = stats.attackRange;
            Damage = stats.damage;
            AttackCooldownInSec = stats.attackCooldown;
            ChanceToSpawnSoul = enemyLevelConfig.chanceToSpawnSoul;
            Experience = experience;
            
            enemyStateMachine.Initialize();
            navMeshAgent.Agent.speed = stats.speed;
        }

        public void SelectTargetAttackable(IAttackable target)
        {
            CanAttack = true;
            
            if (target == null)
                return;
            
            TargetAttackable = target;
            TargetAttackable.OnKilled += StopAttacking;
        }

        private void StopAttacking(IAttackable attackable)
        {
            if (TargetAttackable == null)
                return;
            
            TargetAttackable.OnKilled -= StopAttacking;
            TargetAttackable = null;
        }

        private void OnDestroy()
        {
            if (TargetAttackable != null)
            {
                TargetAttackable.OnKilled -= StopAttacking;
            }
        }

        public bool IsVisible()
        {
            return true;
        }

        public void ApplyDamage(float damage)
        {
            _currentHealth -= damage;
            humanEffects.ApplyVisualDamage();

            if (_currentHealth <= 0 && !Killed)
            {
                Kill();
            }
        }

        public void Destroy()
        {
            if (IsDestroyed)
                return;
            
            OnDestroyed?.Invoke(this);
            IsDestroyed = true;
            Destroy(gameObject);
        }

        public void Kill()
        {
            if (Killed)
                return;
            
            navMeshAgent.Agent.isStopped = true;
            Killed = true;
            OnKilled?.Invoke(this);
            animationsController.TriggerAnimation("Death");
        }

        public bool CanApplyDamage()
        {
            return !Killed;
        }

        public void DefaultAttack()
        {
            Attack(Damage);
        }
        
        protected virtual void Attack(float damage)
        {
            if (AttackCoroutine != null)
            {
                CanAttack = true;
                StopCoroutine(AttackCoroutine);
            }
            
            if (Killed || TargetAttackable == null)
            {
                AttackCoroutine = StartCoroutine(AttackCooldown());
                return;
            }

            if (TargetAttackable.CanApplyDamage())
            {
                TargetAttackable.ApplyDamage(damage);
            
                AttackCoroutine = StartCoroutine(AttackCooldown());
            }
        }
        
        protected IEnumerator AttackCooldown()
        {
            CanAttack = false;
            enemyStateMachine.ChangeStateByType(EnemyStateType.Chase);
            
            yield return new WaitForSeconds(AttackCooldownInSec);
            
            CanAttack = true;
        }

        public virtual void DestroyEnemy()
        {
            if (!IsDestroyed)
            {
                OnDestroyed?.Invoke(this);
                IsDestroyed = true;
            }
        }

        public void DestroyAfterCursedSoulAppearance()
        {
            Destroy(gameObject);
        }

        public bool IsKilled() => Killed || IsDestroyed;

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
    }
}