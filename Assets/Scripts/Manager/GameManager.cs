using Generics;
using System;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private SymbolPool symbolPool;
    [SerializeField] private ReelManager reelManager;
    [SerializeField] private LoadingBar loadingBar;
    [SerializeField] private UIManager uiManager;

    [Header("Public Fields!")]
    public GamePlayStateMachine gamePlayStateMachine;
    public RNG rng;
    public static bool IsSlamStop { get; set; }
    public static StateName CurrentState { get; set; }


    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        EventManager.OnSpinClickedEvent += OnSpinClicked;
    }

    private void Start()
    {
        symbolPool.CreateSymbolPool();
        StartGame();
    }

    private void StartGame()
    {
        uiManager.SetInitialBalance();
        StartCoroutine(loadingBar.MoveSliderRandomly());
        reelManager.SetReels();
    }

    public void OnSpinClicked()
    {
        currentGameState = GameStatesType.NormalSpin;
        currentSpinState = SpinStatesTypes.Spinning;
        EventManager.InvokeSpinButton();
        EconomyManager.OnUpdateCurrentBalance();
    }

    public enum GameStatesType
    {
        NormalSpin = 0,
        Idle,
        FreeGame,
        BonusGame
    }

    public enum SpinStatesTypes
    {
        Idle = 0,
        Spinning
    }

    SpinStatesTypes currentSpinState;
    public SpinStatesTypes CurrentSpinState
    {
        get { return currentSpinState; }
        set
        {
            Debug.Log("Spin State set Value = >  " + value);
            currentSpinState = value;
        }
    }

    GameStatesType currentGameState;
    public GameStatesType CurrentGameState
    {
        get { return currentGameState; }
        set
        {
            Debug.Log("Game State set Value = >  " + value);
            currentGameState = value;
        }
    }

    private void OnDisable()
    {
        EventManager.OnSpinClickedEvent -= OnSpinClicked;
    }
}

