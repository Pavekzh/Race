using UnityEngine;
using Fusion;

public class GameStarter:NetworkBehaviour
{    
    private int readyPlayers = 0;
    private bool isOpponentsScreenShown;

    [Networked] private int StartersReady { get; set; }

    public void AddReadyPlayer()
    {
        readyPlayers++;

        if (readyPlayers == GameBootstrap.MaxPlayers && isOpponentsScreenShown)
            RPC_GameStarterReady();
    }

    public void OpponentsScreenShown()
    {
        isOpponentsScreenShown = true;

        if (readyPlayers == GameBootstrap.MaxPlayers && isOpponentsScreenShown)
            RPC_GameStarterReady();
    }

    [Rpc(sources: RpcSources.All,targets: RpcTargets.StateAuthority,InvokeLocal = true)]
    public void RPC_GameStarterReady()
    {
        StartersReady++;

        if (StartersReady == GameBootstrap.MaxPlayers)
            RPC_StartGame();
    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All, InvokeLocal = true)]
    public void RPC_StartGame()
    {
        Debug.LogError("Game started");
    }
}