using UnityEngine;
using System.Collections;

public class Nitro : MonoBehaviour, IObstacle
{
    [SerializeField] private float amount = 5;
    [SerializeField] private float lifetimeAfterPick = 1;

    public ObstacleType Type => ObstacleType.Nitro;

    public float Amount { get => amount; }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(lifetimeAfterPick);
        Destroy(gameObject);
    }
}