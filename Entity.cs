using SadRogue.Primitives;


namespace Balance;

public class Entity
{

    public Entity(int life, int black, int blackBalance, int white, int whiteBalance, Color color, char glyph)
    {
        Life = life;
        LifeMax = life;
        Black = black;
        BlackBalance = blackBalance;
        White = white;
        WhiteBalance = whiteBalance;
        Color = color;
        Glyph = glyph;
        Position = (-1, -1);

        NextAction = null;

    }
    public int Life { get; set; }
    public int LifeMax { get; set; }

    public int Black { get; set; }
    public int BlackBalance { get; set; }
    public int White { get; set; }
    public int WhiteBalance { get; set; }


    public Color Color { get; set; }
    public char Glyph { get; set; }

    public Point Position { get; set; }

    public Action? NextAction { get; set; }

    public static Entity GenerateEntity()
    {
        return new Entity(50, 10, 25, 40, 25, Color.Red, 'r');
    }


}

public class Player : Entity
{
    public Player() : base(100, 50, 50, 50, 50, Color.White, '@')
    {
    }
}

