using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Generics;

public class ReelManager : MonoSingleton<ReelManager>
{
    [field: SerializeField] public ReelConfigSO ReelConfig { get; private set; }
    [field: SerializeField] public SystemConfigSO SystemConfig { get; private set; }

    public SpinState CurrentSpinState;
    public bool IsReelStopped { get; private set; }
    [SerializeField] private List<Reel> reels = new();
    public List<Reel> Reels { get => reels; }

    private int reelStopCount;

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    private void Init()
    {
        IsReelStopped = false;
        CurrentSpinState = SpinState.Idle;
        reelStopCount = 0;
    }

    public void ResetReels()
    {
        StopAllCoroutines();
        CurrentSpinState = SpinState.Idle;
    }

    public void SetReelsData()
    {
        SetReelsForAnticipation();
        SetOutcomeSymbols();
    }

    public void SetNormalSpinData()
    {
        foreach (Reel reel in Reels)
            reel.ConfigureReel(ReelConfig, false);
    }

    public void SpinReels() => StartCoroutine(SpinReelsRoutine());

    private IEnumerator SpinReelsRoutine()
    {
        if (CurrentSpinState == SpinState.Spinning)
        {
            Debug.LogError($"Already spinning");
            yield break;
        }
        IsReelStopped = false;
        RNG.Instance.SendSpinRequest();
        CurrentSpinState = SpinState.Spinning;
        WaitForSeconds waittime = new WaitForSeconds(GetReelRotationOffset);
        foreach (Reel reel in Reels)
        {
            reel.Spin();
            yield return waittime;
        }
    }

    public void SetReels()
    {
        foreach (Reel reel in Reels)
            reel.ConfigureReel(ReelConfig, SystemConfig);
    }

    private float GetReelRotationOffset => ReelConfig.ReelRotationOffset;

    public void SetReelsForAnticipation()
    {
        List<List<int>> matrix = RNG.Instance.playData.matrix;
        int column = 1;
        int scattercount = 0;
        int bonuscount = 0;

        foreach (List<int> rowData in matrix)
        {
            foreach (int symbolId in rowData)
            {
                if (symbolId == SystemConfig.ScatterId)
                    scattercount++;
                else if (symbolId == SystemConfig.BonusId)
                    bonuscount++;
                if ((scattercount >= 2 || bonuscount >= 2) && column < ReelConfig.Columns)
                    SetReelAnticipationProperty(column);
            }
            column++;
        }
    }

    public void SetReelAnticipationProperty(int column) => reels[column].anticipateReelForSpecialSymbol = true;

    public void SetOutcomeSymbols()
    {
        for (int i = 0; i < reels.Count; i++)
            reels[i].SetOutcomeSymbol(RNG.Instance.playData.matrix[i]);
    }

    public IEnumerator StopReelsWithSpecialSymbol(Reel reel, float anticipationDelay)
    {
        #region Stop Forcefully
        if (GameManager.IsSlamStop)
        {
            reel.HideAnticipationGlow();
            yield return new WaitForEndOfFrame();
        }
        #endregion

        #region Stop Normally
        else
        {
            reel.ShowAnticipationGlow();
            yield return DelayAnticipation(anticipationDelay);
        }
        #endregion
    }

    private IEnumerator DelayAnticipation(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    public IEnumerator StopReelsWithoutSpecialSymbol(float reelstopldelay)
    {
        #region Stop Forcefully
        if (GameManager.IsSlamStop)
        {
            yield return new WaitForEndOfFrame();
        }
        #endregion

        #region Stop Normally
        else
        {
            yield return new WaitForSeconds(reelstopldelay);
        }
        #endregion
    }

    public void OnAllReelStop()
    {
        EventManager.InvokeDisableSlamStop();
        CurrentSpinState = SpinState.Idle;
        EventManager.InvokeOnAllReelStoppped();
    }

    public void OnSpinDataFetched() => StartCoroutine(ShowReels(ReelConfig.GetNormalShowReelDelay));

    private IEnumerator ShowReels(float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        SetReelsData();
        StartCoroutine(StopReels());
    }

    private IEnumerator StopReels()
    {
        reelStopCount = 0;
        foreach (Reel reel in Reels)
        {
            if (reel.anticipateReelForSpecialSymbol)
                yield return StopReelsWithSpecialSymbol(reel, SystemConfig.AnticipationDuration);
            else
                yield return StopReelsWithoutSpecialSymbol(GetReelRotationOffset);

            if (reel.IsSpinning)
                reel.SpinStop(() => OnReelStop());
        }
    }

    public void StopReelsImmediately()
    {
        reelStopCount = 0;

        foreach (Reel reel in Reels)
        {
            reel.SpinStop(() => OnReelStop());
            reel.StopSpinRoutine();
        }
    }

    private void OnReelStop()
    {
        if (IsReelStopped)
            return;

        ++reelStopCount;
        if (reelStopCount == ReelConfig.Columns)
        {
            IsReelStopped = true;
            OnAllReelStop();
        }
    }

}
