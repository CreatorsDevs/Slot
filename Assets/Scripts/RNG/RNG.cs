using Generics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RNG : MonoSingleton<RNG>
{
    private double currentBalance = 100000d;
    public double CurrentBalance { get => currentBalance; }

}
