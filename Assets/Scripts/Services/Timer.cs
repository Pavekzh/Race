using UnityEngine;
using Fusion;

public class Timer:NetworkBehaviour
{
    private float raceStartedTime;

    [Networked(OnChanged = nameof(RaceTimeChanged))]private float raceTime { get; set; }

    public float RaceTime { get => raceTime; }

    private bool timerStarted = false;

    private InGameUI inGameUI;

    public void Init(InGameUI inGameUI)
    {
        this.inGameUI = inGameUI;
    }

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
        timer.Behaviour.inGameUI.UpdateTimer(timer.Behaviour.raceTime);
    }

}