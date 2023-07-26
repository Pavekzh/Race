using UnityEngine;
using System;

public class NitroState : BaseState
{
    public NitroState(StateMachine stateMachine, Player player) : base(stateMachine, player) { }

    public override void Enter()
    {
        Debug.Log("NitroState state enter");
    }

    public override void Exit()
    {
        Debug.Log("NitroState state exit");
    }

    public override void InputNitro()
    {
        throw new NotImplementedException();
    }

    public override void Move()
    {
        throw new NotImplementedException();
    }

    public override void SlowDown()
    {
        throw new NotImplementedException();
    }
}
