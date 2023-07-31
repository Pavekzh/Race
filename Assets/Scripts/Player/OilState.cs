using UnityEngine;
using System;

public class OilState : BaseState
{
    public OilState(StateMachine<BaseState> stateMachine, Player player) : base(stateMachine, player) { }

    public override void Enter()
    {
        Debug.Log("OilState state enter");
    }

    public override void Exit()
    {
        Debug.Log("OilState state exit");
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
