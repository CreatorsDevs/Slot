using Generics;
using System;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private SymbolPool _symbolPool;
    [SerializeField] private ReelManager _reelManager;
    [SerializeField] private LoadingBar _loadingBar;
    public GameStateMachine StateMachine;

    public static bool IsSlamStop { get; set; }
    public static StateName CurrentState { get; set; }


    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        StartCoroutine(_loadingBar.MoveSliderRandomly());
    }
}

