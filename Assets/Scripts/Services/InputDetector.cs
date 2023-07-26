using UnityEngine;
using System;

public abstract class InputDetector:MonoBehaviour
{
    public abstract event Action OnLeftInput;
    public abstract event Action OnRightInput;
    public abstract event Action OnNitroInput;

    public virtual void Init()
    {

    }
}