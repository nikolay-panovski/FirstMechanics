using Godot;

public class CollectibleHandler : Node
{
    [Export] private string destroyAnimationName;

    [Signal] delegate void CollectibleBodyEntered();

    public void OnBodyEnteredDestroyImmediately(Node body)
    {
        // expect the entering body to be intended to collide with coins, e.g. Player. set this up via collision layers.
        QueueFree();
    }

    public void OnBodyEnteredPlayDestroyAnimation(Node body)
    {
        AnimationPlayer player = GetNodeOrNull<AnimationPlayer>(new NodePath("DestructionAnimation"));
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
        QueueFree();
    }
}
