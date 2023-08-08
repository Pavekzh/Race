using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LeaderboardRecord:MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Image avatar;    
    [SerializeField] private TMP_Text place;
    [SerializeField] private TMP_Text username;
    [SerializeField] private TMP_Text time;



    public void Show(int place,Sprite avatar, string username, float bestTimeMinutes,bool selected)
    {
        gameObject.SetActive(true);
        this.avatar.sprite = avatar;
        this.place.text = place.ToString();
        this.username.text = username;

        string time = new DateTime().AddMinutes(bestTimeMinutes).ToString("m:ss.ff");
        this.time.text = time;

        if (selected)
            background.color = selectedColor;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        background.color = defaultColor;
    }
}