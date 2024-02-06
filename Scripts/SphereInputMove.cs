using Godot;
using System;

public class SphereInputMove : RigidBody
{
    // Declare member variables here. Examples:
    [Export] private int speed = 10;
    [Export] private float angularSpeed = Mathf.Pi;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        Vector3 direction = Vector3.Zero;

        if (Input.IsActionPressed("move_right"))
        {
            direction.x += 1f;
        }
        if (Input.IsActionPressed("move_left"))
        {
            direction.x -= 1f;
        }
        if (Input.IsActionPressed("move_forward"))
        {
            direction.z -= 1f;
        }
        if (Input.IsActionPressed("move_backward"))
        {
            direction.z += 1f;
        }

        if (direction != Vector3.Zero) direction = direction.Normalized();

        AddCentralForce(direction * speed * delta);
    }
}
