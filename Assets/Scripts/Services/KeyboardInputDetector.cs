using System;
using UnityEngine;

public class KeyboardInputDetector:InputDetector
{
    [SerializeField] private KeyCode leftKey = KeyCode.A;
    [SerializeField] private KeyCode rightKey = KeyCode.D;
    [SerializeField] private KeyCode nitroKey = KeyCode.Space;

    public override event Action OnLeftInput;
    public override event Action OnRightInput;
    public override event Action OnNitroInput;

    private void Update()
    {
        if (Input.GetKeyDown(leftKey))
            OnLeftInput?.Invoke();
        if (Input.GetKeyDown(rightKey))
            OnRightInput?.Invoke();
        if (Input.GetKeyDown(nitroKey))
            OnNitroInput?.Invoke();
    }
}