using Generics;
using System;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private SymbolPool symbolPool;
    [SerializeField] private ReelManager reelManager;
    [SerializeField] private LoadingBar loadingBar;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameObject BaseBG;
    [SerializeField] private GameObject FreeBG;
    [SerializeField] private GameObject FreeIntro;
    [SerializeField] private GameObject FreeOutro;

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
        EventManager.OnScatterPaylineStopped += OnScatterPaylineStopped;
        EventManager.OnBonusPaylineStopped += OnBonusPaylineStopped;
        EventManager.ScatterStateStartedEvent += EnableFreeGameBG;
        EventManager.NormalStateStartedEvent += DisableFreeGameBG;
        EventManager.ShowFreeSpinIntro += ShowFreeIntroPopUp;
        EventManager.ShowFreeSpinOutro += ShowFreeOutroPopUp;
        EventManager.CloseFreeSpinIntro += CloseFreeIntroPopUp;
        EventManager.CloseFreeSpinOutro += CloseFreeOutroPopUp;
    }

    private void Start()
    {
        StartGame();
        symbolPool.CreateSymbolPool();
        reelManager.SetReels();
    }

    private void StartGame()
    {
        uiManager.SetInitialBalance();
        gamePlayStateMachine.SwitchState(gamePlayStateMachine.NormalGameState);
        StartCoroutine(loadingBar.MoveSliderRandomly());
    }

    public void OnSpinClicked()
    {
        currentGameState = GameStatesType.NormalSpin;
        currentSpinState = SpinStatesTypes.Spinning;
        EventManager.InvokeSpinButton();
        EconomyManager.OnUpdateCurrentBalance();
    }

    private void ShowFreeIntroPopUp()
    {
        FreeIntro.SetActive(true);
    }

    private void CloseFreeIntroPopUp()
    {
        FreeIntro.SetActive(false);
    }

    private void ShowFreeOutroPopUp()
    {
        FreeOutro.SetActive(true);
    }

    private void CloseFreeOutroPopUp()
    {
        FreeOutro.SetActive(false);
    }

    private void EnableFreeGameBG()
    {
        BaseBG.SetActive(false);
        FreeBG.SetActive(true);
    }

    private void DisableFreeGameBG()
    {
        BaseBG.SetActive(true);
        FreeBG.SetActive(false);
    }
    
    public void ResetSlamStop()
    {
        IsSlamStop = false;
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

    public void OnScatterPaylineStopped()
    {
        gamePlayStateMachine.SwitchState(gamePlayStateMachine.ScatterGameState);
    }

    public void OnBonusPaylineStopped()
    {
        gamePlayStateMachine.SwitchState(gamePlayStateMachine.BonusGameState);
    }

    private void OnDisable()
    {
        EventManager.OnSpinClickedEvent -= OnSpinClicked;
        EventManager.OnScatterPaylineStopped -= OnScatterPaylineStopped;
        EventManager.OnBonusPaylineStopped -= OnBonusPaylineStopped;
        EventManager.ScatterStateStartedEvent -= EnableFreeGameBG;
        EventManager.NormalStateStartedEvent -= DisableFreeGameBG;
        EventManager.ShowFreeSpinIntro -= ShowFreeIntroPopUp;
        EventManager.ShowFreeSpinOutro -= ShowFreeOutroPopUp;
        EventManager.CloseFreeSpinIntro -= CloseFreeIntroPopUp;
        EventManager.CloseFreeSpinOutro -= CloseFreeOutroPopUp;
    }
}

