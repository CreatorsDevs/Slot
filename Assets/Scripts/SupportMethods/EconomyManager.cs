using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static void OnUpdateCurrentBalance() => EventManager.InvokeUpdateBalanceAmount();

    public static bool HasSufficientbalance()
    {
        double currentBalance = RNG.Instance.CurrentBalance;
        return currentBalance >= GameConstants.creditValue[BetManager.Instance.BetIndex] * BetManager.Instance.Bet;
    }
}
