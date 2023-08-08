using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;

public class RaceFinish:NetworkBehaviour,INetworkRunnerCallbacks
{
    [Networked] private float HostFinishTime { get; set; }
    [Networked] private float RemoteFinishTime { get; set; }

    public bool HasWinner { get => HostFinishTime != 0 || RemoteFinishTime != 0; }
    public bool IsOpponentFinished
    {
        get
        {
            if (Runner.IsServer)
                return RemoteFinishTime != 0;
            else
                return HostFinishTime != 0;
        }
    }
    public bool IsFinished
    {
        get
        {
            if (Runner.IsServer)
                return HostFinishTime != 0;
            else
                return RemoteFinishTime != 0;
        }
    }

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
                RPC_NotifyRemoteOpponentFinish();
            }
            else if (RemoteFinishTime == 0)
            {
                RemoteFinishTime = raceTimer.RaceTime;                
                RPC_NotifyFinishRemote();
                NotifySelfOpponentFinish();
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

    private void NotifySelfOpponentFinish()
    {
        raceFinishUI.UnlockQuit();
    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All, InvokeLocal = false)]
    private void RPC_NotifyRemoteOpponentFinish()
    {
        raceFinishUI.UnlockQuit();
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

    private void PlayerLeft()
    {
        if (!IsFinished)
        {
            if (IsOpponentFinished)
            {
                raceFinishUI.ShowUI(false);
            }
            else
            {
                raceFinishUI.ShowUI(true);
            }
        }
        raceFinishUI.UnlockQuit();
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        PlayerLeft();
        runner.Shutdown();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        PlayerLeft();
        runner.Shutdown();
    }

    #region unused network callbacks
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }
    #endregion
}
