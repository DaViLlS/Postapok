using _Project.Main.StateMachine;
using _Project.WorldClicking.Scripts;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Enemies.Scripts.EnemyStates.States
{
    public class EnemyAttackState : IState
    {
        protected EnemyStateMachine EnemyStateMachine;
        private Coroutine _coroutine;
        private bool _isAttacking;
        private bool _isPaused;
        private bool _isRetreating;

        protected Enemy Enemy => EnemyStateMachine.Enemy;

        public EnemyAttackState(EnemyStateMachine enemyStateMachine)
        {
            EnemyStateMachine = enemyStateMachine;
        }
        
        public void OnEnterState()
        {
            if (!Enemy.EnemyVision.HasAttackable)
            {
                if (Enemy.MainCharacterPosition != null)
                {
                    Debug.Log("Attack ended");
                    Enemy.AnimationsController.TriggerAnimation("AttackEnded");
                    EnemyStateMachine.ChangeStateByType(EnemyStateType.PlayerChase);
                }
                else
                {
                    Debug.Log("Attack ended");
                    Enemy.AnimationsController.TriggerAnimation("AttackEnded");
                    EnemyStateMachine.ChangeStateByType(EnemyStateType.Idle);
                }
                return;
            }
            
            if (Enemy.TargetAttackable == null)
            {
                CheckAnotherAttackable(Enemy.TargetAttackable);
                return;
            }
            
            Enemy.EnemyVision.OnAttackableLost += CheckAnotherAttackable;
            Enemy.TargetAttackable.OnKilled += CheckAnotherAttackable;
            
            Enemy.NavMeshAgent.Agent.isStopped = true;

            AttackAnimation();
        }

        public void OnExitState()
        {
            Enemy.EnemyVision.OnAttackableLost -= CheckAnotherAttackable;
            
            if (Enemy.TargetAttackable != null)
                Enemy.TargetAttackable.OnKilled -= CheckAnotherAttackable;
            
            if (_coroutine != null)
                EnemyStateMachine.StopCoroutine(_coroutine);
        }
        
        protected virtual void CheckAnotherAttackable(IAttackable attackable)
        {
            if (Enemy.IsKilled())
                return;
            
            if (!Enemy.EnemyVision.HasAttackable)
            {
                Enemy.AnimationsController.TriggerAnimation("AttackEnded");
                
                if (Enemy.MainCharacterPosition != null)
                {
                    EnemyStateMachine.ChangeStateByType(EnemyStateType.PlayerChase);
                }
                else
                {
                    EnemyStateMachine.ChangeStateByType(EnemyStateType.Idle);
                }
                
                return;
            }
            
            Enemy.SelectTargetAttackable(Enemy.EnemyVision.GetNearestAttackable());
            
            EnemyStateMachine.ChangeStateByType(EnemyStateType.Chase);
        }
        
        protected virtual void AttackAnimation()
        {
            if (Enemy.IsKilled() || Enemy.TargetAttackable == null || !Enemy.TargetAttackable.CanApplyDamage())
                return;
            
            Enemy.AnimationsController.ResetTriggerAnimation("AttackEnded");
            Enemy.AnimationsController.TriggerAnimation("Attack");
        }

        public void Execute()
        {
            if (Enemy.IsKilled())
                return;
            
            if (Enemy.TargetAttackable != null)
            {
                var distanceToTarget = Vector2.Distance(Enemy.transform.position, Enemy.TargetAttackable.GetPosition());
        
                if (distanceToTarget < Enemy.LevelConfig.baseRetreatRange)
                {
                    if (!_isRetreating || Enemy.NavMeshAgent.Agent.remainingDistance < 0.5f)
                    {
                        FindRetreatPosition();
                    }
                    
                    return;
                }
                
                if (distanceToTarget > Enemy.LevelConfig.baseAttackRange)
                {
                    MoveToOptimalRange();
                    
                    return;
                }
            }
            
            _isRetreating = false;
        }
        
        private void FindRetreatPosition()
        {
            _isRetreating = true;
            Enemy.NavMeshAgent.Agent.speed = Enemy.RetreatSpeed;
            
            var directionFromTarget = ((Vector2)Enemy.transform.position - Enemy.TargetAttackable.GetPosition()).normalized;
            
            var idealRetreatPoint = (Vector2)Enemy.transform.position + directionFromTarget * Enemy.LevelConfig.baseRetreatRange;
            
            NavMeshHit hit;
            
            if (NavMesh.SamplePosition(idealRetreatPoint, out hit, Enemy.LevelConfig.baseAttackRange, NavMesh.AllAreas))
            {
                Enemy.NavMeshAgent.Agent.SetDestination(hit.position);
            }
            else
            {
                var randomDirection = directionFromTarget + Random.insideUnitCircle * 2f;
                var fallbackPoint = (Vector2)Enemy.transform.position + randomDirection * Enemy.LevelConfig.baseAttackRange;
                
                if (NavMesh.SamplePosition(fallbackPoint, out hit, Enemy.LevelConfig.baseAttackRange, NavMesh.AllAreas))
                {
                    Enemy.NavMeshAgent.Agent.SetDestination(hit.position);
                }
            }
        }
        
        private void MoveToOptimalRange()
        {
            _isRetreating = false;
            Enemy.NavMeshAgent.Agent.speed = Enemy.CircleSpeed;
            
            var directionToTarget = (Enemy.TargetAttackable.GetPosition() - (Vector2)Enemy.transform.position).normalized;
            var optimalPosition = Enemy.TargetAttackable.GetPosition() - directionToTarget * Enemy.LevelConfig.baseAttackRange;
            
            NavMeshHit hit;
            
            if (NavMesh.SamplePosition(optimalPosition, out hit, 2f, NavMesh.AllAreas))
            {
                Enemy.NavMeshAgent.Agent.SetDestination(hit.position);
            }
        }

        public void FixedExecute() { }
    }
}