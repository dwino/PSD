using SadRogue.Primitives;


namespace Balance;

public class Entity
{

    public Entity(Color color, char glyph)
    {
        Color = color;
        Glyph = glyph;
        Position = (-1, -1);

        NextAction = null;
    }

    public Color Color { get; set; }
    public char Glyph { get; set; }

    public Point Position { get; set; }

    public Action? NextAction { get; set; }
}

public class Player : Entity
{
    public Player() : base(Color.White, '@')
    {
    }
}

