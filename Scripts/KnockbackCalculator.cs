using Godot;

public class KnockbackCalculator : Node
{
    public Vector3 CalculateKnockbackVelocity(Vector3 enterVelocity)
    {
        return -enterVelocity;
    }
}
