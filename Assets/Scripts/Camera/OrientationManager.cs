using AutoLetterbox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationManager : MonoBehaviour
{
    [SerializeField] private RectTransform portraitUI;
    [SerializeField] private RectTransform landscapeUI;
    [SerializeField] private RectTransform Slot;
    private int x, y;
    private float ScaledDown;
    private float NativeScale;

    private void Start()
    {
        ScaledDown = 0.8f;
        NativeScale = 1f;
    }

    void Update()
    {
        if(Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        {
            Slot.localScale = new(ScaledDown, ScaledDown, ScaledDown);
            x=9; y=16;
            EventManager.InvokeOrientationChange(x, y);
            portraitUI.gameObject.SetActive(true);
            landscapeUI.gameObject.SetActive(false);
        }
        else if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
        {
            Slot.localScale = new(NativeScale, NativeScale, NativeScale);
            x = 16; y = 9;
            EventManager.InvokeOrientationChange(x, y);
            portraitUI.gameObject.SetActive(false);
            landscapeUI.gameObject.SetActive(true);
        }
    }
}
