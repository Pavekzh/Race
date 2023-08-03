using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InGameUI : MonoBehaviour
{
    [SerializeField] private Button nitroButton;
    [SerializeField] private SpeedometerUI speedometer;
    [SerializeField] private Image oilIndicator;
    [SerializeField] private Image nitro;
    [SerializeField] private TMP_Text time;
    [SerializeField] private TMP_Text position;

    public event Action NitroClick
    {
        add => nitroButton.onClick.AddListener(new UnityEngine.Events.UnityAction(value));
        remove => nitroButton.onClick.RemoveListener(new UnityEngine.Events.UnityAction(value));
    }

    public void SetOilIndicator(bool enabled)
    {
        oilIndicator.gameObject.SetActive(enabled);
    }

    public void UpdateSpeed(float percent)
    {
        speedometer.UpdateValue(percent);
    }

    public void SetEnabledNitroButton(bool isEnabled)
    {
        nitroButton.gameObject.SetActive(isEnabled);
    }

    public void UpdateNitro(float percent)
    {
        nitro.fillAmount = percent;
    }

    public void UpdateTimer(float time)
    {
        this.time.text = time.ToString(".00");
    }


    public void UpdatePlayerPosition(int position,int players)
    {
        this.position.text = $"{position}/{players}";
    }
}
