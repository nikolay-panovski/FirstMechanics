using Godot;

public interface IHurtable
{
    void TakeHurtboxCollisionEffect();
}

public interface IKnockbackCalculator
{
    Vector3 CalculateKnockbackVelocity(Vector3 enterVelocity);
}