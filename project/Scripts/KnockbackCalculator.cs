using Godot;

public class KnockbackCalculator : Node, IKnockbackCalculator
{
    public Vector3 CalculateKnockbackVelocity(Vector3 enterVelocity)
    {
        return -enterVelocity;
    }
}
