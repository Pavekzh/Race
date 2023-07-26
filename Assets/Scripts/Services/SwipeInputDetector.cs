using System;
using UnityEngine;

public class SwipeInputDetector : InputDetector
{
    public override event Action OnLeftInput;
    public override event Action OnRightInput;
    public override event Action OnNitroInput;


}