using Godot;
using System;

public class BreakableCheckImpactForBreak : RigidBody
{
    [Export] private float maxCollisionVelocity = 1f;

    public override void _IntegrateForces(PhysicsDirectBodyState state)
    {
        GD.Print(state.LinearVelocity + " with length" + state.LinearVelocity.Length());

        if (state.LinearVelocity.Length() > maxCollisionVelocity)
        {
            GD.Print("colliding hard enough, break it");
            QueueFree();
        }
    }
}
