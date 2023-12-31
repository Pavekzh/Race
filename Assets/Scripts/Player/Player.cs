using Fusion;
using UnityEngine;

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

    public bool IsLocal { get; private set; }

    public float ChangingLaneSpeed { get => changingLaneSpeed; }
    public string ObstacleTag { get => obstacleTag; }
    public int CurrentLane { get; set; }
    public float Nitro 
    {
        get => nitro;
        set
        {
            nitro = Mathf.Clamp(value, 0, maxNitro);
            UpdateNitrobar();
        }
    }
    public float NitroSpeedMultiplier { get => nitroSpeedMultiplier; }
    public float NitroAccelerationMultiplier { get => nitroAccelerationMultiplier; }
    public float NitroConsumption { get => nitroConsumption; }
    public float SlowEffectTime { get; set; }
    public float SlowEffectMultiplier { get; set; }


    private bool isRaceGoing;

    private StateMachine<BaseState> stateMachine;

    public Rigidbody Rigidbody { get; private set; }
    public NetworkCarController CC { get; private set; }
    public WorldGenerator WorldGenerator { get; private set; }
    private InGameUI PlayerUI;

    public void Init(WorldGenerator worldGenerator,int lane,bool isLocal, InGameUI playerUI = null)
    {
        this.CurrentLane = lane;
        this.WorldGenerator = worldGenerator;
        this.IsLocal = isLocal;
        this.PlayerUI = playerUI;

        UpdateNitrobar();


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
        isRaceGoing = true;
    }

    public void StopRace()
    {
        isRaceGoing = false;
    }

    public override void FixedUpdateNetwork()
    {
        if (isRaceGoing)
        {
            if (HasStateAuthority)
            {
                PlayerInput input;
                if (GetInput<PlayerInput>(out input))
                {
                    if (input.LeftInput)
                        stateMachine.CurrentState.InputLeft();
                    else if (input.RightInput)
                        stateMachine.CurrentState.InputRight();

                    if (input.NitroInput)
                        stateMachine.CurrentState.InputNitro();

                }
            }


            stateMachine.CurrentState.Move();

            UpdateSpedometer();

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isRaceGoing && HasStateAuthority)
        {
            stateMachine.CurrentState.Trigger(other);
        }
    }

    public void SetOilIndicator(bool value)
    {
        if (Runner.IsServer)
            RPC_SetOilIndicator(value);

    }

    public void UpdateNitrobar()
    {
        if (Runner.IsServer)
            RPC_UpdateNitrobar(nitro);
    }

    public void UpdateSpedometer()
    {
        if (PlayerUI != null)
        {
            PlayerUI.UpdateSpeed(CC.HorizontalVelocity.magnitude / CC.DefaultMaxSpeed);
        }
    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_SetOilIndicator(bool value)
    {
        if (PlayerUI != null)
            PlayerUI.SetOilIndicator(value);
    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_UpdateNitrobar(float nitro)
    {
        if (PlayerUI != null)
            PlayerUI.UpdateNitro(nitro / maxNitro);
    }

}
