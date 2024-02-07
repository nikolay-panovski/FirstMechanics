using Godot;
using System;

/// Based on tutorial at:
/// https://docs.godotengine.org/en/3.5/getting_started/first_3d_game/03.player_movement_code.html
public class SphereInputMove : KinematicBody
{
    // Declare member variables here.
    [Export] private int speed = 10;
    [Export] private int fallAcceleration = 75;

    private Vector3 velocity = Vector3.Zero;


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
            velocity.y += fallAcceleration * delta * 8;
        }
        //debug Y acceleration measures
        if ((Time.GetTicksMsec() % 1000f) < 17f)   // Godot complains about output overflows a lot, so only print around once per second
                                                   // (this will eventually be worth a utility script)
        {
            GD.Print("Fall acceleration * delta = " + fallAcceleration * delta);
            GD.Print("Velocity = " + velocity);
        }


        if (direction != Vector3.Zero) direction = direction.Normalized();

        velocity.x = direction.x * speed;           // !! no delta (because of _PhysicsProcess?)
        velocity.z = direction.z * speed;
        velocity.y -= fallAcceleration * delta;     // !! yes delta??

        velocity = MoveAndSlide(velocity, Vector3.Up);
    }
}
