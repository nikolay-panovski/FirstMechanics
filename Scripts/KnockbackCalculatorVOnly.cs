using Godot;
using System;

public class KnockbackCalculatorVOnly : Node, IKnockbackCalculator
{
    [Export] private float VKnockbackStrength;

    public Vector3 CalculateKnockbackVelocity(Vector3 enterVelocity)
    {
        return new Vector3(0, -enterVelocity.y * VKnockbackStrength, 0);
    }
}
