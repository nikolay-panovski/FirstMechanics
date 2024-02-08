using Godot;
using System;

/// Based on tutorial at:
/// https://docs.godotengine.org/en/3.5/getting_started/first_3d_game/03.player_movement_code.html
/// 
/// https://www.youtube.com/watch?v=hG9SzQxaCm8
public class SphereInputMove : KinematicBody
{
    [Export] private int speed = 10;

    private Vector3 velocity = Vector3.Zero;


    [Export] private float jumpPeakHeight = 10f;    // default to maxHeight = 4 * character height; no exact science behind it
    [Export] private float jumpPeakTime = 0.5f;

    private float initialVelocityY;
    private float baseGravity;

    public override void _Ready()
    {
        initialVelocityY = 2 * jumpPeakHeight / jumpPeakTime;
        baseGravity = -2 * jumpPeakHeight / Mathf.Pow(jumpPeakTime, 2);
        GD.Print(initialVelocityY);
        GD.Print(baseGravity);
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
        if (IsOnFloor() && Input.IsActionPressed("jump"))
        {
            velocity.y = initialVelocityY;
        }

        if (direction != Vector3.Zero) direction = direction.Normalized();

        velocity.x = direction.x * speed;
        velocity.z = direction.z * speed;
        velocity.y += baseGravity * delta;  // leave the sign to the gravity variable


        //debug Y acceleration measures
        Utils.DebugPrintTimed(30, "Velocity = " + velocity);

        velocity = MoveAndSlide(velocity, Vector3.Up);
    }
}
