using Godot;

/// USE MoveAndJumpPeakDistance. This one is merely what that one "derives" from, for all other purposes this is just
/// an "alternative" with duplicate code that is not updated.
/// 
/// Based on tutorial at:
/// https://docs.godotengine.org/en/3.5/getting_started/first_3d_game/03.player_movement_code.html
/// 
/// https://www.youtube.com/watch?v=hG9SzQxaCm8
public class MoveAndJumpPeakTime : KinematicBody
{
    [Export] private int speed = 10;

    private Vector3 velocity = Vector3.Zero;

    [Export] private int maxNumberOfJumps = 2;
    [Export] private float jumpPeakHeight = 5f;    // default to maxHeight = 2 * character height; no exact science behind it, hence no "characterHeight" variable
    [Export] private float jumpPeakTime = 0.5f;
    [Export] private float fallGravityMultiplier = 2f;
    [Export] private float fallButtonGravityMultiplier = 3f;
    [Export] private float terminalVelocityY = -40f;

    private float initialVelocityY;
    private float baseGravity;
    private int numberOfJumps;

    public override void _Ready()
    {
        initialVelocityY = 2 * jumpPeakHeight / jumpPeakTime;
        baseGravity = -2 * jumpPeakHeight / Mathf.Pow(jumpPeakTime, 2);
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        Vector3 direction = Vector3.Zero;
        float actualGravity = baseGravity;

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

        if (IsOnFloor())
        {
            numberOfJumps = maxNumberOfJumps;
        }
        if (Input.IsActionJustPressed("jump"))
        {
            numberOfJumps = Mathf.Max(numberOfJumps--, 0);
            if (numberOfJumps > 0) velocity.y = initialVelocityY;
        }
        if (/*velocity.y < 0 || */!Input.IsActionPressed("jump"))   // the commented out part influences the params, so leave it away for this design
        {
            actualGravity = baseGravity * fallGravityMultiplier;
        }
        if (Input.IsActionPressed("fall_down"))
        {
            actualGravity = baseGravity * fallButtonGravityMultiplier;
        }

        if (direction != Vector3.Zero) direction = direction.Normalized();

        velocity.x = direction.x * speed;
        velocity.z = direction.z * speed;
        velocity.y += actualGravity * delta;  // leave the sign to the gravity variable

        velocity.y = Mathf.Max(velocity.y, terminalVelocityY);


        //debug Y acceleration measures
        Utils.DebugPrintTimed(30, "Velocity = " + velocity);

        velocity = MoveAndSlide(velocity, Vector3.Up);
    }
}
