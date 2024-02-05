using Godot;
using System;

public class SphereInputMove : KinematicBody
{
    // Declare member variables here. Examples:
    [Export] private int speed = 10;
    [Export] private float angularSpeed = Mathf.Pi;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Input.IsActionPressed("move_up"))   // works
        {
            MoveAndSlide(Vector3.Forward * speed * delta, Vector3.Up);  // doesn't work
        }
    }

    // TODO: input polling in _Process, but actual movement in _PhysicsProcess
}
