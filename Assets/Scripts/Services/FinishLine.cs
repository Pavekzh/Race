using UnityEngine;

public class FinishLine:MonoBehaviour
{
    private RaceFinish raceFinish;

    public void Init(RaceFinish raceFinish)
    {
        this.raceFinish = raceFinish;
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        raceFinish.RegisterFinish(player);
    }
}
