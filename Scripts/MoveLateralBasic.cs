using Godot;

// Requires the parent to be a KinematicBody, else will throw. How to deal with this?
public class MoveLateralBasic : Node
{
    [Export] private int speedX = 10;

    private Vector3 velocity = Vector3.Zero;

    private KinematicBody parent;

    public override void _Ready()
    {
        parent = GetParent<KinematicBody>();
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

        velocity.x = direction.x * speedX;
        velocity.z = direction.z * speedX;


        //debug Y acceleration measures
        //Utils.DebugPrintTimed(30, "Velocity = " + velocity);

        velocity = parent.MoveAndSlide(velocity, Vector3.Up);
    }
}
