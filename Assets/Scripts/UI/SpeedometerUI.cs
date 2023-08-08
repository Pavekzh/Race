using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public enum ValueMode
{
    Absolute,
    Percent
}

public class SpeedometerUI : MonoBehaviour
{
    [SerializeField] private RectTransform arrow;
    [SerializeField] private float minAngle = 93;
    [SerializeField] private float maxAngle = -153;
    [SerializeField] private float maxAngleNitro = -173;
    [SerializeField] private float maxNitroSpeedPercent = 1.5f;
    [SerializeField] private float smoothChangeFactor = 0.5f;

    private float angleDelta { get => maxAngle - minAngle; }
    private float nitroAngleDelta { get => maxAngleNitro - maxAngle; }

    private float currentAngle = 0;

    private float maxValue = 100;

    private void Awake()
    {
        currentAngle = minAngle;
    }

    public void SetMaxValue(float maxValue)
    {
        this.maxValue = maxValue;
    }

    public void UpdateValue(float value, ValueMode mode = ValueMode.Percent)
    {
        if (mode == ValueMode.Percent)
            UpdateValue(value);
        else
            UpdateValue(value / maxValue);
    }

    private void UpdateValue(float percent)
    {
        float resultAngle;
        if (percent <= 1)
        {
            resultAngle = minAngle + percent * angleDelta;
        }
        else
        {
            Mathf.Clamp(percent, 1, maxNitroSpeedPercent);
            percent = (percent - 1) / (maxNitroSpeedPercent - 1);
            resultAngle = maxAngle + percent * nitroAngleDelta;
        }
        currentAngle = Mathf.Lerp(currentAngle, resultAngle, 1 / smoothChangeFactor * Time.deltaTime);
        arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, currentAngle));
    }

}
