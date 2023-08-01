using UnityEngine;

public class Hatch : Wall, IObstacle
{
    [SerializeField] private float throwBack = 10;

    public override ObstacleType Type => ObstacleType.Hatch;

    public float ThrowBack { get => throwBack; }
}