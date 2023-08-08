using UnityEngine;

public class MenuBootstrap : MonoBehaviour
{
    [Header("Systems")]
    [SerializeField] private FirebaseInit firebaseInit;
    [SerializeField] private FirebaseDatabaseService databaseService;
    [SerializeField] private FirebaseAuthService authService;
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private UserAvatars userAvatars;
    [Header("UI")]
    [SerializeField] private Messenger messenger;
    [SerializeField] private MainMenuUI mainMenuUI;
    [SerializeField] private UserProfileUI userProfileUI;
    [SerializeField] private Leaderboard leaderboard;


    private void Awake()
    {
        InitDatabaseService();       
        InitAuthService();
        InitMainMenuUI();
        InitUserProfileUI();
        InitLeaderboard();
    }

    private void InitLeaderboard()
    {
        leaderboard.Init(databaseService,userAvatars);
    }

    private void InitMainMenuUI()
    {
        mainMenuUI.Init(sceneLoader,databaseService,authService);
    }

    private void InitUserProfileUI()
    {
        userProfileUI.Init(databaseService,userAvatars);
    }

    private void InitDatabaseService()
    {
        databaseService.Init(Firebase.Database.FirebaseDatabase.DefaultInstance, Firebase.Auth.FirebaseAuth.DefaultInstance);
    }

    private void InitAuthService()
    {
        authService.Init(messenger, firebaseInit, Firebase.Auth.FirebaseAuth.DefaultInstance, databaseService);
    }
}
