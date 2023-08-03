using System;
using UnityEngine;
using UnityEngine.UI;

public class SwipeInputDetector : InputDetector
{
    [SerializeField] [Range(0, 1)] private float swipeTreshold = 0.05f;

    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private bool isSwipeHandled;

    private InGameUI inGameUI;

    public void Init(InGameUI inGameUI)
    {
        this.inGameUI = inGameUI;
        this.inGameUI.NitroClick += () => isNitroInput = true;
    }


    public void SetEnabled(bool enabled)
    {
        this.enabled = enabled;
        inGameUI.SetEnabledNitroButton(enabled);
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
                touchEndPos = touch.position;
                isSwipeHandled = false;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                if (!isSwipeHandled)
                {
                    touchEndPos = touch.position;
                    bool isRighSwipe;

                    if(CheckSwipe(out isRighSwipe))
                    {
                        if (isRighSwipe)
                            isRightInput = true;
                        else
                            isLeftInput = true;

                        isSwipeHandled = true;
                    }

                }
            }
        }
    }

    private bool CheckSwipe(out bool isRightSwipe)
    {
        float deltaX = MathF.Abs(touchEndPos.x - touchStartPos.x) / Screen.width;

        if (deltaX > swipeTreshold)
        {
            if (touchEndPos.x > touchStartPos.x)
                isRightSwipe = true;
            else
                isRightSwipe = false;

            return true;
        }

        isRightSwipe = false;
        return false;

    }
}