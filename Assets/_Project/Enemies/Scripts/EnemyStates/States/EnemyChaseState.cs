using _Project.Main.StateMachine;
using _Project.WorldClicking.Scripts;
using UnityEngine;

namespace _Project.Enemies.Scripts.EnemyStates.States
{
    public class EnemyChaseState : IState
    {
        protected EnemyStateMachine EnemyStateMachine;
        private Coroutine _coroutine;

        private bool _isAttackEnded;

        protected Enemy Enemy => EnemyStateMachine.Enemy;

        public EnemyChaseState(EnemyStateMachine enemyStateMachine)
        {
            EnemyStateMachine = enemyStateMachine;
        }
        
        public void OnEnterState()
        {
            if (Enemy.IsKilled())
                return;

            if (Enemy.TargetAttackable == null)
            {
                CheckAnotherAttackable(Enemy.TargetAttackable);
                return;
            }

            Enemy.EnemyVision.OnAttackableDetected += CheckNearestAttackable;
            Enemy.EnemyVision.OnAttackableLost += CheckAnotherAttackable;
            Enemy.TargetAttackable.OnKilled += CheckAnotherAttackable;
            
            Enemy.NavMeshAgent.Agent.destination = Enemy.TargetAttackable.GetPosition();
            Enemy.NavMeshAgent.Agent.isStopped = false;
        }

        public void OnExitState()
        {
            Enemy.EnemyVision.OnAttackableDetected -= CheckNearestAttackable;
            Enemy.EnemyVision.OnAttackableLost -= CheckAnotherAttackable;
            
            if (Enemy.TargetAttackable != null)
                Enemy.TargetAttackable.OnKilled -= CheckAnotherAttackable;
            
            if (Enemy.NavMeshAgent.Agent.isOnNavMesh)
                Enemy.NavMeshAgent.Agent.isStopped = true;
        }
        
        private void CheckNearestAttackable(IAttackable attackable)
        {
            if (Enemy.TargetAttackable != Enemy.EnemyVision.GetNearestAttackable())
            {
                Enemy.SelectTargetAttackable(Enemy.EnemyVision.GetNearestAttackable());
                Enemy.NavMeshAgent.Agent.destination = Enemy.TargetAttackable.GetPosition();
            }
        }

        protected virtual void CheckAnotherAttackable(IAttackable attackable)
        {
            if (EnemyStateMachine == null 
                || EnemyStateMachine.Enemy == null 
                || EnemyStateMachine.Enemy.AnimationsController == null)
                return;
            
            if (!Enemy.EnemyVision.HasAttackable)
            {
                if (Enemy.MainCharacterPosition != null)
                {
                    Enemy.AnimationsController.TriggerAnimation("AttackEnded");
                    EnemyStateMachine.ChangeStateByType(EnemyStateType.PlayerChase);
                }
                else
                {
                    Enemy.AnimationsController.TriggerAnimation("AttackEnded");
                    EnemyStateMachine.ChangeStateByType(EnemyStateType.Idle);
                }
                
                return;
            }
            
            Enemy.SelectTargetAttackable(Enemy.EnemyVision.GetNearestAttackable());
        }
        
        public void Execute()
        {
            if (Enemy.TargetAttackable == null || Enemy.TargetAttackable.IsKilled())
            {
                EnemyStateMachine.Enemy.AnimationsController.TriggerAnimation("AttackEnded");
                CheckNearestAttackable(null);
                return;
            }
            
            Enemy.NavMeshAgent.Agent.destination = Enemy.TargetAttackable.GetPosition();
            
            if (Vector2.Distance(Enemy.transform.position, Enemy.TargetAttackable.GetPosition()) <= Enemy.DistanceToAttack)
            {
                if (!Enemy.CanAttack)
                {
                    Enemy.NavMeshAgent.Agent.isStopped = true;
                    return;
                }

                _isAttackEnded = false;
                
                EnemyStateMachine.ChangeStateByType(EnemyStateType.Attack);
            }
            else
            {
                if (!Enemy.CanAttack && Vector2.Distance(Enemy.transform.position, Enemy.TargetAttackable.GetPosition()) > Enemy.DistanceToAttack + 1)
                {
                    Debug.Log("Хватет атаковать");
                    
                    if (!_isAttackEnded)
                    {
                        _isAttackEnded = true;
                        PerformAttackEnded();
                    }
                }
                
                Enemy.NavMeshAgent.Agent.isStopped = false;
            }
        }

        protected virtual void PerformAttackEnded()
        {
            EnemyStateMachine.Enemy.AnimationsController.TriggerAnimation("AttackEnded");
        }

        public void FixedExecute() { }
    }
}