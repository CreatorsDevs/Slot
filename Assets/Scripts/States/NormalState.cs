using System.Collections;
using UnityEngine;

namespace FSM.GameState
{
    public class NormalState : BaseState
    {
        public NormalState(GameStateMachine gamestatemachine) : base(gamestatemachine) { }
        private StateName _nextStateName;
        public override void Enter()
        {
            GameManager.CurrentState = StateName.Normal;
            _nextStateName = StateName.Normal;
            SubscribeEvents();
        }
        private void SubscribeEvents()
        {
        }
        
        public override void Exit()
        {
            GameManager.IsSlamStop = false;
            ReelManager.Instance.CurrentSpinState = SpinState.Idle;
            UnSubscribeEvents();
        }

        private void UnSubscribeEvents()
        {
        }
    }
}
