using Fusion;
using System;
using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerFactory playerFactory;
    [Header("Network")]
    [SerializeField] private NetworkRunner network;
    [SerializeField] private NetworkSceneManagerDefault sceneManager;
    [Header("Services")]
    [SerializeField] private GameStarter gameStarter;
    [Header("UI")]
    [SerializeField] private MatchmakingUI matchmakingUI;

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
        await InitNetwork();
        InitLevel();
        CreatePlayer();
    }

    private void InitMatchMakingUI()
    {
        matchmakingUI.Init(gameStarter);
    }

    private async System.Threading.Tasks.Task InitNetwork()
    {
        StartGameArgs args = new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = "",
            Scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex,
            PlayerCount = MaxPlayers,
            SceneManager = sceneManager
        };
        await network.StartGame(args);
    }    
    
    private void InitLevel()
    {
        
    }

    private void CreatePlayer()
    {
        playerFactory.CreatePlayer(network);
    }



    public void InitPlayer(Player player)
    {        
        matchmakingUI.PlayerJoined();

        if (this.player == null)
            this.player = player;

        player.Init();

        gameStarter.AddReadyPlayer();
    }    
   
}
