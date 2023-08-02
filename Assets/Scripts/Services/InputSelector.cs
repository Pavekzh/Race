using System;
using UnityEngine;

class InputSelector:InputDetector
{
    [SerializeField] private KeyboardInputDetector keyboardInput;
    [SerializeField] private SwipeInputDetector swipeInput;

    private InputDetector selected;

    public override event Action OnLeftInput { add => selected.OnLeftInput += value; remove => selected.OnLeftInput -= value; }
    public override event Action OnRightInput { add => selected.OnRightInput += value; remove => selected.OnRightInput -= value; }
    public override event Action OnNitroInput { add => selected.OnNitroInput += value; remove => selected.OnNitroInput -= value; }

    public override void Init()
    {
#if UNITY_STANDALONE || UNITY_EDITOR 
        selected = keyboardInput;
        swipeInput.SetEnabled(false);
#elif UNITY_ANDROID || UNITY_IOS 
        selected = swipeInput;
        swipeInput.SetEnabled(true);
        keyboardInput.enabled = false;
#endif
    }
}