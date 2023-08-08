using UnityEngine;
using TMPro;
using UnityEngine.UI;


public enum MessageType
{
    Message,
    Warning,
    Succes,
    Error
}
public class Messenger:UIPanel
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text message;
    [SerializeField] private Button ok;
    [Header("Icons")]
    [SerializeField] private Sprite messageIcon;
    [SerializeField] private Sprite warningIcon;    
    [SerializeField] private Sprite succesIcon;
    [SerializeField] private Sprite errorIcon;

    private bool isClosable;

    private void Awake()
    {
        ok.onClick.AddListener(Close);
    }

    public override void Close()
    {
        base.Close();
    }

    public override void Open()
    {
        base.Open();

        if (isClosable)
            ok.gameObject.SetActive(true);
        else
            ok.gameObject.SetActive(false);
    }

    public void ShowMessage(string title, string message, bool closable = true,MessageType type = MessageType.Message)
    {
        isClosable = closable;

        this.title.text = title;
        this.message.text = message;
        SetIcon(type);
        Open();
    }

    private void SetIcon(MessageType messageType)
    {
        if(icon != null)
        {
            switch (messageType)
            {
                case MessageType.Message:
                    icon.sprite = messageIcon;
                    break;
                case MessageType.Warning:
                    icon.sprite = warningIcon;
                    break;
                case MessageType.Succes:
                    icon.sprite = succesIcon;
                    break;
                case MessageType.Error:
                    icon.sprite = errorIcon;
                    break;

            }                    
        }
    }
}