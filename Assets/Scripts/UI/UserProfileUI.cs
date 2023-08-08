using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UserProfileUI:UIPanel
{
    [Header("Curtain")]
    [SerializeField] private TMP_Text usernameCurtain;
    [SerializeField] private Image avatarCurtain;
    [SerializeField] private Button edit;
    [Header("Editing")]
    [SerializeField] private TMP_InputField username;
    [SerializeField] private Button[] avatars;
    [SerializeField] private Color defaultAvatarColor;
    [SerializeField] private Color selectedAvatarColor;
    [SerializeField] private Button save;

    private int selectedAvatar;
    private int SelectedAvatar
    {
        get => selectedAvatar;
        set
        {
            avatars[selectedAvatar].targetGraphic.color = defaultAvatarColor;
            selectedAvatar = value;
            avatars[selectedAvatar].targetGraphic.color = selectedAvatarColor;
            dataChanged = true;
        }
    }

    private bool dataChanged;

    private FirebaseDatabaseService databaseService;
    private UserAvatars userAvatars;

    public void Init(FirebaseDatabaseService databaseService,UserAvatars userAvatars)
    {
        this.databaseService = databaseService;
        this.userAvatars = userAvatars;

        InitCurtain(databaseService, userAvatars);
        InitEditing();
    }

    private void InitCurtain(FirebaseDatabaseService databaseService, UserAvatars userAvatars)
    {
        edit.onClick.AddListener(Open);
        usernameCurtain.text = databaseService.GetUsername();
        databaseService.GetAvatar(avatar => avatarCurtain.sprite = userAvatars.GetAvatar(avatar));
    }

    private void InitEditing()
    {
        save.onClick.AddListener(Close);
        username.onValueChanged.AddListener(text => dataChanged = true);

        for (int i = 0; i < avatars.Length; i++)
        {
            int index = i;
            avatars[i].transform.GetChild(0).GetComponent<Image>().sprite = userAvatars.GetAvatar(i);
            avatars[i].onClick.AddListener(() => SelectedAvatar = index);
        }

        username.text = databaseService.GetUsername();
        databaseService.GetAvatar(avatar => SelectedAvatar = avatar);
        dataChanged = false;
    }

    public override void Close()
    {
        base.Close();

        if (dataChanged)
            UpdateData();
    }

    private void UpdateData()
    {
        avatarCurtain.sprite = userAvatars.GetAvatar(SelectedAvatar);
        usernameCurtain.text = username.text;

        databaseService.UpdateAvatar(SelectedAvatar);
        databaseService.UpdateUsername(username.text);        
        dataChanged = false;
    }

}