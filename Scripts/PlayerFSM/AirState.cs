using Godot;
using System;

public class AirState : PlayerState
{
    [Export] private float maxSpeedX;    // shared

    [Export] private float airTimeToAccelerateX;
    [Export] private float airTimeToDecelerateX;

    [Export] private int maxNumberOfAirJumps = 2;

    private Vector3 velocity = Vector3.Zero;  // shared
    private Vector3 direction = Vector3.Zero; // shared

    [Export] private float jumpPeakHeight = 5f;       // shared (if air jumps)
    [Export] private float jumpPeakDistanceX = 5f;    // shared
    [Export] private float boostHInTiles = 2f;
    [Export] private float fallGravityMultiplier = 2f;
    [Export] private float fallButtonGravityMultiplier = 3f;
    [Export] private float terminalVelocityY = -40f;

    private float timeHeldLateralButton;      // shared; ??
    private float timeReleasedLateralButton;  // shared; ??
    private float speedX;                     // shared; ??
    private float previousSpeedX;             // shared; ??
    private float initialVelocityY;           // shared?; ??
    private float boostedHVelocityY;          // shared?; ??
    private float baseGravity;        // for upwards
    private int numberOfAirJumps;

    public override void _Ready()
    {
        base._Ready();    // call not explicitly necessary for engine functions? https://github.com/godotengine/godot-docs/issues/2576#issuecomment-529754003

        initialVelocityY = (2 * jumpPeakHeight * maxSpeedX) / jumpPeakDistanceX;
        baseGravity = (-2 * jumpPeakHeight * Mathf.Pow(maxSpeedX, 2)) / Mathf.Pow(jumpPeakDistanceX, 2);

        // max jump boost from horizontal speed - for the same horizontal velocity as a regular jump, at max horizontal speed add "boostHInTiles" tiles' worth of velocity to the final jump
        // paper notes contain how this was derived (thanks to Paul Bonsma)
        // What we want: put "boosted jump height" in the inspector, and calculate the boostedVelocity based on that.
        // (Given: initVel, baseGrav - !! those don't change)
        boostedHVelocityY = Mathf.Sqrt(-2 * baseGravity * (jumpPeakHeight + boostHInTiles));
    }

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
            timeHeldLateralButton = Mathf.Min(timeHeldLateralButton + delta, airTimeToAccelerateX);
            speedX = Mathf.Lerp(previousSpeedX, maxSpeedX, timeHeldLateralButton / airTimeToAccelerateX);
        }
        else
        {
            timeReleasedLateralButton = Mathf.Min(timeReleasedLateralButton + delta, airTimeToDecelerateX);
            speedX = Mathf.Lerp(previousSpeedX, 0f, timeReleasedLateralButton / airTimeToDecelerateX);
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
        if (direction != Vector3.Zero) direction = direction.Normalized();

        velocity.x = direction.x * speedX;
        velocity.z = direction.z * speedX;

        Vector3 velocityH = new Vector3(velocity.x, 0, velocity.z);


        // handle gravity modifiers
        float actualGravity = baseGravity;
        if (/*velocity.y < 0 || */!Input.IsActionPressed("jump"))   // the commented out part influences the params, so leave it away for this design
        {
            actualGravity = baseGravity * fallGravityMultiplier;
        }
        if (Input.IsActionPressed("fall_down"))
        {
            actualGravity = baseGravity * fallButtonGravityMultiplier;
        }

        velocity.y = Mathf.Max(velocity.y + actualGravity * delta, terminalVelocityY);


        // handle jump (apply jump velocity) IF maxNumJumps is defined and > 0
        if (maxNumberOfAirJumps > 0)
        {
            if (numberOfAirJumps > 0)
            {
                if (Input.IsActionJustPressed("jump"))
                {
                    velocity.y = Mathf.Lerp(initialVelocityY, boostedHVelocityY, velocityH.Length() / maxSpeedX);
                }
                numberOfAirJumps = Mathf.Max(numberOfAirJumps--, 0);
            }
        }


        // DID NOT HANDLE JUMP FROM GROUNDED STATE YET


        if (playerBody.IsOnFloor())
        {
            stateMachine.TransitionTo("GroundState", velocity);
        }

        //eventually after processing...
        velocity = playerBody.MoveAndSlide(velocity, Vector3.Up, infiniteInertia: false);
    }

    public override void Enter(Vector3 prevStateVelocity)
    {
        velocity = prevStateVelocity;
        // assume we can only enter from Grounded state
        // (re)set maxNumJumps here, if any.


    }

    public override void Exit()
    {
        // cleanup or value propagation?

        // ~~resetting the number of air jumps really doesn't feel like this state's business...
        numberOfAirJumps = maxNumberOfAirJumps;
    }
}
