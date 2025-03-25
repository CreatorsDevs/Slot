using Data;
using Generics;
using SlotMachineEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Payline = Data.Payline;
using VisualizedPaylines = SlotMachineEngine.Payline;

public class PaylineController : MonoSingleton<PaylineController>
{
    [SerializeField] private PaylineWinning paylineWinning;
    public GameObject ReelTint;
    public VisualizedPaylines normalPayline;
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

    public void GeneratePayline(Payline data)
    {
        var payline = new VisualizedPaylines(data.paylineId, data.symbol, data.positions, data.symbolCount, data.won);
        normalPayline = payline;
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

    public IEnumerator ShowNormalPayline(bool normalpayline, bool scatterpayline)
    {
        AnimatePaylineAndSymbols(normalpayline, scatterpayline);
        yield return WaitTimeForPayLine(CurrentPayLineState);
        StopWinAnimation();
        HidePayline(normalPayline.ID);
    }

    public IEnumerator ShowScatterPayline(bool normalpayline, bool scatterpayline)
    {
        EventManager.InvokeSetButtonForSpin();
        AnimatePaylineAndSymbols(normalpayline, scatterpayline);
        SpecialPayline payline = scatterPayline;
        PlaySymbolSound(payline);
        yield return new WaitForSeconds(ReelManager.Instance.SystemConfig.ShowPaylineDuration);
        ReelTint.SetActive(false);
        StopWinAnimation();
    }

    private WaitForSeconds WaitTimeForPayLine(PayLineState paylinestate)
    {
        if (paylinestate == PayLineState.FirstIteration)
        {
            return new WaitForSeconds(ReelManager.Instance.SystemConfig.ShowFlashPaylineDuration);
        }
        else
        {
            VisualizedPaylines payline = normalPayline;
            PlaySymbolSound(payline);
            return new WaitForSeconds(ReelManager.Instance.SystemConfig.ShowPaylineDuration);
        }
    }

    private static void PlaySymbolSound(VisualizedPaylines payline)
    {
        var symbolPrefabs = ReelManager.Instance.SystemConfig.SymbolPrefabs;
        Symbol payoutSymbol = symbolPrefabs[payline.PAYOUTSYMBOLID];
        AudioManager.Instance.PlaySfx(payoutSymbol._audioClip);
    }

    private static void PlaySymbolSound(SpecialPayline payline)
    {
        var symbolPrefabs = ReelManager.Instance.SystemConfig.SymbolPrefabs;
        var scatterID = ReelManager.Instance.SystemConfig.ScatterId;
        Symbol payoutSymbol = symbolPrefabs[scatterID];
        AudioManager.Instance.PlaySfx(payoutSymbol._audioClip);
    }

    private void AnimatePaylineAndSymbols(bool normalpayline, bool scatterpayline)
    {
        if (normalpayline)
        {
            var payline = normalPayline;

            for (int i = 0; i < payline.SYMBOLSCOUNT; i++)
            {
                Reel reel = ReelManager.Instance.Reels[i];
                reel.OutcomeSymbols[payline.SYMBOLPOSITIONS[i]].ShowWin(CurrentPayLineState);
            }
            ShowPayline(payline.ID);
        }
        else if (scatterpayline) 
        {
            var payline = scatterPayline;
            for (int i = 0; i < payline.SPECIALSYMBOLPOSITIONS.Length; i++)
            {
                Reel reel = ReelManager.Instance.Reels[i];
                reel.OutcomeSymbols[payline.SPECIALSYMBOLPOSITIONS[i]].ShowWin(CurrentPayLineState);
            }
        }
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
