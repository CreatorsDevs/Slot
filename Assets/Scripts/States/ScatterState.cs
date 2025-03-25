using Data;
using System.Collections;
using UnityEngine;

namespace FSM.GameState
{
    public class ScatterState : BaseState
    {
        private bool isSlamStop;
        public ScatterState(GamePlayStateMachine gamestatemachine) : base(gamestatemachine)
        { }
        private StateName nextStateName;
        private int remainingFreeSpin;
        private double currntFreeSpinAmount;

        public override void Enter()
        {
            GameManager.CurrentState = StateName.Scatter;
            EventManager.InvokeScatterStateStartedEvent();
            SubscribeEvents();
            InitializeGamestate();
            gameStateMachine.StartCoroutine(StartFreeSpinWithDelay(2f));
        }
        private void SubscribeEvents()
        {
            EventManager.SpinResponseEvent += OnSpinDataFetched;
            EventManager.OnAllReelStopppedEvent += CheckPaylines;
            RNG.Instance.CompleteResponseFetchedEvent += OnCompleteResponseFetched;
        }

        private void InitializeGamestate()
        {
            remainingFreeSpin = RNG.Instance.RemainingFreeSpins();
        }

        private IEnumerator StartFreeSpinWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            StartFreeSpin();
        }

        private void StartFreeSpin()
        {
            RNG.Instance.SpinTheFreeReel(remainingFreeSpin);

            gameStateMachine.StopAllCoroutines();
            ReelManager.Instance.ResetReels();
            PaylineController.Instance.ResetPayLine();
            AudioManager.Instance._sfxAudioSource.Stop();
            gameStateMachine.StartCoroutine(SpinRoutine());
        }
        private IEnumerator SpinRoutine()
        {
            remainingFreeSpin = UpdateFreeSpinCount();
            EventManager.InvokeFreeSpinPlayed(remainingFreeSpin);
            ReelManager.Instance.SpinReels();
            yield return null;
        }

        private int UpdateFreeSpinCount() => --remainingFreeSpin;
        private void CheckPaylines() => gameStateMachine.StartCoroutine(CheckPaylinesRoutine());

        private void OnSpinDataFetched()
        {
            isSlamStop = false;
            ReelManager.Instance.OnSpinDataFetched();
        }


        private IEnumerator CheckPaylinesRoutine()
        {
            yield return null;
            bool normalpayline = HasNormalPayLine();

            if (normalpayline)
                gameStateMachine.StartCoroutine(ShowWinCoroutine(normalpayline));
            else
            {
                OnCompleteResponseFetched();
                yield break;
            }
        }

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

        private IEnumerator ShowWinCoroutine(bool normalPayline)
        {
            bool hasShownPaylineOnce = false;
            PaylineController.Instance.ReelTint.SetActive(true);
            PaylineController.Instance.SetPayLineState(PayLineState.FirstIteration);

            if (normalPayline)
            {
                PaylineController.Instance.ShowTotalWinAmountVisuals(currntFreeSpinAmount);
                hasShownPaylineOnce = true;
                yield return gameStateMachine.StartCoroutine(PaylineController.Instance.ShowNormalPayline());
            }

            if (normalPayline)
                //yield return CelebrationManager.Instance.ShowCelebrationPopupAndWait(currntFreeSpinAmount);

            PaylineController.Instance.SetPayLineState(PayLineState.NotShowing);
            PaylineController.Instance.ReelTint.SetActive(false);

            OnCompleteResponseFetched();
        }

        private void SetTotalAmount(double amount) => currntFreeSpinAmount = amount;
        private void OnCompleteResponseFetched() => gameStateMachine.StartCoroutine(PlayRemainingFreeSpinsRoutine());

        private IEnumerator PlayRemainingFreeSpinsRoutine()
        {
            if (remainingFreeSpin > 0)
            {
                yield return new WaitForSeconds(0.3f);
                StartFreeSpin();
            }
            else
            {
                ScatterGameEnd();
            }
        }

        private void ScatterGameEnd()
        {
            gameStateMachine.SwitchState(gameStateMachine.NormalGameState);
            GameManager.Instance.CurrentGameState = GameManager.GameStatesType.NormalSpin;
        }
        public override void Exit()
        {
            EventManager.InvokeNormalStateStartedEvent();
            UnSubscribeEvents();
        }

        private void UnSubscribeEvents()
        {
            EventManager.SpinResponseEvent -= OnSpinDataFetched;
            EventManager.OnAllReelStopppedEvent -= CheckPaylines;
            RNG.Instance.CompleteResponseFetchedEvent -= OnCompleteResponseFetched;
        }

    }
}

