using UnityEngine;


public  abstract class UIPanel : MonoBehaviour
{
    [SerializeField] private RectTransform panel;

    private bool isOpen => panel.gameObject.activeSelf;

    public virtual void Open()
    {
        if(!isOpen)
            panel.gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        if(isOpen)
            panel.gameObject.SetActive(false);
    }
}
