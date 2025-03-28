using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

public class EventManager
{

    #region ACTIONS
    // Camera Actions
    public static Action<int, int> OnOrientationChangeEvent;

    //General Actions
    public static Action<double> UpdateCurrentBetEvent;
    public static Action OnSpinClickedEvent;
    public static Action SpinButtonClickedEvent;
    public static Action DisableSlamStopEvent;
    public static Action OnAllReelStopppedEvent;
    public static Action SpinResponseEvent;
    public static Action SetButtonForSpinEvent;
    public static Action OnClickResetDataEvent;
    public static Action OnUpdateCurrentBalanceEvent;
    public static Action OnNormalSpinCompleteEvent;
    public static Action OnScatterPaylineStopped;
    public static Action OnBonusPaylineStopped;
    public static Action ScatterStateStartedEvent;
    public static Action NormalStateStartedEvent;
    public static Action ShowFreeSpinIntro;
    public static Action CloseFreeSpinIntro;
    public static Action ShowFreeSpinOutro;
    public static Action CloseFreeSpinOutro;
    public static Action<int> OnFreeSpinPlayed;

    // Game Economy Actions
    public static Action<int> UpdateCreditValueIndexEvent;
    public static Action BalanceAmountDeductionEvent;
    public static Action<double> WinAmountEvent;


    #endregion

    public static void InvokeOrientationChange(int x, int y)
    {
        OnOrientationChangeEvent(x, y);
    }

    public static void InvokeCurrentBetUpdateFunctionality(double current_Bet)
    {
        UpdateCurrentBetEvent(current_Bet);
    }

    public static void InvokeUpdateCreditValueIndex(int index)
    {
        UpdateCreditValueIndexEvent(index);
    }
    public static void InvokeOnSpinClicked()
    {
        OnSpinClickedEvent();
    }

    public static void InvokeSpinButton()
    {
        SpinButtonClickedEvent?.Invoke();
    }

    public static void InvokeUpdateBalanceAmount()
    {
        BalanceAmountDeductionEvent();
    }

    public static void InvokeDisableSlamStop()
    {
        DisableSlamStopEvent?.Invoke();
    }
    public static void InvokeOnAllReelStoppped() => OnAllReelStopppedEvent?.Invoke();

    public static void InvokeSpinResponse()
    {
        SpinResponseEvent();
    }

    public static void InvokeSetButtonForSpin()
    {
        SetButtonForSpinEvent?.Invoke();
    }

    public static void InvokeOnClickResetData()
    {
        OnClickResetDataEvent();
    }

    public static void InvokeWinAmount(double win)
    {
        WinAmountEvent(win);
    }

    public static void InvokeUpdateBalance()
    {
        OnUpdateCurrentBalanceEvent();
    }

    public static void InvokeOnNormalSpinComplete()
    {
        OnNormalSpinCompleteEvent();
    }

    public static void InvokeScatterPaylineStopped()
    {
        OnScatterPaylineStopped();
    }

    public static void InvokeBonusPaylineStopped()
    {
        OnBonusPaylineStopped();
    }

    public static void InvokeFreeSpinPlayed(int currentFreeSpinCount)
    {
        OnFreeSpinPlayed(currentFreeSpinCount);
    }

    public static void InvokeScatterStateStartedEvent() => ScatterStateStartedEvent?.Invoke();
    public static void InvokeNormalStateStartedEvent() => NormalStateStartedEvent?.Invoke();

    public static void InvokeFreeSpinIntroPopUp() => ShowFreeSpinIntro?.Invoke();
    public static void CloseFreeSpinIntroPopUp() => CloseFreeSpinIntro?.Invoke();
    public static void InvokeFreeSpinOutroPopUp() => ShowFreeSpinOutro?.Invoke();
    public static void CloseFreeSpinOutroPopUp() => CloseFreeSpinOutro?.Invoke();
}
