using UnityEngine;

public class AuthBootstrap : MonoBehaviour
{
    [SerializeField] private FirebaseAuthService firebaseAuth;
    [SerializeField] private FirebaseInit firebaseInit;
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
        firebaseAuth.Init(messenger, firebaseInit);
    }

    private void InitLoginUI()
    {
        loginUI.Init(signupUI, firebaseAuth);
    }

    private void InitSignupUI()
    {
        signupUI.Init(loginUI, firebaseAuth);
    }
}