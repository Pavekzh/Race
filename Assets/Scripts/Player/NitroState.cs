using UnityEngine;
using System;
using System.Collections;

public class NitroState : BaseState
{
    public NitroState(StateMachine<BaseState> stateMachine, Player player) : base(stateMachine, player) { }

    private Coroutine timer;

    public override void Enter()
    {
        player.CC.SetMultipliedMaxSpeed(player.NitroSpeedMultiplier);
        player.CC.SetMultipliedAcceleration(player.NitroAccelerationMultiplier);
        timer = player.StartCoroutine(Timer());

        Debug.Log("NitroState state enter");
    }

    public override void Exit()
    {
        player.CC.SetDefaultAcceleration();
        player.CC.SetDefaultMaxSpeed();

        if(timer != null)
        {
            player.StopCoroutine(timer);
            timer = null;
        }

        Debug.Log("NitroState state exit");
    }

    protected override void SlowDown()
    {
        stateMachine.SwitchState<NitroOilState>();
    }

    public override void InputNitro()
    {
       
    }

    protected IEnumerator Timer()
    {
        while(player.Nitro > 0)
        {
            yield return null;

            player.Nitro -= player.NitroConsumption * Time.deltaTime;
        }
        timer = null;
        stateMachine.SwitchState<DefaultState>();

    }

}
