namespace FSM.GameState
{
    public class BonusState : BaseState
    {
        public BonusState(GamePlayStateMachine gamestatemachine) : base(gamestatemachine) { }

        public override void Enter()
        {
            GameManager.CurrentState = StateName.Bonus;
            _SubscribeEvents();
        }

        private void _SubscribeEvents()
        {
        }

        public override void Exit()
        {
            UnSubscribeEvents();
        }

        private void UnSubscribeEvents()
        {
        }
    }
}
