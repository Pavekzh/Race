using Fusion;
using UnityEngine;

public class SwitchInputDetector : InputDetector
{
    [SerializeField] private SwipeInputDetector swipeInput;
    [SerializeField] private KeyboardInputDetector keyboardInput;

    private InputDetector selected;

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

    public override void OnInput(NetworkRunner runner, NetworkInput input)
    {
        selected.OnInput(runner, input);
    }
}