using Godot;
using System;

// !! Always place in pairs and link in Inspector !!
public class PortalHandler : Spatial
{
    [Export] private NodePath linkedPortalPath;
    private PortalHandler linkedPortal;
    [Export] private float cooldownTime = 1f;
    public Timer cooldownTimer { get; private set; }

    public override void _Ready()
    {
        linkedPortal = GetNode<PortalHandler>(linkedPortalPath);
        cooldownTimer = GetNode<Timer>("CooldownTimer");
    }

    // For simplicity assume that either the only body that can enter is the player,
    // or any body that enters is sensible to teleport.
    public void OnBodyEnteredTeleport(Node body)
    {
        if (cooldownTimer.TimeLeft == 0 && linkedPortal.cooldownTimer.TimeLeft == 0)
        {
            (body as Spatial).GlobalTranslation = linkedPortal.GlobalTranslation;
            cooldownTimer.Start(cooldownTime);
            linkedPortal.cooldownTimer.Start(cooldownTime);
        }
    }
}
