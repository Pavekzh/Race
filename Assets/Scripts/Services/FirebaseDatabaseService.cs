using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseDatabaseService : MonoBehaviour
{    
    private const string UsersRoot = "UsersData";
    private const string UserName = "username";
    private const string UserAvatar = "avatar";
    private const string UserCar = "car";
    private const string UserBestTime = "bestTime";


    //user data
    UserData userData;

    private bool userDataLoaded = false;
    private bool userDataLoading = false;

    //result callbacks
    private Action<UserData> getUserDataResult;
    private Action<int> getAvatarResult;
    private Action<int> getCarResult;
    private Action<float> getBestTimeResult;
    private Action<IEnumerable<UserData>,int> getLeaderboardResult;

    private Action setSelectedCarSucceed;

    //dependencies
    private Messenger messenger;
    private FirebaseDatabase database;
    private FirebaseAuth auth;

    public void Init(FirebaseDatabase firebaseDatabase,FirebaseAuth firebaseAuth)
    {
        this.database = firebaseDatabase;
        this.auth = firebaseAuth;
    }

    public void GetUserData(Action<UserData> OnResult)
    {
        if (!userDataLoaded)
        {
            this.getUserDataResult += OnResult;
            StartCoroutine(LoadUserData());
        }
        else
            OnResult(userData);

    }

    public string GetUsername()
    {
        if (!userDataLoaded)
            StartCoroutine(LoadUserData());

        return userData.Username;
    }

    public void GetSelectedCar(Action<int> OnResult)
    {
        if (!userDataLoaded)
        {
            this.getCarResult += OnResult;
            StartCoroutine(LoadUserData());
        }
        else
            OnResult(userData.SelectedCar);
    }

    public void GetBestTime(Action<float> OnResult)
    {
        if (!userDataLoaded)
        {
            this.getBestTimeResult += OnResult;
            StartCoroutine(LoadUserData());
        }
        else
            OnResult(userData.BestTime);

    }

    public void GetAvatar(Action<int> OnResult)
    {
        if (!userDataLoaded)
        {            
            this.getAvatarResult += OnResult;
            StartCoroutine(LoadUserData());
        }
        else
            OnResult(userData.Avatar);
    }    
    
    public void LoadLeaderbord(int numberOfFirst,Action<IEnumerable<UserData>,int> OnResult)
    {
        getLeaderboardResult += OnResult;
        StartCoroutine(LoadLeaderboard(numberOfFirst));
    }


    public void UpdateBestTime(float bestTime)
    {
        StartCoroutine(UploadBestTime(bestTime));
    }

    public void UpdateUsername(string username)
    {
        StartCoroutine(UploadUsername(username));
    }

    public void UpdateAvatar(int avatar)
    {
        StartCoroutine(UploadAvatar(avatar));
    }

    public void UpdateCar(int car,Action onSucceed = null)
    {
        if (onSucceed != null)
            setSelectedCarSucceed += onSucceed;

        StartCoroutine(UploadSelectedCar(car));
    }


    private IEnumerator UploadSelectedCar(int car)
    {
        Task uploadTask = database.RootReference.Child(UsersRoot).Child(auth.CurrentUser.UserId).Child(UserCar).SetValueAsync(car);
        yield return new WaitUntil(() => uploadTask.IsCompleted);

        if (uploadTask.Exception != null)
        {
            messenger.ShowMessage("Error!", "Updating best time failed", true, MessageType.Error);
        }
        else
        {
            userData.BestTime = car;

            setSelectedCarSucceed?.Invoke();
            setSelectedCarSucceed = null;
        }
    }

    private IEnumerator UploadBestTime(float bestTime)
    {
        Task uploadTask = database.RootReference.Child(UsersRoot).Child(auth.CurrentUser.UserId).Child(UserBestTime).SetValueAsync(bestTime);
        yield return new WaitUntil(() => uploadTask.IsCompleted);

        if (uploadTask.Exception != null)
        {
            messenger.ShowMessage("Error!", "Updating best time failed", true, MessageType.Error);
        }
        else
            userData.BestTime = bestTime;
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
            userData.Avatar = avatar;
    }

    private IEnumerator UploadUsername(string username)
    {
        UserProfile updated = new UserProfile() { DisplayName = username };

        Task updateTask = auth.CurrentUser.UpdateUserProfileAsync(updated);
        Task uploadTask = database.RootReference.Child(UsersRoot).Child(auth.CurrentUser.UserId).Child(UserName).SetValueAsync(username);

        yield return new WaitUntil(() => updateTask.IsCompleted && uploadTask.IsCompleted);

        if (updateTask.Exception != null || uploadTask.Exception != null)
        {
            messenger.ShowMessage("Error!", "Updating username failed", true, MessageType.Error);
        }
        else
            userData.Username = username;

    }

    private IEnumerator LoadUserData()
    {
        if (userDataLoading)
            yield break;

        userDataLoading = true;
        userData.Username = auth.CurrentUser.DisplayName;

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
                userData.Avatar = int.Parse(data.Child(UserAvatar).Value.ToString());
            if (data.Child(UserBestTime).Value != null)
                userData.BestTime = float.Parse(data.Child(UserBestTime).Value.ToString());
            if (data.Child(UserCar).Value != null)
                userData.SelectedCar = int.Parse(data.Child(UserCar).Value.ToString());
        }

        userDataLoading = false;
        userDataLoaded = true;

        getAvatarResult?.Invoke(userData.Avatar);
        getBestTimeResult?.Invoke(userData.BestTime);
        getCarResult?.Invoke(userData.SelectedCar);
        getUserDataResult?.Invoke(userData);

        getUserDataResult = null;        
        getCarResult = null;
        getBestTimeResult = null;
        getAvatarResult = null;
    }

    private IEnumerator LoadLeaderboard(int numberOfFirst)
    {
        Task<DataSnapshot> loadTask = database.RootReference.Child(UsersRoot).OrderByChild(UserBestTime).GetValueAsync();

        yield return new WaitUntil(() => loadTask.IsCompleted);
        DataSnapshot data = loadTask.Result;

        if(loadTask.Exception != null)
        {
            messenger.ShowMessage("Error!", "Loading leaderboard failed", true, MessageType.Error);
            yield break;
        }
        else
        {
            List<UserData> leaderboard = new List<UserData>();
            int userPosition = -1;

            int index = 0;
            foreach(DataSnapshot userSnapshot in data.Children)
            {
                if (userPosition != -1 && index > 9)
                    break;

                if (index < numberOfFirst)
                {

                    string username = userSnapshot.Child(UserName).Value.ToString();
                    int avatar = 0;
                    float bestTime = 0;

                    if (userSnapshot.Child(UserBestTime).Value != null)
                        bestTime = float.Parse(userSnapshot.Child(UserBestTime).Value.ToString());
                    if (userSnapshot.Child(UserAvatar).Value != null)
                        avatar = int.Parse(userSnapshot.Child(UserAvatar).Value.ToString());

                    UserData userData = new UserData() { Avatar = avatar, Username = username, BestTime = bestTime };

                    leaderboard.Add(userData);
                }
                if (userSnapshot.Key == auth.CurrentUser.UserId)
                    userPosition = index;

                index++;
            }
            getLeaderboardResult?.Invoke(leaderboard,userPosition+1);
            getLeaderboardResult = null;
        }
    }
}