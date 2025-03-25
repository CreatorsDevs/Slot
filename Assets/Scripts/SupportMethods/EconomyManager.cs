using System;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static void OnUpdateCurrentBalance() => EventManager.InvokeUpdateBalanceAmount();

    public static bool HasSufficientbalance()
    {
        double currentBalance = RNG.Instance.CurrentBalance;
        return currentBalance >= GameConstants.creditValue[BetManager.Instance.BetIndex] * BetManager.Instance.Bet;
    }

    public static float GetTruncatedTwoDecimalFloatAmount(double amt)
    {
        return (float)(Math.Truncate(amt * 100) / 100);
    }
}
