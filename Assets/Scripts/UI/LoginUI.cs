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
    private FirebaseAuthService firebaseAuth;

    public void Init(SignupUI signupUI, FirebaseAuthService firebaseAuth)
    {
        this.signupUI = signupUI;
        this.firebaseAuth = firebaseAuth;

        this.signup.onClick.AddListener(Signup);
        this.login.onClick.AddListener(Login);
    }

    private void Login()
    {
        firebaseAuth.Login(email.text, password.text);
    }

    private void Signup()
    {
        this.Close();
        signupUI.Open();
    }
}