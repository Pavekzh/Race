using System;
using Fusion;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[OrderBefore(typeof(NetworkTransform))]
[DisallowMultipleComponent]
// ReSharper disable once CheckNamespace
public class NetworkCarController : NetworkTransform
{
    [Header("Character Controller Settings")]
    [SerializeField] private float gravity = -20.0f;
    [SerializeField] private float acceleration = 10.0f;
    [SerializeField] private float braking = 10.0f;
    [SerializeField] private float maxSpeed = 2.0f;
    [SerializeField] private float rotationSpeed = 15.0f;

    public float MaxSpeed { get => maxSpeed; }
    public float DefaultMaxSpeed { get; private set; }
    public float DefaultAcceleration { get; private set; }

    public void SetMultipliedAcceleration(float multiplier)
    {
        acceleration = DefaultAcceleration * multiplier;
    }

    public void SetMultipliedMaxSpeed(float multiplier)
    {
        maxSpeed = DefaultMaxSpeed * multiplier;
    }

    public void SetDefaultAcceleration()
    {
        acceleration = DefaultAcceleration;
    }

    public void SetDefaultMaxSpeed()
    {
        maxSpeed = DefaultMaxSpeed;
    }

    [Networked]
    [HideInInspector]
    public bool IsGrounded { get; set; }

    [Networked]
    [HideInInspector]
    public Vector3 Velocity { get; set; }

    public Vector3 HorizontalVelocity
    {
        get
        {
            return new Vector3(Velocity.x, 0, Velocity.z);
        }
    }

    /// <summary>
    /// Sets the default teleport interpolation velocity to be the CC's current velocity.
    /// For more details on how this field is used, see <see cref="NetworkTransform.TeleportToPosition"/>.
    /// </summary>
    protected override Vector3 DefaultTeleportInterpolationVelocity => Velocity;

    /// <summary>
    /// Sets the default teleport interpolation angular velocity to be the CC's rotation speed on the Z axis.
    /// For more details on how this field is used, see <see cref="NetworkTransform.TeleportToRotation"/>.
    /// </summary>
    protected override Vector3 DefaultTeleportInterpolationAngularVelocity => new Vector3(0f, 0f, rotationSpeed);

    public CharacterController Controller { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        DefaultAcceleration = acceleration;
        DefaultMaxSpeed = maxSpeed;
        CacheController();
    }

    public override void Spawned()
    {
        base.Spawned();
        CacheController();
    }

    private void CacheController()
    {
        if (Controller == null)
        {
            Controller = GetComponent<CharacterController>();

            Assert.Check(Controller != null, $"An object with {nameof(NetworkCharacterControllerPrototype)} must also have a {nameof(CharacterController)} component.");
        }
    }

    protected override void CopyFromBufferToEngine()
    {
        // Trick: CC must be disabled before resetting the transform state
        Controller.enabled = false;

        // Pull base (NetworkTransform) state from networked data buffer
        base.CopyFromBufferToEngine();

        // Re-enable CC
        Controller.enabled = true;
    }


    public virtual void SetSpeed(float speed)
    {
        this.Velocity = Velocity.normalized * speed;
    }

    public virtual void SetPosition(Vector3 position)
    {
        Controller.enabled = false;
        TeleportToPosition(position);
        Controller.enabled = true;
    }

    /// <summary>
    /// Basic implementation of a character controller's movement function based on an intended direction.
    /// <param name="direction">Intended movement direction, subject to movement query, acceleration and max speed values.</param>
    /// </summary>
    public virtual void Move(Vector3 direction)
    {
        var deltaTime = Runner.DeltaTime;
        var previousPos = transform.position;
        var moveVelocity = Velocity;

        direction = direction.normalized;

        if (IsGrounded && moveVelocity.y < 0)
        {
            moveVelocity.y = 0f;
        }

        moveVelocity.y += gravity * Runner.DeltaTime;

        var horizontalVel = default(Vector3);
        horizontalVel.x = moveVelocity.x;
        horizontalVel.z = moveVelocity.z;

        if (direction == default)
        {
            horizontalVel = Vector3.Lerp(horizontalVel, default, braking * deltaTime);
        }
        else
        {
            horizontalVel = Vector3.ClampMagnitude(horizontalVel + direction * acceleration * deltaTime, maxSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Runner.DeltaTime);
        }

        moveVelocity.x = horizontalVel.x;
        moveVelocity.z = horizontalVel.z;

        Controller.Move(moveVelocity * deltaTime);

        if(Runner.IsServer)
            Velocity = (transform.position - previousPos) * Runner.Simulation.Config.TickRate;
        IsGrounded = Controller.isGrounded;
    }


}