using Godot;
using System;

// C# Autoload singleton -> actually implement the Singleton pattern unlike with GDScript.
// https://www.reddit.com/r/godot/comments/12xczyu/can_i_ask_for_an_example_of_c_autoload/
public class CounterCollectedCoins : Node
{
    private static CounterCollectedCoins _instance;
    public static CounterCollectedCoins Instance { get { return _instance; } }

    public int coinsCollected;

    public override void _EnterTree()
    {
        if (_instance != null)  // if another singleton reference existed...
        {
            coinsCollected = _instance.coinsCollected;  // ...copy over the information it held
            _instance.Free();                           // ...and delete it immediately before assigning this
        }

        _instance = this;
    }
}
