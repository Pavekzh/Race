using System;
using UnityEngine;

public class KeyboardInputDetector:InputDetector
{
    [SerializeField] private KeyCode leftKey = KeyCode.A;
    [SerializeField] private KeyCode rightKey = KeyCode.D;
    [SerializeField] private KeyCode nitroKey = KeyCode.Space;

    private void Update()
    {
        if (Input.GetKeyDown(leftKey))
            isLeftInput = true;
        if (Input.GetKeyDown(rightKey))
            isRightInput = true;
        if (Input.GetKeyDown(nitroKey))
            isNitroInput = true;
    }


}