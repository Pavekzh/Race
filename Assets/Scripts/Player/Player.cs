using UnityEngine;

public class Player : MonoBehaviour
{

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

        stateMachine = new StateMachine();

        DefaultState = new DefaultState(stateMachine, this);
        OilState = new OilState(stateMachine, this);
        NitroState = new NitroState(stateMachine, this); 
        NitroOilState = new NitroOilState(stateMachine, this);

        stateMachine.Init(DefaultState);
        isInit = true;
    }

    public void SlowDown()
    {
        if (isInit)
        {
            stateMachine.CurrentState.SlowDown();
        }
    }

    private void Update()
    {
        if(isInit)
        {
            stateMachine.CurrentState.Move();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isInit)
        {
            stateMachine.CurrentState.Trigger(other);
        }
    }

    private void OnNitroInput()
    {
        stateMachine.CurrentState.InputNitro();
    }

    private void OnRightInput()
    {
        stateMachine.CurrentState.InputRight();
    }

    private void OnLeftInput()
    {
        stateMachine.CurrentState.InputLeft();
    }


}
