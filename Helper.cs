using System;
using System.Diagnostics;
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

public class PointFloat(float x, float y)
{
    public float X { get; set; } = x;
    public float Y { get; set; } = y;
    public Point GetPoint()
    {
        return new Point((int)X, (int)Y);
    }
    public PointFloat GetCopy()
    {
        return new PointFloat(X, Y);
    }
}

public class Particle
{
    private Stopwatch _stopwatch;
    private int _lifeSteps;
    public Particle(int lifeSteps, PointFloat position, PointFloat velocity, char glyph, Color fgColor, Color bgColor)
    {
        _stopwatch = new Stopwatch();
        _lifeSteps = lifeSteps;

        Died = false;
        Position = position;
        Velocity = velocity;
        Glyph = glyph;
        FGColor = fgColor;
        BGColor = bgColor;

        _stopwatch.Start();
    }
    public bool Died { get; set; }
    public PointFloat Position { get; set; }
    public PointFloat Velocity { get; set; }
    public char Glyph { get; set; }
    public Color FGColor { get; set; }
    public Color BGColor { get; set; }

    public void Update()
    {
        _lifeSteps--;

        byte r, g, b, a = 0;

        FGColor.Deconstruct(out r, out g, out b, out a);
        a--;
        FGColor = FGColor.SetAlpha(a);

        BGColor.Deconstruct(out r, out g, out b, out a);
        a--;
        BGColor = BGColor.SetAlpha(a);
        if (_lifeSteps <= 0)
        {
            Died = true;
        }
        else
        {
            Position.X += Velocity.X;
            Position.Y += Velocity.Y;

            if (OutOfBounds())
            {
                Died = true;
            }

        }

        System.Console.WriteLine("Position: " + Position.X + ", " + Position.Y);
        System.Console.WriteLine("Velocity: " + Velocity.X + ", " + Velocity.Y);

    }

    private bool OutOfBounds()
    {
        var position = Position.GetPoint();

        return position.X <= 0 || position.X >= Program.Engine.Map.Width || position.Y <= 0 || position.Y >= Program.Engine.Map.Height;

    }



}