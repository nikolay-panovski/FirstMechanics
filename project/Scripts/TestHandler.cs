using Godot;

public class TestHandler : Node
{
    // This almost gets us close to Unity Inspector assignable variables.
    // Except that the Node passed still needs to be got from GetNode, without type safety (throws on incorrect type).
    // Godot does it (in InterpolatedCamera) via directly casting, Object::cast_to<T> (C++).
    //[Export] private NodePath genericGameObjectRef;

    public void OnCollectibleBodyEnteredPrint()
    {
        // autoload -> direct singleton
        CounterCollectedCoins.Instance.coinsCollected++;

        GD.Print("Coins collected: " + CounterCollectedCoins.Instance.coinsCollected);
    }
}
