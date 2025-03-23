using System.Collections;
using UnityEngine;

namespace FSM.GameState
{
    public class ScatterState : BaseState
    {
        public ScatterState(GameStateMachine gamestatemachine) : base(gamestatemachine)
        { }

        public override void Enter()
        {
            GameManager.CurrentState = StateName.Scatter;
            SubscribeEvents();
            InitializeGamestate();
            _gameStateMachine.StartCoroutine(StartFreeSpinWithDelay(2f));
        }
        private void SubscribeEvents()
        {
        }

        private void InitializeGamestate()
        {
        }

        private IEnumerator StartFreeSpinWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            StartFreeSpin();
        }

        private void StartFreeSpin()
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

