using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class GameBootstrap : MonoBehaviour,INetworkRunnerCallbacks
{
    [Header("Player")]
    [SerializeField] private int firstStartLane = 1;
    [SerializeField] private int secondStartLane = 2;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private PlayerFactory playerFactory;    
    [SerializeField] private PlayerPosition playerPosition;
    [SerializeField] private InputDetector inputDetector;
    [SerializeField] private SwipeInputDetector swipeInputDetector;
    [Header("Network")]
    [SerializeField] private NetworkRunner network;
    [SerializeField] private NetworkSceneManagerDefault sceneManager;
    [Header("Services")]
    [SerializeField] private Transform sectionsParent;
    [SerializeField] private Transform obstaclesParent;
    [SerializeField] private RaceStarter raceStarter;
    [SerializeField] private RaceFinish raceFinish;
    [SerializeField] private Timer raceTimer;

    [SerializeField] private WorldGenerator worldGenerator;
    [Header("UI")]
    [SerializeField] private MatchmakingUI matchmakingUI;
    [SerializeField] private InGameUI inGameUI;

    private Player player;

    /// <summary>
    /// Do not use it directly except Auto Inject classes
    /// </summary>
    public static GameBootstrap Instance;

    public const int MaxPlayers = 2;

    private async void Awake()
    {
        if (Instance != null)
            Debug.LogError($"Second instance of GameBootstrap on scene ({gameObject.name})");

        Instance = this;

        InitMatchMakingUI();
        InitTimer();
        InitPlayerPosition();
        InitRaceStarter();
        InitRaceFinish();
        InitLevel();
        InitSwipeInput();
        InitInput();
        
        await InitNetwork();
    }    
    
    private async System.Threading.Tasks.Task InitNetwork()
    {
        network.AddCallbacks(this);
        StartGameArgs args = new StartGameArgs()
        {
            GameMode = GameMode.AutoHostOrClient,
            Scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex,
            PlayerCount = MaxPlayers,
            SceneManager = sceneManager
        };
        await network.StartGame(args);
    }  
    
    private void InitMatchMakingUI()
    {
        matchmakingUI.Init();
    }

    private void InitTimer()
    {
        raceTimer.Init(inGameUI);
    }

    private void InitPlayerPosition()
    {
        playerPosition.Init(inGameUI);
    }

    private void InitRaceStarter()
    {
        raceStarter.Init(matchmakingUI,worldGenerator,raceTimer);
    }

    private void InitRaceFinish()
    {
        raceFinish.Init(raceTimer);
    }
    
    private void InitLevel()
    {
        worldGenerator.Init(sectionsParent, obstaclesParent,this);
    }

    private void InitInput()
    {
        inputDetector.Init();
        network.AddCallbacks(inputDetector);
    }

    private void InitSwipeInput()
    {
        swipeInputDetector.Init(inGameUI);
    }

    private void InitCamera()
    {
        cameraFollow.Init(player.transform);
    }

    public void InitPlayer(Player player)
    {        
        matchmakingUI.PlayerJoined();

        int startingLane;
        bool isLocal;

        if (player.HasInputAuthority)
        {
            this.player = player;        
            InitCamera();
            
            

            startingLane = network.IsServer ? firstStartLane : secondStartLane;
            isLocal = true;
            player.Init(worldGenerator, startingLane, isLocal,inGameUI);
        }
        else
        {
            startingLane = network.IsServer ? secondStartLane : firstStartLane;
            isLocal = false;
            player.Init(worldGenerator, startingLane, isLocal);
        }

        playerPosition.AddPlayer(player);
        raceStarter.AddReadyPlayer(player);

    }

    public void InitFinishLine(FinishLine finishLine)
    {
        finishLine.Init(raceFinish);
    }



    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            int startLane;
            
            if (runner.ActivePlayers.Count() == 1)
                startLane = firstStartLane;
            else
                startLane = secondStartLane;

            playerFactory.CreatePlayer(network, worldGenerator, startLane,player);
        }
    }

    #region unused network callbacks
    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public virtual void OnInput(NetworkRunner runner, NetworkInput input)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
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

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }
    #endregion
}
