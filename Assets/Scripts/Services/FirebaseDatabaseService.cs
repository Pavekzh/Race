using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;

public class FirebaseDatabaseService : MonoBehaviour
{    
    private const string UsersRoot = "UsersData";
    private const string UserAvatar = "avatar";

    //user data
    private string username;
    private int avatar;

    private bool userDataLoaded = false;
    private bool userDataLoading = false;

    //result callbacks
    private Action<int> getAvatarResult;

    //dependencies
    private Messenger messenger;
    private FirebaseDatabase database;
    private FirebaseAuth auth;

    public void Init(FirebaseDatabase firebaseDatabase,FirebaseAuth firebaseAuth)
    {
        this.database = firebaseDatabase;
        this.auth = firebaseAuth;
    }

    public string GetUsername()
    {
        if (!userDataLoaded)
            StartCoroutine(LoadUserData());

        return username;
    }

    public void GetAvatar(Action<int> getAvatarResult)
    {
        if (!userDataLoaded)
        {            
            this.getAvatarResult += getAvatarResult;
            StartCoroutine(LoadUserData());
        }

        getAvatarResult(avatar);
    }

    public void UpdateUsername(string username)
    {
        StartCoroutine(UploadUsername(username));
    }

    public void UpdateAvatar(int avatar)
    {
        StartCoroutine(UploadAvatar(avatar));
    }

    private IEnumerator UploadAvatar(int avatar)
    {
        Task uploadTask = database.RootReference.Child(UsersRoot).Child(auth.CurrentUser.UserId).Child(UserAvatar).SetValueAsync(avatar);
        yield return new WaitUntil(() => uploadTask.IsCompleted);

        if (uploadTask.Exception != null)
        {
            messenger.ShowMessage("Error!", "Updating avatar failed", true, MessageType.Error);
        }
        else
            Debug.Log("Succesfully uploaded avatar");
    }

    private IEnumerator UploadUsername(string username)
    {
        UserProfile updated = new UserProfile() { DisplayName = username };

        Task uploadTask = auth.CurrentUser.UpdateUserProfileAsync(updated);

        yield return new WaitUntil(() => uploadTask.IsCompleted);

        if(uploadTask.Exception != null)
        {
            messenger.ShowMessage("Error!", "Updating username failed", true, MessageType.Error);
        }
        else
            Debug.Log("Succesfully updated username");
    }

    private IEnumerator LoadUserData()
    {
        if (userDataLoading)
            yield break;

        userDataLoading = true;
        username = auth.CurrentUser.DisplayName;

        Task<DataSnapshot> loadTask = database.RootReference.Child(UsersRoot).Child(auth.CurrentUser.UserId).GetValueAsync();
        yield return new WaitUntil(() => loadTask.IsCompleted);
        DataSnapshot data = loadTask.Result;

        if (loadTask.Exception != null)
        {
            messenger.ShowMessage("Error!", "Loading user data failed", true, MessageType.Error);
            getAvatarResult = null;
            yield break;
        }
        else if(data.Value != null)
        {
            if(data.Child(UserAvatar).Value != null)
                avatar = int.Parse(data.Child(UserAvatar).Value.ToString());
        }

        userDataLoading = false;
        userDataLoaded = true;

        getAvatarResult?.Invoke(avatar);
        getAvatarResult = null;
    }
}