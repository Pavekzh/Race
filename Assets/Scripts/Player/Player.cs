using UnityEngine;
using Fusion;

public class Player : NetworkBehaviour
{    
    public Vector3 MoveDirection { get => Vector3.forward; }

    [SerializeField] private float changingLaneSpeed = 1;
    [Header("Obstacles")]
    [SerializeField] private string obstacleTag = "Obstacle";
    [Header("Nitro")]
    [SerializeField] private float nitroSpeedMultiplier = 1.3f;
    [SerializeField] private float nitroAccelerationMultiplier = 3f;
    [SerializeField] private float maxNitro = 100;
    [SerializeField] private float nitro = 0;
    [SerializeField] private float nitroConsumption = 30;


    public float ChangingLaneSpeed { get => changingLaneSpeed; }
    public string ObstacleTag { get => obstacleTag; }
    public int CurrentLane { get; set; }
    public float Nitro 
    {
        get => nitro;
        set
        {
            nitro = Mathf.Clamp(value, 0, maxNitro);
        }
    }
    public float NitroSpeedMultiplier { get => nitroSpeedMultiplier; }
    public float NitroAccelerationMultiplier { get => nitroAccelerationMultiplier; }
    public float NitroConsumption { get => nitroConsumption; }
    public float SlowEffectTime { get; set; }
    public float SlowEffectMultiplier { get; set; }


    private bool isRaceStarted;

    private StateMachine<BaseState> stateMachine;

    public Rigidbody Rigidbody { get; private set; }
    public NetworkCarController CC { get; private set; }
    public WorldGenerator WorldGenerator { get; private set; }

    public void Init(InputDetector inputDetector,WorldGenerator worldGenerator,int lane)
    {
        this.CurrentLane = lane;
        this.WorldGenerator = worldGenerator;
        inputDetector.OnLeftInput += OnLeftInput;
        inputDetector.OnRightInput += OnRightInput;
        inputDetector.OnNitroInput += OnNitroInput;

        Rigidbody = GetComponent<Rigidbody>();
        CC = GetComponent<NetworkCarController>();

        stateMachine = new StateMachine<BaseState>();

        stateMachine.AddState(new DefaultState(stateMachine, this));
        stateMachine.AddState(new OilState(stateMachine, this));
        stateMachine.AddState(new NitroState(stateMachine, this));
        stateMachine.AddState(new NitroOilState(stateMachine, this));

        stateMachine.InitState<DefaultState>();
    }

    public void StartRace()
    {
        if (HasStateAuthority)
        {
            isRaceStarted = true;
        }

    }

    public override void FixedUpdateNetwork()
    {
        if (isRaceStarted)
        {
            stateMachine.CurrentState.Move();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isRaceStarted)
        {
            stateMachine.CurrentState.Trigger(other);
        }
    }

    private void OnNitroInput()
    {
        if (isRaceStarted)
        {
            stateMachine.CurrentState.InputNitro();
        }
    }

    private void OnRightInput()
    {
        if (isRaceStarted)
        {
            stateMachine.CurrentState.InputRight();
        }
    }

    private void OnLeftInput()
    {
        if (isRaceStarted)
        {
            stateMachine.CurrentState.InputLeft();
        }
    }


}
