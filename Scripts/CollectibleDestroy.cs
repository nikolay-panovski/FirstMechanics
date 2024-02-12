using Godot;

public class CollectibleDestroy : Spatial
{
    public void OnBodyEnteredDestroyImmediately(Node body)
    {
        // expect the entering body to be intended to collide with coins, e.g. Player. set this up via collision layers.
        GD.Print("Queueing coin for deletion");
        QueueFree();
    }
}
