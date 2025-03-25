using Data;
using System.Collections;
using UnityEngine;

namespace FSM.GameState
{
    public class NormalState : BaseState
    {
        private bool isSlamStop;

        public NormalState(GamePlayStateMachine gamestatemachine) : base(gamestatemachine) { }
        private StateName nextStateName;
        private bool showPaylineInLoop;
        private double currentSpinTotalWon;
        private const int minScatterForFreeGame = 2;
        private const int minBonusForBonusGame = 2;


        public override void Enter()
        {
            GameManager.CurrentState = StateName.Normal;
            nextStateName = StateName.Normal;
            PaylineController.Instance.SetPayLineState(PayLineState.NotShowing);
            SubscribeEvents();
        }
        private void SubscribeEvents()
        {
            EventManager.SpinResponseEvent += OnSpinDataFetched;
            EventManager.SpinButtonClickedEvent += OnSpinClick;
            EventManager.OnAllReelStopppedEvent += CheckPaylines;
            RNG.Instance.CompleteResponseFetchedEvent += OnCompleteResponseFetched;
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
            AudioManager.Instance._sfxAudioSource.Stop();
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
            bool scatterpayline = HasScatterPayline();
            if (normalpayline || scatterpayline)
            {
                gameStateMachine.StartCoroutine(ShowWinCoroutine(normalpayline, scatterpayline));
            }
            else
            {
                SpinCompleted();
            }
        }
        private void SpinCompleted() => RNG.Instance.SpinCompleted();

        private bool HasNormalPayLine()
        {
            Payline paylineData = RNG.Instance.payline;
            if (RNG.Instance.payline.won > 0)
            {
                PaylineController.Instance.GeneratePayline(paylineData);
                SetTotalAmount(RNG.Instance.payline.won);
                return true;
            }
            else
                return false;
        }

        private bool HasScatterPayline()
        {
            Scatter scatterData = RNG.Instance.scatter;
            if (scatterData.count <= minScatterForFreeGame) return false;

            PaylineController.Instance.scatterPayline = new(scatterData.positions);
            return true;
        }

        private void OnCompleteResponseFetched()
        {
            if (nextStateName == StateName.Normal)
                EventManager.InvokeOnNormalSpinComplete();

            SwitchToNextState();
        }

        private void SwitchToNextState()
        {
            switch (nextStateName)
            {
                case StateName.Scatter:
                    EventManager.InvokeScatterPaylineStopped();
                    break;
                case StateName.Bonus:
                    EventManager.InvokeBonusPaylineStopped();
                    break;
            }
            nextStateName = StateName.Normal;
        }

        private IEnumerator ShowWinCoroutine(bool normalpayline, bool scatterpayline)
        {
            bool hasShownPaylineOnce = false;

            PaylineController.Instance.ReelTint.SetActive(true);
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
            if (scatterpayline)
            {
                nextStateName = StateName.Scatter;
                showPaylineInLoop = false;
                yield return gameStateMachine.StartCoroutine(PaylineController.Instance.ShowScatterPayline());
            }

            SpinCompleted();

            while (showPaylineInLoop)
            {
                PaylineController.Instance.SetPayLineState(PayLineState.Looping);
                yield return gameStateMachine.StartCoroutine(PaylineController.Instance.ShowNormalPayline());
            }
            PaylineController.Instance.SetPayLineState(PayLineState.NotShowing);
            PaylineController.Instance.ReelTint.SetActive(false);
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
            RNG.Instance.CompleteResponseFetchedEvent -= OnCompleteResponseFetched;
        }
    }
}
