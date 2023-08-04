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
    private FirebaseAuthService firebaseAuth;

    public void Init(LoginUI loginUI, FirebaseAuthService firebaseAuth)
    {
        this.loginUI = loginUI;
        this.firebaseAuth = firebaseAuth;

        this.signup.onClick.AddListener(Signup);
        this.login.onClick.AddListener(Login);
    }

    private void Login()
    {
        loginUI.Open();
    }

    private void Signup()
    {
        firebaseAuth.Signup(email.text, username.text, password.text, repeatPassword.text);
    }
}