using System.Collections;
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

    public event System.Action OpponentsScreenShown;

    public void Init()
    {
        searchingScreen.SetActive(true);
    }

    public void Hide()
    {
        opponentsScreen.SetActive(false);
        searchingScreen.SetActive(false);
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
        OpponentsScreenShown?.Invoke();
    }
}
