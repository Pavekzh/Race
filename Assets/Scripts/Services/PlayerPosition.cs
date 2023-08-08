using Fusion;
using UnityEngine;


public class PlayerPosition : NetworkBehaviour
{

    private InGameUI inGameUI;
    private Transform host;
    private Transform remote;

    public void Init(InGameUI inGameUI)
    {
        this.inGameUI = inGameUI;
    }

    public void AddPlayer(Player player)
    {
        if (Runner.IsServer)
        {
            if (player.IsLocal)
                host = player.transform;
            else
                remote = player.transform;
        }
        else
        {
            if (player.IsLocal)
                remote = player.transform;
            else
                host = player.transform;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (Runner.IsServer && host != null && remote != null)
        {
            if (host.position.z > remote.position.z)
                RPC_PositionsChanged(1, 2);
            else
                RPC_PositionsChanged(2, 1);
        }
    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_PositionsChanged(int hostPosition,int remotePosition)
    {
        if (Runner.IsServer)
        {
            inGameUI.UpdatePlayerPosition(hostPosition, 2);
        }
        else
        {
            inGameUI.UpdatePlayerPosition(remotePosition, 2);
        }
    }
}
