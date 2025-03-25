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
        EventManager.DisableSlamStopEvent += DisableSlamStopButton;
        EventManager.SetButtonForSpinEvent += SetButtonForStartingSpin;
        EventManager.OnClickResetDataEvent += ResetWinAndDisableSlamStop;
        EventManager.WinAmountEvent += WinAmount;
        EventManager.OnUpdateCurrentBalanceEvent += UpdateBalanceAmount;
    }

    public void SetUIOnStart()
    {
        UpdateCreditValue();
        SetInitialBalance();
    }

    public void UpdateCreditValue()
    {
        landscapeCurrentBet.text = (GameConstants.creditValue[currentBetIndex] * BetManager.Instance.Bet).ToString("F2");
        portraitCurrentBet.text = (GameConstants.creditValue[currentBetIndex] * BetManager.Instance.Bet).ToString("F2");
        EventManager.InvokeUpdateCreditValueIndex(currentBetIndex);
        SetBetButtonInteractivity();
    }

    public void SetBetButtonInteractivity()
    {
        if (currentBetIndex == GameConstants.creditValue.Count - 1)
        {
            landscapeUpBtn.interactable = false;
            portraitUpBtn.interactable = false;
        }
        else
        {
            landscapeUpBtn.interactable = true;
            portraitUpBtn.interactable = true;
        }
        if (currentBetIndex == 0)
        {
            landscapeDownBtn.interactable = false;
            portraitDownBtn.interactable = false;
        }
        else
        {
            landscapeDownBtn.interactable = false;
            portraitDownBtn.interactable = false;
        }
    }

    public void OnClickUp()
    {
        if (currentBetIndex >= GameConstants.creditValue.Count - 1) return;

        currentBetIndex++;
        UpdateCreditValue();
    }

    public void OnClickDown()
    {
        if (currentBetIndex > 0)
        {
            currentBetIndex--;
            UpdateCreditValue();
        }
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

    private void SetButtonForStartingSpin()
    {
        SetButton(false);
    }

    public void ResetWinAndDisableSlamStop() // Contains Button Edge Cases
    {
        landscapeWinAmount.text = "0.00";
        portraitWinAmount.text = "0.00";

        landscapeSlamStopButton.interactable = false;
        portraitSlamStopButton.interactable = false;
    }

    private void WinAmount(double amount)
    {
        landscapeWinAmount.text = (amount).ToString("F2");
        portraitWinAmount.text = (amount).ToString("F2");
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

    private void UpdateBalanceAmount()
    {
        double updatedBalance = RNG.Instance.CurrentBalance;
        landscapeAvailableBalance.text = updatedBalance.ToString("F2");
        portraitAvailableBalance.text = updatedBalance.ToString("F2");
    }

    private void OnNormalSpinComplete()
    {
        StartCoroutine(OnNormalSpinCompleteRoutine());
    }

    private IEnumerator OnNormalSpinCompleteRoutine()
    {
        yield return null;
        SetButton(true);
        SetBetButtonInteractivity();
    }

    public void DisableSlamStopButton()
    {
        landscapeSlamStopButton.interactable = false;
        portraitSlamStopButton.interactable = false;
    }

    private void UnSubscribeEvents()
    {
        EventManager.BalanceAmountDeductionEvent -= BalanceDeduction;
        EventManager.DisableSlamStopEvent -= DisableSlamStopButton;
        EventManager.SetButtonForSpinEvent -= SetButtonForStartingSpin;
        EventManager.OnClickResetDataEvent -= ResetWinAndDisableSlamStop;
        EventManager.WinAmountEvent -= WinAmount;
        EventManager.OnUpdateCurrentBalanceEvent -= UpdateBalanceAmount;

    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }
}
