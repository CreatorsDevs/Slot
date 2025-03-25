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
        private double currentSpinTotalWon;
        private const int minScatterForFreeGame = 2;
        private const int minBonusForBonusGame = 2;


        public override void Enter()
        {
            GameManager.CurrentState = StateName.Normal;
            _nextStateName = StateName.Normal;
            PaylineController.Instance.SetPayLineState(PayLineState.NotShowing);
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
            PaylineController.Instance.ResetPayLine();
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
            bool normalpayline = HasNormalPayLine();
            gameStateMachine.StartCoroutine(ShowWinCoroutine(normalpayline));
        }

        private bool HasNormalPayLine()
        {
            int paylineNumber = RNG.Instance.GetPaylineNumber();
            int paylineCount = RNG.Instance.GetPaylineCount();

            if (paylineCount > 0)
            {
                SetTotalAmount(RNG.Instance.GetCurrentSpinTotalWon());
                return true;
            }
            else
                return false;
        }

        private IEnumerator ShowWinCoroutine(bool normalpayline)
        {
            bool hasShownPaylineOnce = false;

            PaylineController.Instance.WinTint.SetActive(true);
            if (PaylineController.Instance.CurrentPayLineState == PayLineState.NotShowing)
            {
                PaylineController.Instance.SetPayLineState(PayLineState.FirstIteration);
            }
            if (normalpayline)
            {
                hasShownPaylineOnce = true;

                PaylineController.Instance.ShowTotalWinAmountVisuals(currentSpinTotalWon);
                yield return gameStateMachine.StartCoroutine(PaylineController.Instance.ShowNormalPayline());
            }

            while (showPaylineInLoop)
            {
                    PaylineController.Instance.SetPayLineState(PayLineState.Looping);
                    yield return gameStateMachine.
                    StartCoroutine(PaylineController.Instance.ShowNormalPayline());
            }
            PaylineController.Instance.SetPayLineState(PayLineState.NotShowing);
            PaylineController.Instance.WinTint.SetActive(false);
        }


        private void SetTotalAmount(double amount) => currentSpinTotalWon = amount;

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
