using System.Collections.Generic;
using _Project.Enemies.Scripts.EnemyStates.States;
using _Project.Main.StateMachine;
using _Project.WorldClicking.Scripts;
using UnityEngine;

namespace _Project.Enemies.Scripts.EnemyStates
{
    public class EnemyStateMachine : StateMachine
    {
        [SerializeField] private Enemy enemy;

        public Enemy Enemy => enemy;
        
        protected Dictionary<EnemyStateType, IState> StateHandlers;
        
        public EnemyStateType CurrentStateType { get; private set; }
        
        public override void Initialize()
        {
            enemy.OnKilled += StopHandlingStates;
            enemy.OnDestroyed += StopHandlingStates;
            
            StateHandlers = new Dictionary<EnemyStateType, IState>()
            {
                { EnemyStateType.Idle, new EnemyIdleState(this) },
                { EnemyStateType.Chase, new EnemyChaseState(this) },
                { EnemyStateType.Attack, new EnemyAttackState(this) },
                { EnemyStateType.PlayerChase, new EnemyPlayerChaseState(this) },
            };

            ChangeStateByType(EnemyStateType.Idle);
        }

        protected virtual void OnDestroy()
        {
            Dispose();
            enemy.OnKilled -= StopHandlingStates;
            enemy.OnDestroyed -= StopHandlingStates;
        }

        protected void StopHandlingStates(IAttackable attackable)
        {
            Dispose();
        }

        public void ChangeStateByType(EnemyStateType enemyStateType)
        {
            CurrentStateType = enemyStateType;
            ChangeState(StateHandlers[enemyStateType]);
        }
    }
}