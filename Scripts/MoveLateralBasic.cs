using Godot;

// Can only be called by KinematicBodies.
public class MoveLateralBasic : Node
{
    // ~~uh oh, coupling crap (see JumpByPeakDistance being coupled to this)
    [Export] public int hSpeed { get; private set; } = 10;

    public Vector3 GetLateralMoveVector()
    {
        Vector3 velocity = Vector3.Zero;
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

        velocity.x = direction.x * hSpeed;
        velocity.z = direction.z * hSpeed;

        return velocity;
    }
}
