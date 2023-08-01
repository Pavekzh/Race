using UnityEngine;
using System.Collections;
using UnityEngine.VFX;

public class Nitro : MonoBehaviour, IObstacle
{
    [SerializeField] private GameObject view;
    [SerializeField] private float amount = 5;
    [SerializeField] private VisualEffect visualEffect;

    public ObstacleType Type => ObstacleType.Nitro;

    public float Amount { get => amount; }

    private void OnTriggerEnter(Collider other)
    {
        view.SetActive(false);
        visualEffect.Play();
    }

}