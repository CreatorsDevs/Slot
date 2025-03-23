using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reel : MonoBehaviour
{
    [SerializeField] public BlurredStrip blurredStrip;
    [SerializeField] public GameObject outcomeStrip;
    [SerializeField] private GameObject anticipationGlowNode;
    [SerializeField] private int baseSpineSortingId = 12; // Since, we set the spine sorting order to be 11 by default on each symbol prefab
    [HideInInspector] public bool anticipateReelForSpecialSymbol;

    private List<Symbol> outcomeSymbols = new();
    private ReelConfigSO reelConfig;
    private Vector3 outcomeStripPosn = new();
    private Coroutine spinCoroutine;
    private bool isSpinning = false;
    private Tween tween;
    private float reelTweenEndDuration;

    public bool IsSpinning { get => isSpinning; private set => isSpinning = value; }


    private void Start()
    {
        outcomeStripPosn = outcomeStrip.transform.localPosition;
    }

    public void ConfigureReel(ReelConfigSO reelconfig, bool setrandomsymbol = true)
    {
        reelConfig = reelconfig;
        if (setrandomsymbol) SetRandomOutcomeSymbols();
    }

    public void SetRandomOutcomeSymbols()
    {
        for (int i = 0; i < reelConfig.Rows + 2; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, SymbolPool.Instance.poolDictionary.Count);
            SymbolPool.Instance.GetObject(randomIndex, outcomeStrip.transform);
        }
    }

    public void SetOutcomeSymbol(List<int> symbolIds)
    {
        ClearOutcomeStrip();
        outcomeSymbols.Clear();
        int fillerCount = 2; // Recommended = 2, to avoid showing blank space
        int sortingOffset = 1;

        #region REEL
        // FILLER FOR REEL
        InstantiateRandomFiller(fillerCount);

        // GENUINE REEL
        foreach (int symbolId in symbolIds)
        {
            Symbol symbol = SymbolPool.Instance.GetObject(symbolId, outcomeStrip.transform).GetComponent<Symbol>();
            symbol.SetSpineSortingOrder(baseSpineSortingId + sortingOffset);
            outcomeSymbols.Add(symbol);

            sortingOffset++;
        }

        // FILLER FOR REEL
        InstantiateRandomFiller(fillerCount);
        #endregion
    }

    private void InstantiateRandomFiller(int fillerCount)
    {
        for (int i = 0; i < fillerCount; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, SymbolPool.Instance.poolDictionary.Count);
            SymbolPool.Instance.GetObject(randomIndex, outcomeStrip.transform);
        }
    }

    public void Spin()
    {
        anticipateReelForSpecialSymbol = false;
        spinCoroutine = StartCoroutine(SpinRoutine());
    }

    public void ClearOutcomeStrip()
    {
        Symbol[] symbols = outcomeStrip.GetComponentsInChildren<Symbol>();
        foreach (Symbol child in symbols)
            SymbolPool.Instance.ReturnObject(child);

        foreach (Transform child in outcomeStrip.transform)
        {
            Destroy(child.gameObject); 
        }
    }

    private IEnumerator SpinRoutine()
    {
        // Anticipate reel rotation
        if (reelConfig.AnticipateRotation)
        {
            float anticipationElapsedTime = 0;
            float anticipationSpeed = reelConfig.RotationSpeed / 4;
            {
                while (anticipationElapsedTime < reelConfig.SpinAnticipationDuration)
                {
                    float t = anticipationElapsedTime / reelConfig.SpinAnticipationDuration;
                    float variedSpeed = Mathf.Lerp(0, anticipationSpeed, t);
                    outcomeStrip.transform.Translate(Vector3.up * (anticipationSpeed - variedSpeed) * Time.deltaTime);
                    anticipationElapsedTime += Time.deltaTime;
                    yield return null;
                }
            }
        }

        float newPositionY = outcomeStrip.transform.localPosition.y - 1550;
        outcomeStrip.SetActive(false);
        outcomeStrip.transform.localPosition = Vector3.up * newPositionY;
        IsSpinning = true;
        blurredStrip.gameObject.SetActive(true);
        blurredStrip.StartSpinIllusion(reelConfig.RotationSpeed);
    }

    public void HideAnticipationGlow() => anticipationGlowNode.SetActive(false);
    public void ShowAnticipationGlow() => anticipationGlowNode.SetActive(true);

    public void SpinStop(Action callback)
    {
        outcomeStrip.SetActive(true);
        disableBlur();
        HideAnticipationGlow();
        if (GameManager.IsSlamStop)
        {
            tween?.Kill();
            outcomeStrip.transform.localPosition = Vector3.zero; // value: 400 is for setting the offset of the outcome strip's local position. 
            callback();
            return;
        }
        else
        {
            reelTweenEndDuration = 0.3f;
            outcomeStrip.transform.localPosition = new(outcomeStripPosn.x, outcomeStripPosn.y + 400, outcomeStripPosn.z); // value: 400 is for setting the offset of the outcome strip's local position. 
        }
        tween = outcomeStrip.transform.DOLocalMoveY(outcomeStripPosn.y, reelTweenEndDuration)
            .SetEase(Ease.OutBack)
            .OnComplete(() => { callback(); });
    }
    public void StopSpinRoutine()
    {
        if (spinCoroutine != null)
        {
            StopCoroutine(spinCoroutine);
        }
    }

    private void disableBlur()
    {
        blurredStrip.gameObject.SetActive(false);
        IsSpinning = false;
    }
}
