using Godot;
using System;

// C# Autoload singleton -> actually implement the Singleton pattern unlike with GDScript.
// https://www.reddit.com/r/godot/comments/12xczyu/can_i_ask_for_an_example_of_c_autoload/
public class CounterBrokenBoxes : Node
{
    private static CounterBrokenBoxes _instance;
    public static CounterBrokenBoxes Instance { get { return _instance; } }

    public int boxesBroken;

    public override void _EnterTree()
    {
        if (_instance != null)
        {
            boxesBroken = _instance.boxesBroken;
            _instance.Free();
        }

        _instance = this;
    }
}
