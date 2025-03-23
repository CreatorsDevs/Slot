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

    protected override void Awake()
    {
        base.Awake();
    }

    // TODO: ReelManager Logic
}
