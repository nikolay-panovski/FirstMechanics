

public interface IJumper
{
    float GetJumpInitialVelocity();
    float GetJumpBaseGravity();
}

/// <summary>
/// Implement this interface on an object ONLY if you want to define the gravity for that object based on
/// jump parameters. In reality, this is a DESIGN MODEL, so this interface should be used very few times -
/// e.g. for player character. Afterwards, take the sensible gravity value and apply it across the environment.
/// </summary>
public interface IGravityApplier
{
    void CalculateGravityFromJumpParams(float jumpPeakXVar, float jumpPeakYVar);
}