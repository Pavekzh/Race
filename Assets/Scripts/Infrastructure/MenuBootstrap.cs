using UnityEngine;

public class MenuBootstrap : MonoBehaviour
{
    [SerializeField] private SceneLoader sceneLoader;
    [Header("UI")]
    [SerializeField] private Messenger messenger;
    [SerializeField] private MainMenuUI mainMenuUI;


    private void Awake()
    {
        InitMainMenuUI();

    }

    private void InitMainMenuUI()
    {
        mainMenuUI.Init(sceneLoader);
    }
}
