using System;
using UnityEngine;
using UnityEngine.UI;

public class SwipeInputDetector : InputDetector
{
    [SerializeField] [Range(0, 1)] private float swipeTreshold = 0.05f;
    [SerializeField] private Button nitroButton;

    public override event Action OnLeftInput;
    public override event Action OnRightInput;
    public override event Action OnNitroInput;

    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private bool isSwipeHandled;

    private void Awake()
    {
        nitroButton.onClick.AddListener(() => OnNitroInput?.Invoke());
    }

    public void SetEnabled(bool enabled)
    {
        this.enabled = enabled;
        nitroButton.gameObject.SetActive(enabled);
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
                            OnRightInput?.Invoke();
                        else
                            OnLeftInput?.Invoke();
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