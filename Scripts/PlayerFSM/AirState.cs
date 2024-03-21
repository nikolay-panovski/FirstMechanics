using Godot;
using System;

public class AirState : PlayerState
{
    // [Export] private float maxSpeedX;    // shared

    // [Export] private float airTimeToAccelerateX;
    // [Export] private float airTimeToDecelerateX;

    //[Export] private int maxNumberOfAirJumps = 2;

    //private Vector3 velocity = Vector3.Zero;  // shared
    //private Vector3 direction = Vector3.Zero; // shared

    //[Export] private float jumpPeakHeight = 5f;       // shared (if air jumps)
    //[Export] private float jumpPeakDistanceX = 5f;    // shared
    //[Export] private float fallGravityMultiplier = 2f;
    //[Export] private float fallButtonGravityMultiplier = 3f;
    //[Export] private float terminalVelocityY = -40f;

    //private float timeHeldLateralButton;      // shared; ??
    //private float timeReleasedLateralButton;  // shared; ??
    //private float speedX;                     // shared; ??
    //private float previousSpeedX;             // shared; ??
    //private float initialVelocityY;           // shared?; ??
    //private float boostedHVelocityY;          // shared?; ??
    //private float baseGravity;        // for upwards
    //private int numberOfAirJumps;

    public override void HandleInput(InputEvent inputEvent)
    {
        // 
        throw new NotImplementedException();
    }

    public override void Update(float deltaTime)
    {
        throw new NotImplementedException();
    }

    public override void PhysicsUpdate(float deltaTime)
    {
        // handle horizontal moves

        // handle jump (apply jump velocity) IF maxNumJumps is defined and > 0

        // but air state should kick in all the same when falling
        if (!playerBody.IsOnFloor())
        {
            stateMachine.TransitionTo("AirState");
        }

        //eventually after processing...
        //velocity = playerBody.MoveAndSlide(velocity, Vector3.Up, infiniteInertia: false);
    }

    public override void Enter()
    {
        // assume we can only enter from Grounded state
        // (re)set maxNumJumps here, if any.
        throw new NotImplementedException();
    }

    public override void Exit()
    {
        throw new NotImplementedException();
    }
}
