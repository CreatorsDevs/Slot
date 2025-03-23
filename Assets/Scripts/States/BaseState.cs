using FSM;

public abstract class BaseState : State
{
    protected GameStateMachine _gameStateMachine;

    public BaseState(GameStateMachine gamestatemachine)
    {
        _gameStateMachine = gamestatemachine;
    }
}