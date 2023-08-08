using UnityEngine;

public class DefaultState : BaseState
{
    public DefaultState(StateMachine<BaseState> stateMachine, Player player) : base(stateMachine, player) { }

    public override void Enter()
    {
        Debug.Log("Default state enter");
    }

    public override void Exit()
    {
        Debug.Log("Default state exit");
    }
}