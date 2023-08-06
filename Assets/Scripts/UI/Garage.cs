using UnityEngine;
using UnityEngine.UI;

public class Garage : MonoBehaviour
{
    [SerializeField] private Button saveButton;
    [SerializeField] private Button exitButton;
    [Header("Scroll")]
    [SerializeField] private float scrollSpeed;
    [SerializeField] private Transform[] cars;
    [SerializeField] private Vector3 centralPoint;

    private Vector2 lastPointerPosition;
    private int selected = 0;

    private FirebaseDatabaseService databaseService;
    private SceneLoader sceneLoader;

    public void Init(FirebaseDatabaseService databaseService,SceneLoader sceneLoader)
    {
        this.databaseService = databaseService;
        this.sceneLoader = sceneLoader;

        saveButton.onClick.AddListener(SaveSelected);
        exitButton.onClick.AddListener(ExitGarage);

        databaseService.GetSelectedCar(ShowSelected);
    }

    private void Update()
    {
        if(Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                lastPointerPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                float deltaX = (touch.position.x - lastPointerPosition.x) / Screen.width;
                lastPointerPosition = touch.position;

                ScrollCars(deltaX * scrollSpeed);
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                CenterCars();

            }
        }
    }

    private void ExitGarage()
    {
        sceneLoader.LoadMenu();
    }

    private void SaveSelected()
    {
        databaseService.UpdateCar(selected,ExitGarage);
    }

    private void ShowSelected(int index)
    {
        selected = index;
        float delta = centralPoint.x - cars[index].position.x;
        ScrollCars(delta);
    }

    private void ScrollCars(float delta)
    {
        foreach(Transform car in cars)
        {
            car.position += Vector3.right * delta;
        }
    }

    private void CenterCars()
    {  
        float deltaToNearest = centralPoint.x - cars[0].position.x;
        float minDistance = Mathf.Abs(deltaToNearest);
        selected = 0;

        for(int i = 1; i < cars.Length; i++)
        {
            float delta = centralPoint.x - cars[i].position.x;
            float distance = Mathf.Abs(delta);

            if (distance < minDistance)
            {
                minDistance = distance;
                deltaToNearest = delta;
                selected = i;
            }
        }
        ScrollCars(deltaToNearest);

    }
}