using Godot;

// Use this to control AnimationPlayers.
public class CollectiblePlayCollectedAnimation : Node
{
    [Export] private string animationName;
    private AnimationPlayer player;

    public override void _Ready()
    {
        player = GetParent<AnimationPlayer>();
    }

    public void OnCollectedPlayAnimation()
    {
        player.Play(animationName);
    }
}
