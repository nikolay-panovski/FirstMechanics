using Godot;

/// This is currently the "god script" of the player character, with its core being movement, but potentially far and tightly coupled reach.
/// For this I blame whatever the recommended Godot architecture is for being confusing, coming from a Components background (Unity).
/// I will NOT use GetNode("../../../..")... as a substitute.
/// 
/// Based on tutorial at:
/// https://docs.godotengine.org/en/3.5/getting_started/first_3d_game/03.player_movement_code.html
/// 
/// https://www.youtube.com/watch?v=hG9SzQxaCm8
public class MoveAndJumpPeakDistance : KinematicBody, IHurtable
{
    [Export] private float maxSpeedX = 10f;
    [Export] private float groundTimeToAccelerateX = 0.5f;
    [Export] private float groundTimeToDecelerateX = 0.3f;
    [Export] private float airTimeToAccelerateX = 0.15f;
    [Export] private float airTimeToDecelerateX = 0.1f;

    private Vector3 velocity = Vector3.Zero;

    private Vector3 direction = Vector3.Zero;

    [Export] private int maxNumberOfJumps = 2;
    [Export] private float jumpPeakHeight = 5f;    // default to maxHeight = 2 * character height; no exact science behind it, hence no "characterHeight" variable
    [Export] private float jumpPeakDistanceX = 5f;  // half of total distance movable by full jump, assuming standard parabola
    [Export] private float fallGravityMultiplier = 2f;
    [Export] private float fallButtonGravityMultiplier = 3f;
    [Export] private float terminalVelocityY = -40f;
    [Export] private float boostHInTiles = 2f;

    [Export] private float hitInvincibilitySeconds = 1f;

    private float timeHeldLateralButton;
    private float timeReleasedLateralButton;
    private float speedX;
    private float previousSpeedX;
    private float initialVelocityY;
    private float boostedHVelocityY;
    private float baseGravity;
    private int numberOfJumps;

    private KnockbackCalculator knockback;
    private Timer invincibilityTimer;

    public override void _Ready()
    {
        knockback = GetNode<KnockbackCalculator>(new NodePath("KnockbackCalculator"));
        invincibilityTimer = GetNode<Timer>(new NodePath("HitTimer"));

        initialVelocityY = (2 * jumpPeakHeight * maxSpeedX) / jumpPeakDistanceX;
        baseGravity = (-2 * jumpPeakHeight * Mathf.Pow(maxSpeedX, 2)) / Mathf.Pow(jumpPeakDistanceX, 2);

        // max jump boost from horizontal speed - for the same horizontal velocity as a regular jump, at max horizontal speed add "boostHInTiles" tiles' worth of velocity to the final jump
        // paper notes contain how this was derived (thanks to Paul Bonsma)
        // What we want: put "boosted jump height" in the inspector, and calculate the boostedVelocity based on that.
        // (Given: initVel, baseGrav - !! those don't change)
        boostedHVelocityY = Mathf.Sqrt(-2 * baseGravity * (jumpPeakHeight + boostHInTiles));

        // = -baseGravity * jumpPeakDistanceX / speedX;
        // = boostHInTiles / jumpPeakHeight * initialVelocityY;
        // = (2 * boostHInTiles * speedX) / jumpPeakDistanceX
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        float actualGravity = baseGravity;

        float actualTimeToAccelerateX = IsOnFloor() ? groundTimeToAccelerateX : airTimeToAccelerateX;
        float actualTimeToDecelerateX = IsOnFloor() ? groundTimeToDecelerateX : airTimeToDecelerateX;

        if (Input.IsActionJustPressed("move_right") || Input.IsActionJustPressed("move_left") || Input.IsActionJustPressed("move_forward") || Input.IsActionJustPressed("move_backward"))
        {
            previousSpeedX = speedX;

            timeReleasedLateralButton = 0f;
            direction = Vector3.Zero;
        }
        if (Input.IsActionJustReleased("move_right") || Input.IsActionJustReleased("move_left") || Input.IsActionJustReleased("move_forward") || Input.IsActionJustReleased("move_backward"))
        {
            previousSpeedX = speedX;

            timeHeldLateralButton = 0f;
        }

        if (Input.IsActionPressed("move_right") || Input.IsActionPressed("move_left") || Input.IsActionPressed("move_forward") || Input.IsActionPressed("move_backward"))
        {
            timeHeldLateralButton += delta;
            if (timeHeldLateralButton > actualTimeToAccelerateX) timeHeldLateralButton = actualTimeToAccelerateX;
            speedX = Mathf.Lerp(previousSpeedX, maxSpeedX, timeHeldLateralButton / actualTimeToAccelerateX);
        }
        else
        {
            timeReleasedLateralButton += delta;
            if (timeReleasedLateralButton > actualTimeToDecelerateX) timeReleasedLateralButton = actualTimeToDecelerateX;
            speedX = Mathf.Lerp(previousSpeedX, 0f, timeReleasedLateralButton / actualTimeToDecelerateX);
        }

        if (Input.IsActionPressed("move_right"))
        {
            direction.x = 1f;
        }
        if (Input.IsActionPressed("move_left"))
        {
            direction.x = -1f;
        }
        if (Input.IsActionPressed("move_forward"))
        {
            direction.z = -1f;
        }
        if (Input.IsActionPressed("move_backward"))
        {
            direction.z = 1f;
        }


        if (direction != Vector3.Zero) direction = direction.Normalized();

        velocity.x = direction.x * speedX;
        velocity.z = direction.z * speedX;

        Vector3 velocityH = new Vector3(velocity.x, 0, velocity.z);

        if (/*velocity.y < 0 || */!Input.IsActionPressed("jump"))   // the commented out part influences the params, so leave it away for this design
        {
            actualGravity = baseGravity * fallGravityMultiplier;
        }
        if (Input.IsActionPressed("fall_down"))
        {
            actualGravity = baseGravity * fallButtonGravityMultiplier;
        }


        velocity.y += actualGravity * delta;  // leave the sign to the gravity variable

        velocity.y = Mathf.Max(velocity.y, terminalVelocityY);


        if (IsOnFloor())
        {
            numberOfJumps = maxNumberOfJumps;
        }
        if (Input.IsActionJustPressed("jump"))
        {
            if (numberOfJumps > 0)
            {
                velocity.y = Mathf.Lerp(initialVelocityY, boostedHVelocityY, velocityH.Length() / maxSpeedX);
            }

            numberOfJumps--;
            if (numberOfJumps < 0) numberOfJumps = 0;
        }


        //debug Y acceleration measures
        //Utils.DebugPrintTimed(30, velocity);

        velocity = MoveAndSlide(velocity, Vector3.Up, infiniteInertia: true);
        // re: infiniteInertia: should this ignore collisions with RigidBodies but push them? or should it treat them as statics?
        // in either case, the pushee should handle the fact it's being touched/kicked somehow. so let's try with push without collision (true).
    }

    public void TakeHurtboxCollisionEffect()
    {
        // CALL on touching a hurtbox area
        // report linear velocity 
        // calculate knockback from that
        // stun player (do not accept input)
        // apply the knockback (only use MoveAndCollide for movement, but take same gravity as above into account)
        // -> properly fixing all of these will take quite long and points towards code architecture and FSM, therefore only sufficiently patch it together
        
        // print the fact of knockback

        if (invincibilityTimer.TimeLeft == 0)
        {
            invincibilityTimer.Start(hitInvincibilitySeconds);

            Vector3 knockbackVelocity;
            knockbackVelocity = knockback.CalculateKnockbackVelocity(velocity);
            MoveAndCollide(knockbackVelocity);
            GD.Print("Knockback, moved by " + knockbackVelocity);
        }
        // else: player still has iframes, do not parse the hurtbox collision as such for this duration
    }
}
