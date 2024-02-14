using Godot;
using System;

public class BreakableKickForceModifier : Node
{
    [Export] private float kickForce;
    private RigidBody parent;

    public override void _Ready()
    {
        parent = GetParent<RigidBody>();
    }

    public void OnBodyEntered(Node body)
    {
        if (body.Name == "PlayerCylinder")
        {
            GD.Print("kicked");
            parent.AddCentralForce(((body as Spatial).GlobalTranslation - parent.GlobalTranslation).Normalized() * kickForce);
        }
    }
}
