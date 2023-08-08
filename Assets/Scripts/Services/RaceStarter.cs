using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class RaceStarter:NetworkBehaviour
{    
    private List<Player> readyPlayers = new List<Player>();
    private bool isOpponentsScreenShown;
    private bool isLevelReady;

    private int StartersReady { get; set; }

    private MatchmakingUI matchmaking;
    private Timer raceTimer;

    public void Init(MatchmakingUI matchmaking,WorldGenerator worldGenerator,Timer raceTimer)
    {
        this.matchmaking = matchmaking;
        this.raceTimer = raceTimer;
        matchmaking.OpponentsScreenShown += OpponentsScreenShown;
        worldGenerator.LevelGenerated += LevelGenerated;
    }

    public void AddReadyPlayer(Player player)
    {
        readyPlayers.Add(player);

        if (readyPlayers.Count == GameBootstrap.MaxPlayers && isOpponentsScreenShown && isLevelReady)
            RPC_GameStarterReady();
    }

    public void LevelGenerated()
    {
        isLevelReady = true;

        if (readyPlayers.Count == GameBootstrap.MaxPlayers && isOpponentsScreenShown && isLevelReady)
            RPC_GameStarterReady();
    }

    public void OpponentsScreenShown()
    {
        isOpponentsScreenShown = true;

        if (readyPlayers.Count == GameBootstrap.MaxPlayers && isOpponentsScreenShown && isLevelReady)
            RPC_GameStarterReady();
    }


    [Rpc(sources: RpcSources.All,targets: RpcTargets.StateAuthority,InvokeLocal = true)]
    public void RPC_GameStarterReady()
    {
        StartersReady++;
        if (Runner.IsServer)
        {
            raceTimer.StartTimer();
            if (StartersReady == GameBootstrap.MaxPlayers)
                RPC_StartGame();
        }

    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All, InvokeLocal = true)]
    public void RPC_StartGame()
    {
        matchmaking.Hide();        
        
        foreach(Player player in readyPlayers)
        {
            player.StartRace();
        }
    }
}