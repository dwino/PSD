using SadRogue.Primitives;


namespace Balance;

public class Entity
{

    public Entity(string name, Color color, char glyph)
    {
        Name = name;
        Color = color;
        Glyph = glyph;
        Position = (-1, -1);
        Interaction = null;
    }
    public string Name { get; set; }
    public Color Color { get; set; }
    public char Glyph { get; set; }
    public Point Position { get; set; }
    public DialogueRunner? Interaction { get; set; }
}

public class Player
{

    public Player()
    {
        Name = "You";
        Color = Color.White;
        Glyph = '@';
        Position = (-1, -1);

        NextAction = null;
    }
    public string Name { get; set; }
    public Color Color { get; set; }
    public char Glyph { get; set; }
    public Point Position { get; set; }

    public Action? NextAction { get; set; }
}

