using CusTween;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaylineWinning : MonoBehaviour
{
    [SerializeField] private GameObject[] paylines;
    [SerializeField] private GameObject TotalWinPanel;
    [SerializeField] private MultipleFontHandler totalWinText;
    [SerializeField] private float normalAnimationDuration = 0.5f;

    public void Reset()
    {
        CancelInvoke(nameof(HideTotalWin));
        HidePayline();
        HideTotalWin();
    }

    public void ShowTotalWin(double amt)
    {
        RectTransform rt = TotalWinPanel.GetComponent<RectTransform>();
        rt.localScale = Vector3.zero;

        TotalWinPanel.SetActive(true);

        totalWinText.setText(EconomyManager.GetTruncatedTwoDecimalFloatAmount(amt));

        rt.DOScale(1f, normalAnimationDuration)
                    .SetEase(Ease.OutBack);

        Invoke(nameof(HideTotalWin), 2f);
    }

    public void ShowPayline(int paylineId)
    {
        paylines[paylineId].SetActive(true);
    }

    public void HidePayline(int id)
    {
        paylines[id].SetActive(false);
    }

    public void HidePayline()
    {
        foreach (GameObject payline in paylines)
            payline.SetActive(false);
    }

    private void HideTotalWin()
    {
        RectTransform rt = TotalWinPanel.GetComponent<RectTransform>();
        rt.DOScale(0f, normalAnimationDuration / 2).SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    totalWinText.setText(0f);
                    TotalWinPanel.SetActive(false);
                });
    }
}
