using Godot;
using System;

public class GroundedState : PlayerState
{
    // private float groundTimeToAccelerateX;
    // private float groundTimeToDecelerateX;

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
        // when jump (apply jump velocity)

        // but air state should kick in all the same when falling
        if (!(Owner as KinematicBody).IsOnFloor())
        {
            stateMachine.TransitionTo("AirState");
        }
    }

    public override void Enter()
    {
        // coming back from airborne state, apply remaining momentum or something
        throw new NotImplementedException();
    }

    public override void Exit()
    {
        throw new NotImplementedException();
    }
}
