using Godot;
using System;

public class ResetOnDeath : Area
{
    public void ResetOnPlayerDeath(Node body)
    {
        if (body.IsInGroup("Player"))
        {
            GetTree().ReloadCurrentScene();
            // and since the player is an autoload in this project, it needs to be "returned" manually:
            (body as Spatial).GlobalTranslation = Vector3.Zero;
        }
    }
}
