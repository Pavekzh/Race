using UnityEngine;

public class Oil : MonoBehaviour, IObstacle
{
    [SerializeField] private float effectTime = 5;
    [SerializeField] private float speedMultiplier = 0.60f;

    public ObstacleType Type => ObstacleType.Oil;

    public float EffectTime { get => effectTime; set => effectTime = value; }
    public float SpeedMultiplier { get => speedMultiplier; }
}