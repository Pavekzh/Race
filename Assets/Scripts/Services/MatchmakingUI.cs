using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchmakingUI:MonoBehaviour
{
    [SerializeField] private GameObject searchingScreen;
    [SerializeField] private GameObject opponentsScreen;
    [SerializeField] private float showOpponentsScreenTime = 2;
    [SerializeField] private Image avatarSelf;
    [SerializeField] private TMP_Text usernameSelf;
    [SerializeField] private Image avatarOpponent;
    [SerializeField] private TMP_Text usernameOpponent;

    private int players = 0;

    private GameStarter gameStarter;

    public void Init(GameStarter gameStarter)
    {
        this.gameStarter = gameStarter;

        searchingScreen.SetActive(true);
    }

    public void PlayerJoined()
    {
        players++;
        if(players == 1)
        {
            Debug.LogWarning("Player data should be loaded from database");
            avatarSelf.color = Color.green;
            usernameSelf.text = "Player1";
        }
        else if(players == 2)
        {            
            Debug.LogWarning("Opponent player data should be loaded from database");
            avatarOpponent.color = Color.red;
            usernameOpponent.text = "Player2";
            StartCoroutine(ShowOpponentsScreen());
        }        

    }

    private IEnumerator ShowOpponentsScreen()
    {
        searchingScreen.SetActive(false);
        opponentsScreen.SetActive(true);
        yield return new WaitForSeconds(showOpponentsScreenTime);
        opponentsScreen.SetActive(false);
        gameStarter.OpponentsScreenShown();
    }
}
