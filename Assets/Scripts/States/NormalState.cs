using System.Collections;
using UnityEngine;

namespace FSM.GameState
{
    public class NormalState : BaseState
    {
        private bool isSlamStop;

        public NormalState(GamePlayStateMachine gamestatemachine) : base(gamestatemachine) { }
        private StateName _nextStateName;
        private bool showPaylineInLoop;

        public override void Enter()
        {
            GameManager.CurrentState = StateName.Normal;
            _nextStateName = StateName.Normal;
            SubscribeEvents();
        }
        private void SubscribeEvents()
        {
            EventManager.SpinResponseEvent += OnSpinDataFetched;
            EventManager.SpinButtonClickedEvent += OnSpinClick;
            EventManager.OnAllReelStopppedEvent += CheckPaylines;
        }

        private void OnSpinClick()
        {
            isSlamStop = true;

            EventManager.InvokeSetButtonForSpin();
            GameManager.Instance.ResetSlamStop();
            showPaylineInLoop = ReelManager.Instance.SystemConfig.ShowPaylinesInLoop;
            gameStateMachine.StopAllCoroutines();
            ReelManager.Instance.ResetReels();
            EventManager.InvokeOnClickResetData();
            ReelManager.Instance.SpinReels();
        }

        private void OnSpinDataFetched()
        {
            isSlamStop = false;
            ReelManager.Instance.OnSpinDataFetched();
        }

        private void CheckPaylines() => gameStateMachine.StartCoroutine(CheckPaylinesRoutine());

        private IEnumerator CheckPaylinesRoutine()
        {
            yield return null;
        }
        public override void Exit()
        {
            GameManager.IsSlamStop = false;
            ReelManager.Instance.CurrentSpinState = SpinState.Idle;
            UnSubscribeEvents();
        }

        private void UnSubscribeEvents()
        {
            EventManager.SpinResponseEvent -= OnSpinDataFetched;
            EventManager.SpinButtonClickedEvent -= OnSpinClick;
            EventManager.OnAllReelStopppedEvent -= CheckPaylines;
        }
    }
}
