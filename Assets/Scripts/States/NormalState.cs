using System.Collections;
using UnityEngine;

namespace FSM.GameState
{
    public class NormalState : BaseState
    {
        public NormalState(GamePlayStateMachine gamestatemachine) : base(gamestatemachine) { }
        private StateName _nextStateName;
        public override void Enter()
        {
            GameManager.CurrentState = StateName.Normal;
            _nextStateName = StateName.Normal;
            SubscribeEvents();
        }
        private void SubscribeEvents()
        {
            EventManager.SpinButtonClickedEvent += OnSpinClick;
        }

        private void OnSpinClick()
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
            EventManager.SpinButtonClickedEvent += OnSpinClick;
        }
    }
}
