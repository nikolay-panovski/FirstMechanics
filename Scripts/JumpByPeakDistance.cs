using Godot;

/// <summary>
/// IMPLEMENT A LATERAL MOVER IF YOU ARE IMPLEMENTING THIS!!
/// </summary>
public class JumpByPeakDistance : Node, IJumper, IGravityApplier
{
    [Export] private float jumpPeakHeight = 5f;    // default to maxHeight = 2 * character height; no exact science behind it, hence no "characterHeight" variable
    [Export] private float jumpPeakDistanceX = 5f;  // half of total distance movable by full jump, assuming standard parabola

    private float initialVelocityY;
    private float baseGravity;


    public override void _Ready()
    {
        float speedX = GetNode<MoveLateralBasic>(new NodePath("../SCR_Move")).hSpeed;

        initialVelocityY = (2 * jumpPeakHeight * speedX) / jumpPeakDistanceX;
        CalculateGravityFromJumpParams(jumpPeakHeight, jumpPeakDistanceX / speedX);
    }

    public float GetJumpInitialVelocity()
    {
        return initialVelocityY;
    }

    public float GetJumpBaseGravity()
    {
        return baseGravity;
    }

    public void CalculateGravityFromJumpParams(float jumpPeakXVar, float jumpPeakYVar)  // this interface/method is bad *because* of the params
    {
        baseGravity = -2 * jumpPeakXVar / Mathf.Pow(jumpPeakYVar, 2);
    }
}
