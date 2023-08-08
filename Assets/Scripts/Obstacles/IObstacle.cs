
public enum ObstacleType
{
    Oil,
    Hatch,
    Wall,
    Nitro
}

public interface IObstacle
{
    public ObstacleType Type { get; }
}

