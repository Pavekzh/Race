using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard:UIPanel
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button openButton;
    [SerializeField] private LeaderboardRecord[] records;
    [SerializeField] private LeaderboardRecord bottomRecord;

    private UserData userData;

    private FirebaseDatabaseService databaseService;
    private UserAvatars userAvatars;

    public void Init(FirebaseDatabaseService databaseService,UserAvatars userAvatars)
    {
        this.databaseService = databaseService;
        this.userAvatars = userAvatars;

        openButton.onClick.AddListener(Open);
        closeButton.onClick.AddListener(Close);

        HideRecords();
    }

    public override void Open()
    {
        base.Open();
        LoadLeaderboard();
    }

    public override void Close()
    {
        base.Close();
        HideRecords();
    }

    private void HideRecords()
    {
        foreach (LeaderboardRecord record in records)
            record.Hide();
        bottomRecord.Hide();
    }

    private void LoadLeaderboard()
    {
        databaseService.LoadLeaderbord(records.Length, SetupLeaderboard);
        databaseService.GetUserData(userData => this.userData = userData);
    }

    private void SetupLeaderboard(IEnumerable<UserData> leaderboard,int playerPosition)
    {
        int index = 0;
        foreach(UserData userdata in leaderboard)
        {
            bool isPlayer = false;
            if (index + 1 == playerPosition)
                isPlayer = true;

            records[index].Show(index + 1,userAvatars.GetAvatar(userdata.Avatar), userdata.Username, userdata.BestTime, isPlayer);
            index++;
        }

        if (playerPosition > records.Length)
            bottomRecord.Show(playerPosition,userAvatars.GetAvatar(userData.Avatar), userData.Username, userData.BestTime, false);
    }
}