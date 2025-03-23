using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Landscape UI")]
    #region LandscapeUI
    [SerializeField] private TextMeshProUGUI landscapeAvailableBalance;
    [SerializeField] private TextMeshProUGUI landscapeWinAmount;
    [SerializeField] private TextMeshProUGUI landscapeCurrentBet;
    [SerializeField] private Button landscapeUpBtn;
    [SerializeField] private Button landscapeDownBtn;
    [SerializeField] private Button landscapeInfoBtn;
    [SerializeField] private Button landscapeSpinButton;
    [SerializeField] private Button landscapeSlamStopButton;
    #endregion

    [Header("Portrait UI")]
    #region PortraitUI
    [SerializeField] private TextMeshProUGUI portraitAvailableBalance;
    [SerializeField] private TextMeshProUGUI portraitWinAmount;
    [SerializeField] private TextMeshProUGUI portraitCurrentBet;
    [SerializeField] private Button portraitUpBtn;
    [SerializeField] private Button portraitDownBtn;
    [SerializeField] private Button portraitInfoBtn;
    [SerializeField] private Button portraitSpinButton;
    [SerializeField] private Button portraitSlamStopButton;
    #endregion

    private int currentBetIndex;
    private decimal currentAvailableBalance;
    private decimal currentBet;
    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void Awake()
    {
        currentBetIndex = BetManager.Instance.BetIndex;
    }

    void Start()
    {
        
    }

    private void SubscribeEvents()
    {
        EventManager.BalanceAmountDeductionEvent += BalanceDeduction;
    }

    public void SetInitialBalance()
    {
        double balance = GameManager.Instance.rng.CurrentBalance;
        landscapeAvailableBalance.text = balance.ToString("F2");
        portraitAvailableBalance.text = balance.ToString("F2");
        EventManager.InvokeCurrentBetUpdateFunctionality(GameConstants.creditValue[currentBetIndex] * BetManager.Instance.Bet);
    }

    public void OnSpinButtonClicked()
    {
        if (EconomyManager.HasSufficientbalance())
        {
            ResetWinAndDisableSlamStop();
            EventManager.InvokeOnSpinClicked();
            SetButton(false);
        }
        else
        {
            Debug.LogError("LOW BALANCE!");
        }
    }

    private void SetButton(bool enable)
    {
        landscapeSlamStopButton.interactable = enable;
        portraitSlamStopButton.interactable = enable;
        
        landscapeSpinButton.gameObject.SetActive(enable);
        portraitSpinButton.gameObject.SetActive(enable);
        
        landscapeInfoBtn.interactable = enable;
        portraitInfoBtn.interactable = enable;
        
        landscapeUpBtn.interactable = enable;
        portraitUpBtn.interactable = enable;
        
        landscapeDownBtn.interactable = enable;
        portraitDownBtn.interactable = enable;
    }

    public void ResetWinAndDisableSlamStop() // Contains Button Edge Cases
    {
        landscapeWinAmount.text = "0.00";
        portraitWinAmount.text = "0.00";

        landscapeSlamStopButton.interactable = false;
        portraitSlamStopButton.interactable = false;
    }

    private void BalanceDeduction()
    {
        if (GameManager.Instance.CurrentGameState == GameManager.GameStatesType.FreeGame) return;
        
        EventManager.InvokeCurrentBetUpdateFunctionality(GameConstants.creditValue[currentBetIndex] * BetManager.Instance.Bet);
        
        currentAvailableBalance = decimal.Parse(landscapeAvailableBalance.text);
        currentBet = decimal.Parse(landscapeCurrentBet.text);
        decimal balance = currentAvailableBalance - currentBet;

        landscapeAvailableBalance.text = balance.ToString("F2");
        portraitAvailableBalance.text = balance.ToString("F2");
    }

    private void UnSubscribeEvents()
    {
        EventManager.BalanceAmountDeductionEvent += BalanceDeduction;
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }
}
