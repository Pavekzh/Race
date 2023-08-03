using UnityEngine;
using Fusion;
using System;

public class RaceFinish:NetworkBehaviour
{
    [Networked] private float HostFinishTime { get; set; }
    [Networked] private float RemoteFinishTime { get; set; }

    private Timer raceTimer;

    public void Init(Timer raceTimer)
    {
        this.raceTimer = raceTimer;
    }

    public void RegisterFinish(Player player)
    {
        if (Runner.IsServer)
        {
            if(player.IsLocal && HostFinishTime == 0)
            {
                HostFinishTime = raceTimer.RaceTime;
                NotifyFinishSelf();
            }
            else if (RemoteFinishTime == 0)
            {
                RemoteFinishTime = raceTimer.RaceTime;                
                RPC_NotifyFinishRemote();
            }
        }
    }

    private void NotifyFinishSelf()
    {
        if (RemoteFinishTime == 0 || HostFinishTime < RemoteFinishTime)
        {
            Win();
        }
        else
            Lose();
    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All,InvokeLocal = false)]
    private void RPC_NotifyFinishRemote()
    {
        if (HostFinishTime == 0 || RemoteFinishTime < HostFinishTime)
        {
            Win();
        }
        else
            Lose();
    }


    private void Win()
    {
        float time = Runner.IsServer ? HostFinishTime : RemoteFinishTime;
        Debug.LogError("Win: "+time);
    }

    private void Lose()
    {
        float time = Runner.IsServer ? HostFinishTime : RemoteFinishTime;
        Debug.LogError("Lose: "+time);
    }
}
