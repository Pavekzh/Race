using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    [SerializeField] private Button logoutButton;
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button garageButton;
    [Header("Car preview")]
    [SerializeField] private Transform[] cars;
    [SerializeField] private float rotateSpeed = 20;

    private Vector2 lastPointerPosition;

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

    private void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastPointerPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                float deltaX = (touch.position.x - lastPointerPosition.x) / Screen.width;
                lastPointerPosition = touch.position;

                RotateCar(-deltaX * rotateSpeed);
            }
        }
    }

    private void RotateCar(float delta)
    {
        foreach(Transform car in cars)
        {
            car.transform.Rotate(Vector3.up, delta);
        }
    }

    private void ShowSelectedCar(int index)
    {
        for(int i = 0; i< cars.Length; i++)
        {
            if (i != index)
                cars[i].gameObject.SetActive(false);
            else
                cars[i].gameObject.SetActive(true);
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
