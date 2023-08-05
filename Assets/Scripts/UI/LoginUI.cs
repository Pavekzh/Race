using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class LoginUI:UIPanel
{
    [SerializeField] private TMP_InputField email;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private Button login;
    [SerializeField] private Button signup;


    private SignupUI signupUI;
    private FirebaseAuthService authService;
    private SceneLoader sceneLoader;
    private Messenger messenger;

    public void Init(SignupUI signupUI, FirebaseAuthService firebaseAuth,SceneLoader sceneLoader,Messenger messenger)
    {
        this.signupUI = signupUI;
        this.authService = firebaseAuth;
        this.sceneLoader = sceneLoader;
        this.messenger = messenger;

        InitButtons();
        TrySilentLogin(signupUI, messenger);
    }

    private void TrySilentLogin(SignupUI signupUI, Messenger messenger)
    {
        string savedEmail = PlayerPrefs.GetString(FirebaseAuthService.SilentEmailPrefsKey, "");
        string savedPassword = PlayerPrefs.GetString(FirebaseAuthService.SilentPasswordPrefsKey, "");
        bool firstLogin = PlayerPrefs.GetInt(FirebaseAuthService.FirstLoginPrefsKey) == 0;

        if (firstLogin)
        {
            signupUI.Open();
        }
        else if (savedEmail != "" && savedPassword != "")
        {
            messenger.ShowMessage("", "Login...", false);
            authService.Login(savedEmail, savedPassword, LoggedIn, LoginFailed);
        }
        else
        {
            Open();
        }
    }

    private void InitButtons()
    {
        this.signup.onClick.AddListener(Signup);
        this.login.onClick.AddListener(Login);
    }

    private void Login()
    {
        authService.Login(email.text, password.text,LoggedIn);
    }

    private void Signup()
    {
        this.Close();
        signupUI.Open();
    }

    private void LoginFailed()
    {
        Open();
        messenger.Close();
    }

    private void LoggedIn()
    {
        sceneLoader.LoadMenu();
    }
}