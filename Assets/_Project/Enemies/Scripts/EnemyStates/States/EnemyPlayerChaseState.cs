using _Project.Main.StateMachine;
using _Project.WorldClicking.Scripts;
using UnityEngine;

namespace _Project.Enemies.Scripts.EnemyStates.States
{
    public class EnemyPlayerChaseState : IState
    {
        private EnemyStateMachine _enemyStateMachine;
        private Coroutine _coroutine;

        private Enemy Enemy => _enemyStateMachine.Enemy;

        public EnemyPlayerChaseState(EnemyStateMachine enemyStateMachine)
        {
            _enemyStateMachine = enemyStateMachine;
        }
        
        public void OnEnterState()
        {
            if (Enemy.IsKilled())
                return;
            
            Enemy.EnemyVision.OnAttackableDetected += OnCharacterDetected;
            
            Enemy.NavMeshAgent.Agent.destination = Enemy.MainCharacterPosition.position;
            Enemy.NavMeshAgent.Agent.isStopped = false;
        }

        public void OnExitState()
        {
            Enemy.EnemyVision.OnAttackableDetected -= OnCharacterDetected;
            
            if (Enemy.NavMeshAgent.Agent.isOnNavMesh)
                Enemy.NavMeshAgent.Agent.isStopped = true;
        }
        
        protected virtual void OnCharacterDetected(IAttackable attackable)
        {
            Enemy.SelectTargetAttackable(attackable);
            _enemyStateMachine.ChangeStateByType(EnemyStateType.Chase);
        }
        
        public void Execute()
        {
            if (Enemy.IsKilled())
                return;
            
            Enemy.NavMeshAgent.Agent.destination = Enemy.MainCharacterPosition.position;
            
            if (Vector2.Distance(Enemy.transform.position, Enemy.MainCharacterPosition.position) <= Enemy.DistanceToAttack)
            {
                _enemyStateMachine.ChangeStateByType(EnemyStateType.Idle);
            }
        }

        public void FixedExecute() { }
    }
}