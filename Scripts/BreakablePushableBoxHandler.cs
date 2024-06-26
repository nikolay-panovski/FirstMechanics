using Godot;
using System;

public class BreakablePushableBoxHandler : RigidBody
{
    [Export] private bool particlesOnBreak = true;
    [Export] private int numCoinsOnBreak = 3;

    [Export] private float breakingWallCollisionVelocity = 1f;
    // actually exporting Node-derived properties instead of bare NodePaths is only available in 4.X:
    // https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_exports.html#nodes
    // a PropertyHint export attribute for this type restriction is also not available in C#, at least here on 3.5.
    //[Export] private PushableKickForceModifier kickable;
    // solution? not in this project. bare GetNode("pathname") instead to adhere to "call down, signal up".

    // final version: the object is assumed to have all properties (kickable, wallBreakable, playerStompable) by default, non-adjustable,
    // and is hardcoded between itself and the player object as such. this is because of all different involved body interactions.
    // not sure whether anything more designer-/Inspector-adjustable is possible in this case.

    private PackedScene particlePrefab;
    private PackedScene coinPrefab;

    public override void _Ready()
    {
        particlePrefab = ResourceLoader.Load<PackedScene>("res://Scenes/Prefabs/OnBreakParticles.tscn");
        coinPrefab = ResourceLoader.Load<PackedScene>("res://Scenes/Prefabs/Coin.tscn");
    }

    public void OnBodyEnteredWallBreakable(Node body)
    {
        if (!body.IsInGroup("Player"))
        {
            if (LinearVelocity.Length() > breakingWallCollisionVelocity)
            {
                BreakableBreak();
            }
        }
    }

    public void BreakableBreak()
    {
        if (particlesOnBreak)
        {
            Node particleInstance = particlePrefab.Instance();
            GetTree().Root.AddChild(particleInstance);
            (particleInstance as Spatial).GlobalTranslation = GlobalTranslation;
            (particleInstance as Particles).Emitting = true;
            // leaks particle instances (set as one-shot, it will stop emitting, but will still exist)
        }
        for (int i = 0; i < numCoinsOnBreak; i++)
        {
            Node coinInstance = coinPrefab.Instance();
            GetTree().Root.AddChild(coinInstance);
            (coinInstance as Spatial).GlobalTranslation = GlobalTranslation;
            // probably out of scope: add movement on spawn (random initial velocity ala the particles)
        }

        CounterBrokenBoxes.Instance.boxesBroken++;
        GD.Print("Boxes broken: " + CounterBrokenBoxes.Instance.boxesBroken);

        QueueFree();
    }
}
