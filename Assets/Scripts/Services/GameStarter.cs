using UnityEngine;
using Fusion;

public class GameStarter:NetworkBehaviour
{    
    private int readyPlayers = 0;
    private bool isOpponentsScreenShown;
    private bool isLevelReady;

    [Networked] private int StartersReady { get; set; }

    MatchmakingUI matchmaking;

    public void Init(MatchmakingUI matchmaking,WorldGenerator worldGenerator)
    {
        this.matchmaking = matchmaking;
        matchmaking.OpponentsScreenShown += OpponentsScreenShown;
        worldGenerator.LevelGenerated += LevelGenerated;
    }
    public void AddReadyPlayer()
    {
        readyPlayers++;

        if (readyPlayers == GameBootstrap.MaxPlayers && isOpponentsScreenShown && isLevelReady)
            RPC_GameStarterReady();
    }

    public void LevelGenerated()
    {
        isLevelReady = true;

        if (readyPlayers == GameBootstrap.MaxPlayers && isOpponentsScreenShown && isLevelReady)
            RPC_GameStarterReady();
    }

    public void OpponentsScreenShown()
    {
        isOpponentsScreenShown = true;

        if (readyPlayers == GameBootstrap.MaxPlayers && isOpponentsScreenShown && isLevelReady)
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
        matchmaking.Hide();
        Debug.LogError("Game started");
    }
}