using Godot;
using System;

// Grounded state deals with horizontal movements. Jumps are offloaded to the air state.
public class GroundedState : PlayerState
{
    [Export] private float maxSpeedX;    // shared

    [Export] private float groundTimeToAccelerateX;
    [Export] private float groundTimeToDecelerateX;

    private Vector3 velocity = Vector3.Zero;  // shared
    private Vector3 direction = Vector3.Zero; // shared

    private float timeHeldLateralButton;      // shared; ??
    private float timeReleasedLateralButton;  // shared; ??
    private float speedX;                     // shared; ??
    private float previousSpeedX;             // shared; ??


    public override void PhysicsUpdate(float delta)
    {
        // handle horizontal moves as constant Input.IsXWhatever input
        // !! and leftover velocity from previous state too

        if (Input.IsActionJustPressed("move_right") || Input.IsActionJustPressed("move_left") || Input.IsActionJustPressed("move_forward") || Input.IsActionJustPressed("move_backward"))
        {
            previousSpeedX = speedX;

            timeReleasedLateralButton = 0f;
            direction = Vector3.Zero;
        }
        if (Input.IsActionJustReleased("move_right") || Input.IsActionJustReleased("move_left") || Input.IsActionJustReleased("move_forward") || Input.IsActionJustReleased("move_backward"))
        {
            previousSpeedX = speedX;

            timeHeldLateralButton = 0f;
        }

        if (Input.IsActionPressed("move_right") || Input.IsActionPressed("move_left") || Input.IsActionPressed("move_forward") || Input.IsActionPressed("move_backward"))
        {
            timeHeldLateralButton = Mathf.Min(timeHeldLateralButton + delta, groundTimeToAccelerateX);
            speedX = Mathf.Lerp(previousSpeedX, maxSpeedX, timeHeldLateralButton / groundTimeToAccelerateX);
        }
        else
        {
            timeReleasedLateralButton = Mathf.Min(timeReleasedLateralButton + delta, groundTimeToDecelerateX);
            speedX = Mathf.Lerp(previousSpeedX, 0f, timeReleasedLateralButton / groundTimeToDecelerateX);
        }

        if (Input.IsActionPressed("move_right"))
        {
            direction.x = 1f;
        }
        if (Input.IsActionPressed("move_left"))
        {
            direction.x = -1f;
        }
        if (Input.IsActionPressed("move_forward"))
        {
            direction.z = -1f;
        }
        if (Input.IsActionPressed("move_backward"))
        {
            direction.z = 1f;
        }

        // let air state completely deal with vertical velocity
        if (Input.IsActionJustPressed("jump"))
        {
            stateMachine.TransitionTo("AirState", velocity);    // !! TODO: AIR STATE DOES NOT KNOW WE JUMPED HERE
        }

        // air state should kick in whether jumping (input pressed "jump") or falling (not on ground) -> check for both!
        if (!playerBody.IsOnFloor())
        {
            stateMachine.TransitionTo("AirState", velocity);
        }

        if (direction != Vector3.Zero) direction = direction.Normalized();

        velocity.x = direction.x * speedX;
        velocity.z = direction.z * speedX;

        //Vector3 velocityH = new Vector3(velocity.x, 0, velocity.z);

        //eventually after processing...
        velocity = playerBody.MoveAndSlide(velocity, Vector3.Up, infiniteInertia: false);
    }

    public override void Enter(Vector3 prevStateVelocity)
    {
        // coming back from airborne/knockback state, apply remaining momentum or something
        // [NEEDS MESSAGE] - what did I mean??
        velocity = prevStateVelocity;
        // interesting opportunity to inspect the floor more depending on velocity
        // e.g.: velocity higher than a threshold + breakable floor = break the floor
        
        
    }

    public override void Exit()
    {
        // cleanup or value propagation?
    }
}
