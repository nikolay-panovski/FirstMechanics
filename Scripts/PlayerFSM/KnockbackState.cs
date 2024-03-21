using Godot;
using System;

public class KnockbackState : PlayerState
{
    // [Export] private float maxSpeedX;    // ??

    //private Vector3 velocity = Vector3.Zero;  // shared
    //private Vector3 direction = Vector3.Zero; // shared

    //[Export] private float jumpPeakHeight = 5f;       // ??
    //[Export] private float jumpPeakDistanceX = 5f;    // ??
    //[Export] private float fallGravityMultiplier = 2f;
    //[Export] private float fallButtonGravityMultiplier = 3f;
    //[Export] private float terminalVelocityY = -40f;

    //private KnockbackCalculator knockback;    // ??
    //private Timer invincibilityTimer;
    //[Export] private float hitInvincibilitySeconds = 1f;

    public override void HandleInput(InputEvent inputEvent)
    {
        // do not handle input.
    }

    public override void Update(float deltaTime)
    {
        throw new NotImplementedException();
    }

    public override void PhysicsUpdate(float deltaTime)
    {
        // apply auto velocity per frame (TODO: to be figured out after applying regular velocity in the other 2 states)
        // ?? velocity = playerBody.MoveAndSlide(velocity, Vector3.Up, infiniteInertia: false) ??



        // we're only in this state to punish the player and auto-move it, exit on timer
        if (playerBody.IsOnFloor() /*&& invincibilityTimer == 0 or Elapsed*/)
        {
            stateMachine.TransitionTo("AirState");
        }
    }

    public override void Enter()
    {
        // need to calculate knockback velocity from incoming velocity here
        throw new NotImplementedException();
    }

    public override void Exit()
    {
        // nothing much to do, though think about (extra) jumps
    }
}
