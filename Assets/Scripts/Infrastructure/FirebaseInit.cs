using System;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using System.Threading.Tasks;

public class FirebaseInit : MonoBehaviour
{
    public FirebaseApp App { get; private set; }
    public FirebaseAuth Auth { get; private set; }

    public bool FirebaseReady { get; set; } = false;

    public async Task Init()
    {
        await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            DependencyStatus dependencyStatus = task.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                App = FirebaseApp.DefaultInstance;
                Auth = FirebaseAuth.DefaultInstance; 
                FirebaseReady = true;
                Debug.Log("Firebase ready");
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                FirebaseReady = false;
            }
        });
    }
}