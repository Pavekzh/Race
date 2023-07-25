using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private int menuScene = 0;    
    [SerializeField] private int gameScene = 1;
    [SerializeField] private int loginScene = 2;

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
}
