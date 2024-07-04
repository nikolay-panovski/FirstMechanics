using Godot;

public static partial class Utils
{
    /// <summary>
    /// Prints to console via <see cref="GD.Print(object[])"/> once per second. Will fail to print if the "once" takes more than 17 milliseconds.
    /// </summary>
    public static void DebugPrintPerSecond(params object[] what)
    {
        if ((Time.GetTicksMsec() % 1000f) < 17f)
        {
            GD.Print(what);
        }
    }
}