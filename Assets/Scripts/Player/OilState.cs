using System.Collections;
using UnityEngine;

public class OilState : BaseState
{
    public OilState(StateMachine<BaseState> stateMachine, Player player) : base(stateMachine, player) { }

    protected Coroutine timer;
    protected float slowStartedTime;

    public override void Enter()
    {
        player.CC.SetMultipliedMaxSpeed(player.SlowEffectMultiplier);
        timer = player.StartCoroutine(Timer());

        Debug.Log("OilState state enter");
    }

    public override void Exit()
    {
        player.CC.SetDefaultMaxSpeed();

        if (timer != null)
        {
            player.SlowEffectTime -= Time.realtimeSinceStartup - slowStartedTime;
            player.StopCoroutine(timer);
            timer = null;
        }

        Debug.Log("OilState state exit");
    }

    public override void InputNitro()
    {
        stateMachine.SwitchState<NitroOilState>();
    }

    protected override void SlowDown()
    {
        player.StopCoroutine(timer);
        timer = player.StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        slowStartedTime = Time.realtimeSinceStartup;
        yield return new WaitForSeconds(player.SlowEffectTime);
        timer = null;
        stateMachine.SwitchState<DefaultState>();
    }
}
