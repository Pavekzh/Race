using Fusion;

public class RaceFinish:NetworkBehaviour
{
    [Networked] private float HostFinishTime { get; set; }
    [Networked] private float RemoteFinishTime { get; set; }

    private Timer raceTimer;
    private RaceFinishUI raceFinishUI;

    public void Init(Timer raceTimer,RaceFinishUI raceFinishUI)
    {
        this.raceTimer = raceTimer;
        this.raceFinishUI = raceFinishUI;
    }

    public void RegisterFinish(Player player)
    {           
        player.StopRace();
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
        raceFinishUI.ShowUI(true, time / 60);
    }

    private void Lose()
    {
        float time = Runner.IsServer ? HostFinishTime : RemoteFinishTime;
        raceFinishUI.ShowUI(false, time / 60);
    }
}
