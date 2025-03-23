using UnityEngine;

public class BetManager : MonoBehaviour
{
    public static BetManager Instance;
    internal double CurrentBet = 0;
    internal int BetIndex = 5;
    internal double Bet = 5.00;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        EventManager.UpdateCurrentBetEvent += UpdateCurrentBet;
        EventManager.UpdateCreditValueIndexEvent += UpdateBetIndex;
    }

    public void UpdateCurrentBet(double bet)
    {
        CurrentBet = bet;
    }

    private void UpdateBetIndex(int betIndex)
    {
        this.BetIndex = betIndex;
    }

    private void OnDisable()
    {
        EventManager.UpdateCurrentBetEvent -= UpdateCurrentBet;
        EventManager.UpdateCreditValueIndexEvent -= UpdateBetIndex;
    }
}
