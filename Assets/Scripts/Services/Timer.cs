using UnityEngine;
using Fusion;

public class Timer:NetworkBehaviour
{
    private float raceStartedTime;

    [Networked(OnChanged = nameof(RaceTimeChanged))]private float raceTime { get; set; }

    public float RaceTime { get => raceTime; }

    private bool timerStarted = false;

    public void StartTimer()
    {
        if (Runner.IsServer)
        {
            timerStarted = true;
            raceTime = 0;
        }
    }

    private void Update()
    {
        if (timerStarted && Runner.IsServer)
        {
            raceTime = raceTime + Time.deltaTime;
        }

    }

    private static void RaceTimeChanged(Changed<Timer> timer)
    {
        //Debug.LogError(timer.Behaviour.RaceTime);
    }

}