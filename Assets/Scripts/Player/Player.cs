using UnityEngine;
using Fusion;

public class Player : NetworkBehaviour
{
    public Vector3 MoveDirection { get => Vector3.forward; }

    private bool isRaceStarted;

    public Rigidbody Rigidbody { get; private set; }
    public NetworkCharacterControllerPrototype CC { get; private set; }

    private StateMachine stateMachine;
    public DefaultState DefaultState { get; private set; }
    public OilState OilState { get; private set; }
    public NitroState NitroState { get; private set; }
    public NitroOilState NitroOilState { get; private set; }
    private bool isInit;

    public void Init(InputDetector inputDetector)
    {
        inputDetector.OnLeftInput += OnLeftInput;
        inputDetector.OnRightInput += OnRightInput;
        inputDetector.OnNitroInput += OnNitroInput;

        Rigidbody = GetComponent<Rigidbody>();
        CC = GetComponent<NetworkCharacterControllerPrototype>();

        stateMachine = new StateMachine();

        DefaultState = new DefaultState(stateMachine, this);
        OilState = new OilState(stateMachine, this);
        NitroState = new NitroState(stateMachine, this); 
        NitroOilState = new NitroOilState(stateMachine, this);

        stateMachine.Init(DefaultState);
        isInit = true;
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
