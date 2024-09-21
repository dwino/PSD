using System;
using Figgle;

namespace Balance;

public class Helper
{
    public static Random Rnd { get; set; } = new Random();
}

public struct Condition
{
    public string Dialogue { get; set; }
    public string Variable { get; set; }
}

public class ShadowMap
{
    public int X => XRef + XOffset;
    public int Y => YRef + YOffset;
    public string MapString { get; set; }
    public int XRef { get; set; }
    public int YRef { get; set; }
    public int XOffset { get; set; }
    public int YOffset { get; set; }

}