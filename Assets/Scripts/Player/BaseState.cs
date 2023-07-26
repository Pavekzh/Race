using UnityEngine;
using System;

public abstract class BaseState
{
    private StateMachine stateMachine;
    private Player player;

    public BaseState(StateMachine stateMachine,Player player)
    {
        this.stateMachine = stateMachine;
        this.player = player;
    }

    public abstract void Enter();
    public abstract void Exit();

    public virtual void InputLeft()
    {
        throw new NotImplementedException();
    }

    public virtual void InputRight()
    {
        throw new NotImplementedException();
    }

    public virtual void InputNitro()
    {
        throw new NotImplementedException();
    }

    public virtual void Move()
    {
        throw new NotImplementedException();
    }

    public virtual void SlowDown()
    {
        throw new NotImplementedException();
    }

    public virtual void Trigger(Collider trigger)
    {
        throw new NotImplementedException();
    }
}