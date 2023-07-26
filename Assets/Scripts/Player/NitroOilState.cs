using UnityEngine;
using System;

public class NitroOilState : BaseState
{
    public NitroOilState(StateMachine stateMachine, Player player) : base(stateMachine, player) { }

    public override void Enter()
    {
        Debug.Log("NitroOilState state enter");
    }

    public override void Exit()
    {
        Debug.Log("NitroOilState state exit");
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
