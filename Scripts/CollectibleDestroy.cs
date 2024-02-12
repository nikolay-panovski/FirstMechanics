using Godot;

public class CollectibleDestroy : Spatial
{
    public void OnBodyEnteredDestroyImmediately(Node body)
    {
        // the bad way of detecting specific collision, compare to comparing object names (or lesser, CompareTag) in Unity
        if (body.Name == "PlayerCylinder")
        {
            GD.Print("Queueing coin for deletion");
            QueueFree();
        }
        else
        {
            GD.Print("Entered body name: " + body.Name + ". You might want to remove this collision and focus on collision with the Player.");
        }
    }
}
