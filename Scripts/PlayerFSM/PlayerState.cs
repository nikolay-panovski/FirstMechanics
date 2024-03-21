using Godot;
using System;

public abstract class PlayerState : Node
{
    // ~~totally encapsulated
    public PlayerStateMachine stateMachine { get; set; }

    public abstract void HandleInput(InputEvent inputEvent);
    public abstract void Update(float deltaTime);
    public abstract void PhysicsUpdate(float deltaTime);
    public abstract void Enter(/*object message*/);
    public abstract void Exit();
}
