using UnityEngine;
using Firebase.Auth;
using Firebase.Database;

public class GarageBootstrap:MonoBehaviour
{
    [SerializeField] FirebaseDatabaseService databaseService;
    [SerializeField] SceneLoader scenLoader;
    [SerializeField] Garage garage;

    private void Awake()
    {
        databaseService.Init(FirebaseDatabase.DefaultInstance, FirebaseAuth.DefaultInstance);
        garage.Init(databaseService, scenLoader);
    }
}