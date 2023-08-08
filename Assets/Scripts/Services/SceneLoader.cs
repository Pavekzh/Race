using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private int loginScene = 0;
    [SerializeField] private int menuScene = 1;
    [SerializeField] private int gameScene = 2;
    [SerializeField] private int garageScene = 3;

    public void LoadMenu()
    {
        SceneManager.LoadSceneAsync(menuScene);
    }

    public void LoadGame()
    {
        SceneManager.LoadSceneAsync(gameScene);
    }

    public void LoadLogin()
    {
        SceneManager.LoadSceneAsync(loginScene);
    }

    public void LoadGarage()
    {
        SceneManager.LoadSceneAsync(garageScene);
    }
}
