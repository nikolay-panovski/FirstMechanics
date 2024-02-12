using Godot;

public class CollectibleDestroy : Spatial
{
    [Export] private string destroyAnimationName;

    [Signal] delegate void CollectibleBodyEntered();

    public void OnBodyEnteredDestroyImmediately(Node body)
    {
        // expect the entering body to be intended to collide with coins, e.g. Player. set this up via collision layers.
        GD.Print("Queueing coin for deletion");
        QueueFree();
    }

    public void OnBodyEnteredPlayDestroyAnimation(Node body)
    {
        AnimationPlayer player = GetNodeOrNull<AnimationPlayer>(new NodePath("DestructionPlayer"));
        if (!string.IsNullOrEmpty(destroyAnimationName) && player != null)
        {
            player.Play(destroyAnimationName);
        }
        else OnBodyEnteredDestroyImmediately(body); // "graceful" fallback
    }

    public void OnBodyEnteredEmit(Node body)
    {
        EmitSignal(nameof(CollectibleBodyEntered));
    }

    public void OnAnimationFinished(string animName)
    {
        if (animName == destroyAnimationName)
        {
            GD.Print("Queueing coin for deletion");
            QueueFree();
        }
    }
}
