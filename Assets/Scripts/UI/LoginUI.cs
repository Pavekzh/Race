using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class LoginUI:UIPanel
{
    [SerializeField] private TMP_InputField email;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private Button login;
    [SerializeField] private Button signup;

    private const string FirstLoginPrefsKey = "wasLoggedIn";

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
        SelectLoginMethod(signupUI, messenger);
    }

    private void SelectLoginMethod(SignupUI signupUI, Messenger messenger)
    {
        bool firstLogin = PlayerPrefs.GetInt(FirstLoginPrefsKey) == 0;

        if (authService.IsLoggedIn)
        {
            LoggedIn();
            if (firstLogin)
                PlayerPrefs.SetInt(FirstLoginPrefsKey, 1);
        }
        else if (firstLogin)
        {
            signupUI.Open();
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
        messenger.ShowMessage("", "Login...", false);
        sceneLoader.LoadMenu();
    }
}