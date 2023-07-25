using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button startGameButton;

    private SceneLoader sceneLoader;

    public void Init(SceneLoader sceneLoader)
    {
        this.sceneLoader = sceneLoader;

        this.startGameButton.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        sceneLoader.LoadGame();
    }

}
