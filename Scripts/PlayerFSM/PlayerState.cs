using Godot;
using System;

public abstract class PlayerState : Node
{
    // ~~totally encapsulated
    public PlayerStateMachine stateMachine { get; set; }
    // all player states probably want to do something physics-related on the body (here: move and/or jump, whether by input or automatically)
    protected KinematicBody playerBody;

    public override void _Ready()
    {
        playerBody = Owner as KinematicBody;
    }
 
    public abstract void PhysicsUpdate(float delta);    // everything will happen in PhysicsUpdate for a movement-focused FSM
    public abstract void Enter(/*object message*/ Vector3 prevStateVelocity);
    public abstract void Exit(/* TODO likely: pass over the velocity from the previous state */);
}
