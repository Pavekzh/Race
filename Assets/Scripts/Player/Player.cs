using UnityEngine;
using Fusion;

public class Player : NetworkBehaviour
{
    [SerializeField] private float changingLaneSpeed = 1;

    public float ChangingLaneSpeed { get => changingLaneSpeed; }


    public Vector3 MoveDirection { get => Vector3.forward; }
    public int CurrentLane { get; set; }

    private bool isRaceStarted;

    public Rigidbody Rigidbody { get; private set; }
    public NetworkCarController CC { get; private set; }

    private StateMachine<BaseState> stateMachine;

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

    public void SlowDown()
    {
        if (isRaceStarted)
        {  
            stateMachine.CurrentState.SlowDown();
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
