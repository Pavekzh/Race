using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    [SerializeField] private Button logoutButton;
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button garageButton;
    [Header("Car preview")]
    [SerializeField] private GameObject[] cars;



    private SceneLoader sceneLoader;
    private FirebaseDatabaseService databaseService;
    private FirebaseAuthService authService;

    public void Init(SceneLoader sceneLoader,FirebaseDatabaseService databaseService,FirebaseAuthService authService)
    {
        this.sceneLoader = sceneLoader;
        this.databaseService = databaseService;
        this.authService = authService;

        databaseService.GetSelectedCar(ShowSelectedCar);

        this.startGameButton.onClick.AddListener(StartGame);
        this.garageButton.onClick.AddListener(EnterGarage);
        this.exitButton.onClick.AddListener(Exit);
        this.logoutButton.onClick.AddListener(Logout);
    }

    private void ShowSelectedCar(int index)
    {
        for(int i = 0; i< cars.Length; i++)
        {
            if (i != index)
                cars[i].SetActive(false);
            else
                cars[i].SetActive(true);
        }
    }

    private void StartGame()
    {
        sceneLoader.LoadGame();
    }

    private void EnterGarage()
    {
        sceneLoader.LoadGarage();
    }

    private void Exit()
    {
        Application.Quit();
    }
    
    private void Logout()
    {
        authService.LogOut();

        sceneLoader.LoadLogin();
    }
}
