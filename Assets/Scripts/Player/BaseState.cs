using UnityEngine;
using System.Collections;
using System;

public abstract class BaseState
{
    protected StateMachine<BaseState> stateMachine;
    protected Player player;

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
        if(player.Nitro > 0)
            stateMachine.SwitchState<NitroState>();
    }

    public virtual void Move()
    {
        player.CC.Move(player.MoveDirection);
    }

    public virtual void Trigger(Collider trigger)
    {

        if(trigger.tag == player.ObstacleTag)
        {
            IObstacle obstacle = trigger.GetComponent<IObstacle>();
            if(obstacle.Type == ObstacleType.Oil)
            {
                Oil oil = obstacle as Oil;
                player.SlowEffectMultiplier = oil.SpeedMultiplier;
                player.SlowEffectTime = oil.EffectTime;
                SlowDown();
            }
            else if(obstacle.Type == ObstacleType.Hatch)
            {
                Hatch hatch = obstacle as Hatch;
                ThrowBack(hatch.ThrowBack);
                ResetSpeed(hatch.ResetedSpeed);
            }
            else if (obstacle.Type == ObstacleType.Wall)
            {
                Wall wall = obstacle as Wall;
                ResetSpeed(wall.ResetedSpeed);                  
            }
            else if (obstacle.Type == ObstacleType.Nitro)
            {
                Nitro nitro = obstacle as Nitro;
                AddNitro(nitro.Extract());
            }
        }

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

    protected void AddNitro(float amount)
    {
        player.Nitro += amount;
    }

    protected void ThrowBack(float delta)
    {
        Vector3 throwedPosition = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - delta);
        player.CC.SetPosition(throwedPosition);
    }

    protected void ResetSpeed(float speed)
    {
        player.CC.SetSpeed(speed);
    }

    protected virtual void SlowDown()
    {
        stateMachine.SwitchState<OilState>();
    }
}