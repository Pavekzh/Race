using UnityEngine;
using System.Collections;
using System;

public abstract class BaseState
{
    private StateMachine<BaseState> stateMachine;
    private Player player;

    public BaseState(StateMachine<BaseState> stateMachine,Player player)
    {
        this.stateMachine = stateMachine;
        this.player = player;
    }

    public abstract void Enter();
    public abstract void Exit();

    public virtual void InputLeft()
    {
        if (player.CurrentLane != 0)
        {
            player.CurrentLane--;
            UpdateLane();
        }

    }

    public virtual void InputRight()
    {
        if (player.CurrentLane != 3)
        {
            player.CurrentLane++;
            UpdateLane();
        }

    }

    public virtual void InputNitro()
    {
        throw new NotImplementedException();
    }

    public virtual void Move()
    {
        player.CC.Move(player.MoveDirection);
    }


    public virtual void SlowDown()
    {
        throw new NotImplementedException();
    }

    public virtual void Trigger(Collider trigger)
    {
        throw new NotImplementedException();
    }

    protected void UpdateLane()
    {
        Vector3 newPos = new Vector3(player.WorldGenerator.LanesXs[player.CurrentLane], player.transform.position.y, player.transform.position.z);
        player.CC.SetPosition(newPos);
    }

    protected IEnumerator ChangingLane()
    {
        float startingDelta = MathF.Abs(player.WorldGenerator.LanesXs[player.CurrentLane] - player.transform.position.x);
        float startingX = player.transform.position.x;

        while (true)
        {
            float newX = player.transform.position.x;
            newX += MathF.Sign(player.WorldGenerator.LanesXs[player.CurrentLane] - player.transform.position.x) * player.ChangingLaneSpeed * Time.fixedDeltaTime;

            if (MathF.Abs(newX - startingX) > startingDelta)
                break;
            else
            {
                player.CC.SetPosition(new Vector3(newX, player.transform.position.y, player.transform.position.z));
                yield return new WaitForFixedUpdate();
            }
              
                
        }

        float resultX = player.WorldGenerator.LanesXs[player.CurrentLane];
        player.CC.SetPosition(new Vector3(resultX, player.transform.position.y, player.transform.position.z));
    }
}