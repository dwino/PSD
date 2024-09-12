using System.Diagnostics;
using SadConsole.Readers;

namespace Balance;

public abstract class Map
{
    protected Dictionary<string, Func<Map>> mapDict = new Dictionary<string, Func<Map>>(){
        {"sleepingPod", ()=> new SleepingPodMap()},
        {"cryo",() => new EngineRoomMap()},
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
        var path = "Content/Maps/" + xpMapString + ".xp";
        RPImg = SadConsole.Readers.REXPaintImage.Load(new FileStream(path, FileMode.Open));

        _visibleMap = (CellSurface?)RPImg.ToCellSurface()[0]!;
        _walkableMap = (CellSurface?)RPImg.ToCellSurface()[1]!;
        _interactionMap = (CellSurface?)RPImg.ToCellSurface()[2]!;

        LoadInteractionMap();

        _xOffset = (GameSettings.GAME_WIDTH / 2) - (Width / 2);
        _yOffset = GameSettings.GAME_HEIGHT / 2 - Height / 2;
    }
    public int Width => RPImg.Width;
    public int Height => RPImg.Height;
    public REXPaintImage RPImg { get; set; }
    public Point StartingPosition { get; set; }
    public TravelNode TravelNode { get; set; }

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
                if (_interactionMap.GetGlyph(x, y) == '=')
                {
                    TravelNode = new TravelNode(this, (x, y));
                }
                LoadSpecificInteractionMap(x, y);
            }
        }

        CompleteTravelNode();
    }

    public abstract void LoadSpecificInteractionMap(int x, int y);
    public abstract void CompleteTravelNode();

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

    public override void CompleteTravelNode()
    {
        TravelNode.TravelToMap = "cryo";
        // var tmpMap = GetMap(TravelNode.TravelToMap);
        // TravelNode.PositionTravelToMap = tmpMap.StartingPosition;

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
    // TODO JE KAN DIT ABSTRACT MAKEN
    public override void CompleteTravelNode()
    {
        TravelNode.TravelToMap = "sleepingPod";
        // var tmpMap = GetMap(TravelNode.TravelToMap);
        // TravelNode.PositionTravelToMap = tmpMap.StartingPosition;
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


