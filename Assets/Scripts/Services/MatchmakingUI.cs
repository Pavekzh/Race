using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class MatchmakingUI:NetworkBehaviour
{
    [SerializeField] private GameObject searchingScreen;
    [SerializeField] private GameObject opponentsScreen;
    [SerializeField] private float showOpponentsScreenTime = 2;
    [SerializeField] private Image avatarSelf;
    [SerializeField] private TMP_Text usernameSelf;
    [SerializeField] private Image avatarOpponent;
    [SerializeField] private TMP_Text usernameOpponent;

    private UserData selfData;
    private int playerJoined = 0;
    private bool opponentDataLoaded = false;

    public event System.Action OpponentsScreenShown;

    private UserAvatars userAvatars;

    public void Init(FirebaseDatabaseService databaseService,UserAvatars userAvatars)
    {
        this.userAvatars = userAvatars;

        databaseService.GetUserData(SetSelfData);
        searchingScreen.SetActive(true);
    }

    public void Hide()
    {
        opponentsScreen.SetActive(false);
        searchingScreen.SetActive(false);
    }

    public void PlayerJoined()
    {
        playerJoined++;
        if(playerJoined == 1)
        {

        }
        else if(playerJoined == 2)
        {
            RPC_SendUserDataToOpponent(selfData.Username,selfData.Avatar);
            StartCoroutine(ShowOpponentsScreen());
        }        

    }

    private IEnumerator ShowOpponentsScreen()
    {
        searchingScreen.SetActive(false);
        opponentsScreen.SetActive(true);

        yield return new WaitUntil(() => opponentDataLoaded);

        yield return new WaitForSeconds(showOpponentsScreenTime);
        OpponentsScreenShown?.Invoke();
    }

    private void SetSelfData(UserData data)
    {
        avatarSelf.sprite = userAvatars.GetAvatar(data.Avatar);
        usernameSelf.text = data.Username;
        selfData = data;
    }

    [Rpc(sources: RpcSources.All,targets: RpcTargets.All,InvokeLocal = false)]
    private void RPC_SendUserDataToOpponent(string username, int avatar)
    {
        avatarOpponent.sprite = userAvatars.GetAvatar(avatar);
        usernameOpponent.text = username;
        opponentDataLoaded = true;
    }
}
