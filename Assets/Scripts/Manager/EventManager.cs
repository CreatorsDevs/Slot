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

    // Game Economy Actions
    public static Action<int> UpdateCreditValueIndexEvent;
    public static Action BalanceAmountDeductionEvent;

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
}
