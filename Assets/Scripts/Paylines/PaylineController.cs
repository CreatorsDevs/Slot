using Generics;
using SlotMachineEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PaylineController : MonoSingleton<PaylineController>
{
    [SerializeField] private PaylineWinning paylineWinning;
    public GameObject ReelTint;
    public Payline Payline;
    public SpecialPayline scatterPayline;
    public SpecialPayline bonusPayline;

    private PayLineState state;
    public PayLineState CurrentPayLineState
    {
        get { return state; }
    }

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    private void Init()
    {
        state = PayLineState.NotShowing;
    }

    public void SetPayLineState(PayLineState state) => this.state = state;

    public void ResetPayLine()
    {
        StopWinAnimation();
        SetPayLineState(PayLineState.NotShowing);
        ReelTint.SetActive(false);
        paylineWinning.Reset();
        StopAllCoroutines();
    }

    public void ShowTotalWinAmountVisuals(double currentspincreditwon)
    {
        if (CurrentPayLineState == PayLineState.FirstIteration)
        {
            ShowTotalWinPopup(currentspincreditwon);
            Invoke(nameof(UpdateBalance), 1.5f);
        }
    }

    private void UpdateBalance() => EventManager.InvokeWinAmount(RNG.Instance.payline.won);

    public void ShowTotalWinPopup(double winamount) => paylineWinning.ShowTotalWin(winamount);

    public IEnumerator ShowNormalPayline()
    {
        AnimatePaylineAndSymbols();
        yield return WaitTimeForPayLine(CurrentPayLineState);
        StopWinAnimation();
        HidePayline(Payline.ID);
    }

    private WaitForSeconds WaitTimeForPayLine(PayLineState paylinestate)
    {
        if (paylinestate == PayLineState.FirstIteration)
        {
            return new WaitForSeconds(ReelManager.Instance.SystemConfig.ShowFlashPaylineDuration);
        }
        else
        {
            Payline payline = Payline;
            PlaySymbolSound(payline);
            return new WaitForSeconds(ReelManager.Instance.SystemConfig.ShowPaylineDuration);
        }
    }

    private static void PlaySymbolSound(Payline payline)
    {
        var symbolPrefabs = ReelManager.Instance.SystemConfig.SymbolPrefabs;
        Symbol payoutSymbol = symbolPrefabs[payline.PAYOUTSYMBOLID];
        //Audiomanager.Instance.PlaySfx(payoutSymbol._audioClip);
    }

    private void AnimatePaylineAndSymbols()
    {
        var payline = Payline;

        for (int i = 0; i < payline.SYMBOLSCOUNT; i++)
        {
            Reel reel = ReelManager.Instance.Reels[i];
            reel.OutcomeSymbols[payline.SYMBOLPOSITIONS[i]].ShowWin(CurrentPayLineState);
        }

        ShowPayline(payline.ID);
    }

    public void ShowPayline(int paylineid)
    {
        paylineWinning.ShowPayline(paylineid);
    }

    public void HidePayline(int paylineid)
    {
        paylineWinning.HidePayline(paylineid);
    }

    public void StopWinAnimation()
    {
        foreach (Symbol symbol in ReelManager.Instance.Reels.SelectMany(reel => reel.OutcomeSymbols))
        {
            symbol.HideWin();
        }
    }
}
