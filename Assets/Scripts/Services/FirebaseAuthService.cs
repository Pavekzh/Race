using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Firebase;
using Firebase.Auth;

public class FirebaseAuthService : MonoBehaviour
{
    public FirebaseUser User { get; private set; }

    private Messenger messenger;
    private FirebaseInit firebaseInit;

    public void Init(Messenger messenger, FirebaseInit firebaseInit)
    {
        this.messenger = messenger;
        this.firebaseInit = firebaseInit;
    }

    public void Login(string email, string password)
    {
        if (!firebaseInit.FirebaseReady)
        {
            messenger.ShowMessage("Error!", "Database is not ready", true, MessageType.Error);
            return;
        }

        StartCoroutine(DoLogin(email, password));
    }

    public void Signup(string email, string username, string password, string repeatPassword)
    {
        if (!firebaseInit.FirebaseReady)
        {
            messenger.ShowMessage("Error!", "Database is not ready", true, MessageType.Error);
            return;
        }
        StartCoroutine(DoSignUp(email, username, password, repeatPassword));

    }

    private IEnumerator DoLogin(string email, string password)
    {
        Task<AuthResult> loginTask = firebaseInit.Auth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {loginTask.Exception}");
            FirebaseException firebaseEx = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    messenger.ShowMessage("Error!", "Missing Email", true, MessageType.Error);
                    break;
                case AuthError.MissingPassword:
                    messenger.ShowMessage("Error!", "Missing Password", true, MessageType.Error);
                    break;
                case AuthError.WrongPassword:
                case AuthError.InvalidEmail:
                case AuthError.UserNotFound:
                    messenger.ShowMessage("Error!", "Wrong Password or Email ", true, MessageType.Error);
                    break;
            }

        }
        else
        {
            User = loginTask.Result.User;
            messenger.ShowMessage("Succes", $"Welcome {User.DisplayName}", true, MessageType.Succes);
        }
    }

    private IEnumerator DoSignUp(string email, string username, string password, string verifyPassword)
    {
        if (username == "" || username == null)
        {
            messenger.ShowMessage("Error!", "Missing username", true, MessageType.Error);
        }
        else if (password != verifyPassword)
        {
            messenger.ShowMessage("Error!", "Password does not match", true, MessageType.Error);
        }
        else
        {
            Task<AuthResult> registerTask = firebaseInit.Auth.CreateUserWithEmailAndPasswordAsync(email, password);

            yield return new WaitUntil(predicate: () => registerTask.IsCompleted);

            if (registerTask.Exception != null)
            {
                Debug.LogError(message: $"Failed to register task with {registerTask.Exception}");
                FirebaseException firebaseEx = registerTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        messenger.ShowMessage("Error!", "Missing Email", true, MessageType.Error);
                        break;
                    case AuthError.MissingPassword:
                        messenger.ShowMessage("Error!", "Missing Password", true, MessageType.Error);
                        break;
                    case AuthError.WeakPassword:
                        messenger.ShowMessage("Error!", "Weak Password", true, MessageType.Error);
                        break;
                    case AuthError.EmailAlreadyInUse:
                        messenger.ShowMessage("Error!", "Email already in use", true, MessageType.Error);
                        break;
                }
            }
            else
            {
                User = registerTask.Result.User;

                if (User != null)
                {
                    UserProfile profile = new UserProfile { DisplayName = username };

                    var ProfileTask = User.UpdateUserProfileAsync(profile);
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        Debug.LogError(message: $"Failed to register task with {ProfileTask.Exception}");

                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        messenger.ShowMessage("Error!", "Username set error", true, MessageType.Error);
                    }

                    messenger.ShowMessage("Succes!", $"You`re succesefully created account. Welcome {User.DisplayName}",true,MessageType.Succes);
                }
            }
        }
    }
}