using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBootstrap : MonoBehaviour
{
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private MainMenuUI mainMenuUI;

    private void Awake()
    {
        mainMenuUI.Init(sceneLoader);
    }
}
