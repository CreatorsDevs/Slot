using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

public class EventManager
{

    #region ACTIONS
    // Camera Actions
    public static Action<int, int> OnOrientationChangeEvent;

    #endregion

    public static void InvokeOrientationChange(int x, int y)
    {
        OnOrientationChangeEvent(x, y);
    }
}
