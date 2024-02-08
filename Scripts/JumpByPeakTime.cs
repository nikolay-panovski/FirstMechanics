using Godot;

public class JumpByPeakTime : Node, IJumper, IGravityApplier
{
    [Export] private float jumpPeakHeight = 5f;    // default to maxHeight = 2 * character height; no exact science behind it, hence no "characterHeight" variable
    [Export] private float jumpPeakTime = 0.5f;

    private float initialVelocityY;
    private float baseGravity;      // per jump implementation, [Export] EITHER the jump params above OR this directly!

    public override void _Ready()
    {
        initialVelocityY = 2 * jumpPeakHeight / jumpPeakTime;
        CalculateGravityFromJumpParams(jumpPeakHeight, jumpPeakTime);
    }


    public void CalculateGravityFromJumpParams(float jumpPeakXVar, float jumpPeakYVar)
    {
        baseGravity = -2 * jumpPeakXVar / Mathf.Pow(jumpPeakYVar, 2);
    }

    public float GetJumpInitialVelocity()
    {
        return initialVelocityY;
    }

    public float GetJumpBaseGravity()
    {
        return baseGravity;
    }
}
