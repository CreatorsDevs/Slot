using FSM;
using FSM.GameState;

public class GameStateMachine : StateMachine

{
    #region states
    public State NormalGameState { get; private set; }
    public State ScatterGameState { get; private set; }
    public State BonusGameState { get; private set; }
    #endregion

    private void Awake()
    {
        Init();
    }
    public void Init()
    {
        NormalGameState = new NormalState(this);
        ScatterGameState = new ScatterState(this);
        BonusGameState = new BonusState(this);
    }

}