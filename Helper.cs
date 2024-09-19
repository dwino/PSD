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