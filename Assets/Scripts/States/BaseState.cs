using FSM;

public abstract class BaseState : State
{
    protected GamePlayStateMachine gameStateMachine;

    public BaseState(GamePlayStateMachine gamestatemachine)
    {
        gameStateMachine = gamestatemachine;
    }
}