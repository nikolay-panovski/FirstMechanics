using Godot;
using System;

// !! Always place in pairs and link in Inspector !!
public class PortalHandler : Spatial
{
    [Export] private string newLevelFilePath;
    [Export] private NodePath linkedPortalPath;
    private PortalHandler linkedPortal;
    [Export] private float cooldownTime = 1f;
    public Timer cooldownTimer { get; private set; }

    public override void _Ready()
    {
        if (linkedPortalPath != null) linkedPortal = GetNodeOrNull<PortalHandler>(linkedPortalPath);
        cooldownTimer = GetNode<Timer>("CooldownTimer");
    }


    public void OnBodyEnteredTeleport(Node body)
    {
        if (body.IsInGroup("Player"))
        {
            // crappily switching between level teleport and local teleport:
            if (!string.IsNullOrEmpty(newLevelFilePath))
            {
                if (cooldownTimer.TimeLeft == 0)
                {
                    cooldownTimer.Start(cooldownTime);
                    (body as Spatial).GlobalTranslation = Vector3.Zero;
                    Error err = GetTree().ChangeScene(newLevelFilePath);
                    //GD.Print(err);
                    //GD.Print(GetTree().CurrentScene);
                }
            }

            else if (cooldownTimer.TimeLeft == 0 && linkedPortal.cooldownTimer.TimeLeft == 0)
            {
                (body as Spatial).GlobalTranslation = linkedPortal.GlobalTranslation;
                cooldownTimer.Start(cooldownTime);
                linkedPortal.cooldownTimer.Start(cooldownTime);
            }
        }
    }
}
