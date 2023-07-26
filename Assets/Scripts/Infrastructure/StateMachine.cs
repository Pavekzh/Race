
public class StateMachine
{
    public BaseState CurrentState { get; private set; }

    public void Init(BaseState state)
    {
        CurrentState = state;
        CurrentState.Enter();
    }

    public void ChangeState(BaseState state)
    {
        CurrentState.Exit();

        CurrentState = state;
        CurrentState.Enter();
    }
}
