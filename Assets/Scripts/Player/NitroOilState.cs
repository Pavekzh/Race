using UnityEngine;
using System.Collections;

public class NitroOilState : BaseState
{
    public NitroOilState(StateMachine<BaseState> stateMachine, Player player) : base(stateMachine, player) { }

    protected float slowStartedTime;

    private Coroutine slowTimer;
    private Coroutine nitroTimer;

    public override void Enter()
    {
        player.CC.SetMultipliedMaxSpeed(player.NitroSpeedMultiplier * player.SlowEffectMultiplier);
        player.CC.SetMultipliedAcceleration(player.NitroAccelerationMultiplier);
        player.SetOilIndicator(true);

        slowTimer = player.StartCoroutine(SlowTimer());
        nitroTimer = player.StartCoroutine(NitroTimer());

        Debug.Log("NitroOilState state enter");
    }

    public override void Exit()
    {
        player.CC.SetDefaultAcceleration();
        player.CC.SetDefaultMaxSpeed();
        player.SetOilIndicator(false);

        if (slowTimer != null)
        {
            player.SlowEffectTime -= Time.realtimeSinceStartup - slowStartedTime;
            player.StopCoroutine(slowTimer);
            slowTimer = null;
        }

        if(nitroTimer != null)
        {
            player.StopCoroutine(nitroTimer);
            nitroTimer = null;
        }

        Debug.Log("NitroOilState state exit");
    }

    public override void InputNitro()
    {

    }

    protected override void SlowDown()
    {
        player.StopCoroutine(slowTimer);
        slowTimer = player.StartCoroutine(SlowTimer());
    }

    private IEnumerator SlowTimer()
    {
        slowStartedTime = Time.realtimeSinceStartup;
        yield return new WaitForSeconds(player.SlowEffectTime);
        slowTimer = null;
        stateMachine.SwitchState<NitroState>();
    }

    protected IEnumerator NitroTimer()
    {
        while (player.Nitro > 0)
        {
            yield return null;

            player.Nitro -= player.NitroConsumption * Time.deltaTime;
        }
        nitroTimer = null;
        stateMachine.SwitchState<OilState>();

    }
}
