using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SignupUI:UIPanel
{
    [SerializeField] private TMP_InputField email;
    [SerializeField] private TMP_InputField username;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private TMP_InputField repeatPassword;

    [SerializeField] private Button login;
    [SerializeField] private Button signup;

    private LoginUI loginUI;
    private FirebaseAuthService authService;
    private SceneLoader sceneLoader;


    public void Init(LoginUI loginUI, FirebaseAuthService firebaseAuth, SceneLoader sceneLoader)
    {
        this.loginUI = loginUI;
        this.authService = firebaseAuth;
        this.sceneLoader = sceneLoader;

        this.signup.onClick.AddListener(Signup);
        this.login.onClick.AddListener(Login);
    }

    private void Login()
    {
        this.Close();
        loginUI.Open();
    }

    private void Signup()
    {
        authService.Signup(email.text, username.text, password.text, repeatPassword.text,LoggedIn);
    }

    private void LoggedIn()
    {
        sceneLoader.LoadMenu();
    }
}