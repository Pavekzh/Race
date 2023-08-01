using UnityEngine;

public class Wall : MonoBehaviour, IObstacle
{
    [SerializeField] private float resetedSpeed = 0;

    public virtual ObstacleType Type => ObstacleType.Wall;

    public float ResetedSpeed { get => resetedSpeed; }
}