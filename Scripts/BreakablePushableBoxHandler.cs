using Godot;
using System;

public class BreakablePushableBoxHandler : RigidBody
{
    [Export] private float breakingWallCollisionVelocity = 1f;
    // actually exporting Node-derived properties instead of bare NodePaths is only available in 4.X:
    // https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_exports.html#nodes
    // a PropertyHint export attribute for this type restriction is also not available in C#, at least here on 3.5.
    //[Export] private PushableKickForceModifier kickable;
    // solution? not in this project. bare GetNode("pathname") instead to adhere to "call down, signal up".

    // final version: the object is assumed to have all properties (kickable, wallBreakable, playerStompable) by default, non-adjustable,
    // and is hardcoded between itself and the player object as such. this is because of all different involved body interactions.
    // not sure whether anything more designer-/Inspector-adjustable is possible in this case.

    public void OnBodyEnteredWallBreakable(Node body)
    {
        if (!body.IsInGroup("Player"))
        {
            if (LinearVelocity.Length() > breakingWallCollisionVelocity)
            {
                QueueFree();

                // TODO: instantiate predefined objects (coins) by predefined structure (moving + random starting vector)
            }
        }
    }
}
