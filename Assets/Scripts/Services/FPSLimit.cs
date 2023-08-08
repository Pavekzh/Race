using UnityEngine;

class FPSLimit : MonoBehaviour
{
    [SerializeField] private int limit = 60;

    private void Awake()
    {
        Application.targetFrameRate = limit;
    }
}