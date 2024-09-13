using System.Diagnostics;
using SadConsole.Readers;

namespace Balance;

public abstract class Map
{
    protected Dictionary<string, Func<Map>> mapDict = new Dictionary<string, Func<Map>>(){
        {"sleepingPod", ()=> new SleepingPodMap()},
        {"cryo",() => new EngineRoomMap()},
        {"corridor",() => new CorridorMap()},
        {"cafeteria",() => new CafeteriaMap()},

    };

    public static Dictionary<string, Dictionary<Point, (string, Point)>> InterMapDict
                            = new Dictionary<string, Dictionary<Point, (string, Point)>>(){
                                {"cryo", new Dictionary<Point, (string, Point)>(){
                                    {(24,6), ("corridor", (1,1))},
                                }},
                                {"sleepingPod", new Dictionary<Point, (string, Point)>(){
                                    {(3,3), ("corridor", (1,10))},
                                }},
                                {"corridor", new Dictionary<Point, (string, Point)>(){
                                    {(0,1), ("cryo", (23,6))},
                                    {(0,10), ("sleepingPod", (2,3))},
                                    {(11,9), ("cafeteria", (1,14))},

                                }},{"cafeteria", new Dictionary<Point, (string, Point)>(){
                                    {(0,14), ("corridor", (10,9))},
                                }},
                            };
    public Map GetMap(string xpMapString)
    {
        // Dark Magic, dont change
        Func<Map> func;
        return mapDict.TryGetValue(xpMapString, out func)
            ? func() // invoking the delegate creates the instance of the brand object
            : null;  // brandName was not in the dictionary
    }

    protected int _xOffset;
    protected int _yOffset;
    protected CellSurface _visibleMap;
    protected CellSurface _walkableMap;
    protected CellSurface _interactionMap;

    protected Map(string xpMapString) : base()
    {
        XpMapString = xpMapString;
        var path = "Content/Maps/" + xpMapString + ".xp";
        RPImg = SadConsole.Readers.REXPaintImage.Load(new FileStream(path, FileMode.Open));

        _visibleMap = (CellSurface?)RPImg.ToCellSurface()[0]!;
        _walkableMap = (CellSurface?)RPImg.ToCellSurface()[1]!;
        _interactionMap = (CellSurface?)RPImg.ToCellSurface()[2]!;

        LoadInteractionMap();

        _xOffset = (GameSettings.GAME_WIDTH / 2) - (Width / 2);
        _yOffset = GameSettings.GAME_HEIGHT / 2 - Height / 2;
    }
    public string XpMapString { get; set; }
    public int Width => RPImg.Width;
    public int Height => RPImg.Height;
    public REXPaintImage RPImg { get; set; }
    public Point StartingPosition { get; set; }

    public void LoadInteractionMap()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (_interactionMap.GetGlyph(x, y) == '@')
                {
                    StartingPosition = (x, y);
                }

                LoadSpecificInteractionMap(x, y);
            }
        }
    }
    public abstract void LoadSpecificInteractionMap(int x, int y);
    public virtual void Draw(Console console)
    {
        _visibleMap.Copy(console.Surface, _xOffset, _yOffset);
    }
}

public class SleepingPodMap : Map
{
    public SleepingPodMap() : base("sleepingPod")
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }


}

public class CorridorMap : Map
{
    public CorridorMap() : base("corridor")
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}

public class EngineRoomMap : Map
{
    private Stopwatch _stopwatch;
    private Stopwatch _stopwatch1;

    public EngineRoomMap() : base("cryo")
    {
        _stopwatch = new Stopwatch();
        _stopwatch.Start();
        _stopwatch1 = new Stopwatch();
        _stopwatch1.Start(); int x = (GameSettings.GAME_WIDTH / 2) - (Width / 2);
    }

    public Point AnimationPosition1 { get; set; }
    public Point AnimationPosition2 { get; set; }
    public Point AnimationPosition3 { get; set; }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
        if (_interactionMap.GetGlyph(x, y) == 'a')
        {
            AnimationPosition1 = (x, y);
        }
        if (_interactionMap.GetGlyph(x, y) == 'b')
        {
            AnimationPosition2 = (x, y);
        }
        if (_interactionMap.GetGlyph(x, y) == 'c')
        {
            AnimationPosition3 = (x, y);
        }

    }

    public override void Draw(Console console)
    {
        base.Draw(console);

        if (_stopwatch.ElapsedMilliseconds < Helper.Rnd.Next(500, 2500))
        {
            console.SetBackground(AnimationPosition2.X + _xOffset, AnimationPosition2.Y + _yOffset, new SadRogue.Primitives.Color(217, 0, 0));
        }
        else
        {
            console.SetBackground(AnimationPosition2.X + _xOffset, AnimationPosition2.Y + _yOffset, new SadRogue.Primitives.Color(25, 25, 25));
            if (_stopwatch.ElapsedMilliseconds > Helper.Rnd.Next(3000, 4500))
            {
                var sfx = AudioManager.MalfunctionSFX.CreateInstance();
                sfx.Volume = 0.3f;
                sfx.Play();

                _stopwatch.Restart();
            }
        }

        if (_stopwatch1.ElapsedMilliseconds < 4000)
        {
            console.SetBackground(AnimationPosition1.X + _xOffset, AnimationPosition1.Y + _yOffset, new SadRogue.Primitives.Color(0, 217, 0));
            console.SetBackground(AnimationPosition3.X + _xOffset, AnimationPosition3.Y + _yOffset, new SadRogue.Primitives.Color(0, 217, 0));

        }
        else
        {
            console.SetBackground(AnimationPosition1.X + _xOffset, AnimationPosition1.Y + _yOffset, new SadRogue.Primitives.Color(25, 25, 25));
            console.SetBackground(AnimationPosition3.X + _xOffset, AnimationPosition3.Y + _yOffset, new SadRogue.Primitives.Color(25, 25, 25));

            if (_stopwatch1.ElapsedMilliseconds > 5000)
            {
                _stopwatch1.Restart();
            }
        }
    }


}
public class CafeteriaMap : Map
{
    public CafeteriaMap() : base("cafeteria")
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}


