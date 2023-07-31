using System.Collections.Generic;
using System.Linq;


public class StateMachine<T> where T : BaseState
{
    private T currentState;
    protected List<T> states;

    public T CurrentState
    {
        get => currentState;
        private set
        {
            if (currentState != null)
                currentState.Exit();

            currentState = value;
            currentState.Enter();
        }
    }

    public StateMachine()
    {
        states = new List<T>();
    }

    public void AddState(T state)
    {
        states.Add(state);
    }

    public void InitState<U>() where U : T
    {
        CurrentState = states.FirstOrDefault(state => state is U);
    }

    public void SwitchState<U>() where U : T
    {
        CurrentState = states.FirstOrDefault(state => state is U);
    }

}
