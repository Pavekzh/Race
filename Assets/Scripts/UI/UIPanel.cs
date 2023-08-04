using UnityEngine;


public  abstract class UIPanel : MonoBehaviour
{
    [SerializeField] private RectTransform panel;

    public virtual void Open()
    {
        panel.gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        panel.gameObject.SetActive(false);
    }
}
