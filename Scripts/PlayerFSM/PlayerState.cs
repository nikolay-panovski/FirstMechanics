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

    public abstract void HandleInput(InputEvent inputEvent);
    public abstract void Update(float deltaTime);   // maybe everything will happen in PhysicsUpdate for this project?
    public abstract void PhysicsUpdate(float deltaTime);
    public abstract void Enter(/*object message*/ /* TODO likely: receive the velocity from the previous state */);
    public abstract void Exit(/* TODO likely: pass over the velocity from the previous state */);
}
