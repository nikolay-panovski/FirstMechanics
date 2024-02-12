using Godot;

public class CollectibleDestroy : Spatial
{
    public void OnBodyEnteredDestroyImmediately(Node body)
    {
        GD.Print("Queueing coin for deletion");
        QueueFree();
    }
}
