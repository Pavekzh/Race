using UnityEngine;
using System.Collections;
using UnityEngine.VFX;

public class Nitro : MonoBehaviour, IObstacle
{
    [SerializeField] private GameObject view;
    [SerializeField] private float amount = 5;
    [SerializeField] private VisualEffect visualEffect;

    public ObstacleType Type => ObstacleType.Nitro;

    private bool isPicked = false;

    public float Extract()
    {
        float amount = 0;

        amount = this.amount;
        this.amount = 0;

        return amount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPicked)
        {
            view.SetActive(false);
            visualEffect.Play();
            isPicked = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isPicked)
        {
            this.amount = 0;
        }
    }

}