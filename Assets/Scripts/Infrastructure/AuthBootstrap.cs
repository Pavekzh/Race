using UnityEngine;

public class AuthBootstrap : MonoBehaviour
{
    [Header("Systems")]
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private FirebaseInit firebaseInit;
    [SerializeField] private FirebaseDatabaseService databaseService;    
    [SerializeField] private FirebaseAuthService authService;
    [Header("UI")]
    [SerializeField] private Messenger messenger;
    [SerializeField] private LoginUI loginUI;
    [SerializeField] private SignupUI signupUI;

    private async void Awake()
    {
        await firebaseInit.Init();

        InitFirebaseAuth();
        InitLoginUI();
        InitSignupUI();
    }

    private void InitFirebaseAuth()
    {
        authService.Init(messenger, firebaseInit,Firebase.Auth.FirebaseAuth.DefaultInstance,databaseService);
    }

    private void InitLoginUI()
    {
        loginUI.Init(signupUI, authService,sceneLoader,messenger);
    }

    private void InitSignupUI()
    {
        signupUI.Init(loginUI, authService,sceneLoader);
    }
}