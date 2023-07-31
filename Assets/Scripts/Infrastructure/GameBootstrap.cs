using Fusion;
using UnityEngine;


public class GameBootstrap : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private int firstStartLane = 1;
    [SerializeField] private int secondStartLane = 2;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private PlayerFactory playerFactory;
    [SerializeField] private InputDetector inputDetector;
    [Header("Network")]
    [SerializeField] private NetworkRunner network;
    [SerializeField] private NetworkSceneManagerDefault sceneManager;
    [Header("Services")]
    [SerializeField] private Transform sectionsParent;
    [SerializeField] private Transform obstaclesParent;
    [SerializeField] private RaceStarter gameStarter;
    [SerializeField] private WorldGenerator worldGenerator;
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
        InitGameStarter();
        InitLevel();
        InitInput();
        
        await InitNetwork();
        CreatePlayer();
    }

    private void InitGameStarter()
    {
        gameStarter.Init(matchmakingUI,worldGenerator);
    }

    private void InitMatchMakingUI()
    {
        matchmakingUI.Init();
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
        worldGenerator.Init(sectionsParent, obstaclesParent);
    }

    private void InitInput()
    {
        inputDetector.Init();
    }

    private void InitCamera()
    {
        cameraFollow.Init(player.transform);
    }

    private void CreatePlayer()
    {
        int startLane;
        if (network.IsSharedModeMasterClient)
            startLane = firstStartLane;
        else
            startLane = secondStartLane;

        playerFactory.CreatePlayer(network,worldGenerator,startLane);
    }



    public void InitPlayer(Player player)
    {        
        matchmakingUI.PlayerJoined();

        if (this.player == null)
        {
            this.player = player;        
            InitCamera();
            player.Init(inputDetector, worldGenerator,firstStartLane);
        }
        else
            player.Init(inputDetector, worldGenerator,secondStartLane);

        gameStarter.AddReadyPlayer(player);

    }
}
