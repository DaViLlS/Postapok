namespace _Project.Main.StateMachine
{
    public interface IState
    {
        public void Execute();
        public void FixedExecute();
        public void OnEnterState();
        public void OnExitState();
    }
}
