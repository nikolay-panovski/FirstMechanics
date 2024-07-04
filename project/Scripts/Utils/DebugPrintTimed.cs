using Godot;

public static partial class Utils
{
    /// <summary>
    /// Prints to console via <see cref="GD.Print(object[])"/> multiple <paramref name="timesPerSecond"/>. Will fail to print if "one" time takes more than 17 milliseconds. Period success subject to floating point errors.
    /// </summary>
    public static void DebugPrintTimed(int timesPerSecond, params object[] what)
    {
        if ((Time.GetTicksMsec() % (1000f / timesPerSecond)) < 17f)
        {
            GD.Print(what);
        }
    }
}