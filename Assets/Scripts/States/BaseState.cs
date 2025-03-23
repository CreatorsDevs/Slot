using FSM;

public abstract class BaseState : State
{
    protected GamePlayStateMachine _gameStateMachine;

    public BaseState(GamePlayStateMachine gamestatemachine)
    {
        _gameStateMachine = gamestatemachine;
    }
}