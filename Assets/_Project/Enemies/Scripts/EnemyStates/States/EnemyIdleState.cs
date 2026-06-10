using _Project.Main.StateMachine;
using _Project.WorldClicking.Scripts;

namespace _Project.Enemies.Scripts.EnemyStates.States
{
    public class EnemyIdleState : IState
    {
        protected EnemyStateMachine EnemyStateMachine;
        
        public Enemy Enemy => EnemyStateMachine.Enemy;

        public EnemyIdleState(EnemyStateMachine enemyStateMachine)
        {
            EnemyStateMachine = enemyStateMachine;
        }
        
        public void OnEnterState()
        {
            Enemy.EnemyVision.OnAttackableDetected += OnCharacterDetected;
        }
        
        public void OnExitState()
        {
            Enemy.EnemyVision.OnAttackableDetected -= OnCharacterDetected;
        }

        protected virtual void OnCharacterDetected(IAttackable attackable)
        {
            Enemy.SelectTargetAttackable(attackable);
            EnemyStateMachine.ChangeStateByType(EnemyStateType.Chase);
        }
        
        public void Execute() { }

        public void FixedExecute() { }
    }
}