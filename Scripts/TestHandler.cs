using Godot;

public class TestHandler : Node
{
    public void OnCollectibleBodyEnteredPrint()
    {
        GD.Print("Coin collected - signal received");
    }
}
