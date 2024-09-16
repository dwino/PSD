using System.Diagnostics;
using SadConsole.Readers;

namespace Balance;

public abstract class Map
{
    protected Dictionary<string, Func<Map>> mapDict = new Dictionary<string, Func<Map>>(){
        {"Cafeteria",() => new Cafeteria()},
        {"Cryo1",() => new Cryo1()},
        {"Cryo2",() => new Cryo2()},
        {"Cryo3",() => new Cryo3()},
        {"EngineRoom",() => new EngineRoom()},
        {"FacilitiesCorridor",() => new FacilitiesCorridor()},
        {"Hydroponics",() => new Hydroponics()},
        {"MainCorridor",() => new MainCorridor()},
        {"NorthCorridor",() => new NorthCorridor()},
        {"Room1",() => new Room1()},
        {"Room2",() => new Room2()},
        {"Room3",() => new Room3()},
        {"Room4",() => new Room4()},
        {"Room5",() => new Room5()},
        {"Room6",() => new Room6()},
        {"RoomsCorridor",() => new RoomsCorridor()},
        {"Security",() => new Security()},
        {"SecurityZone1",() => new SecurityZone1()},
        {"SecurityZone2",() => new SecurityZone2()},
        {"SecurityZone3",() => new SecurityZone3()},
        {"SecurityZone4",() => new SecurityZone4()},
        {"SecurityZoneCenter",() => new SecurityZoneCenter()},
        {"ShowerNorth",() => new ShowerNorth()},
        {"ShowerSouth",() => new ShowerSouth()},
        {"Storage",() => new Storage()},
        {"Toilet",() => new Toilet()},
        {"YourRoom",() => new YourRoom()},


    };

    public static Dictionary<string, Dictionary<Point, (string, Point)>> InterMapDict
                            = new Dictionary<string, Dictionary<Point, (string, Point)>>(){
                                {"Cafeteria", new Dictionary<Point, (string, Point)>(){
                                    {(0,9), ("MainCorridor", (10,9))},
                                    {(6,0), ("FacilitiesCorridor", (1,5))},
                                }},
                                {"Cryo1", new Dictionary<Point, (string, Point)>(){
                                    {(10,1), ("SecurityZone1", (1,1))},
                                }},
                                {"Cryo2", new Dictionary<Point, (string, Point)>(){
                                    {(0,1), ("SecurityZone2", (7,1))},
                                }},
                                {"Cryo3", new Dictionary<Point, (string, Point)>(){
                                    {(9,0), ("SecurityZone3", (7,6))},
                                }},
                                {"EngineRoom", new Dictionary<Point, (string, Point)>(){
                                    {(10,9), ("Security", (1,9))},
                                }},
                                {"FacilitiesCorridor", new Dictionary<Point, (string, Point)>(){
                                    {(1,6), ("Cafeteria", (6,1))},
                                    {(0,5), ("Toilet", (3,3))},
                                    {(4,0), ("ShowerNorth", (2,3))},
                                    {(0,1), ("RoomsCorridor", (13,1))},
                                    {(2,5), ("ShowerSouth", (1,3))},
                                }},
                                {"Hydroponics", new Dictionary<Point, (string, Point)>(){
                                    {(20,4), ("NorthCorridor", (1,1))},
                                }},
                                {"MainCorridor", new Dictionary<Point, (string, Point)>(){
                                    {(1,0), ("NorthCorridor", (1,5))},
                                    {(0,9), ("Security", (8,9))},
                                    {(1,10), ("SecurityZoneCenter", (2,1))},
                                    {(5,8), ("Storage", (3,5))},
                                    {(11,9), ("Cafeteria", (1,9))},
                                }},
                                {"NorthCorridor", new Dictionary<Point, (string, Point)>(){
                                    {(1,6), ("MainCorridor", (1,1))},
                                    {(1,0), ("Hydroponics", (20,3))},
                                    {(2,1), ("RoomsCorridor", (1,1))},
                                }},
                                {"Room1", new Dictionary<Point, (string, Point)>(){
                                    {(3,4), ("RoomsCorridor", (3,1))},
                                }},
                                {"Room2", new Dictionary<Point, (string, Point)>(){
                                    {(3,4), ("RoomsCorridor", (7,1))},
                                }},
                                {"Room3", new Dictionary<Point, (string, Point)>(){
                                    {(3,4), ("RoomsCorridor", (11,1))},
                                }},
                                {"Room4", new Dictionary<Point, (string, Point)>(){
                                    {(1,4), ("RoomsCorridor", (13,1))},
                                }},
                                {"Room5", new Dictionary<Point, (string, Point)>(){
                                    {(4,3), ("RoomsCorridor", (5,5))},
                                }},
                                {"Room6", new Dictionary<Point, (string, Point)>(){
                                    {(0,3), ("RoomsCorridor", (5,5))},
                                }},
                                {"RoomsCorridor", new Dictionary<Point, (string, Point)>(){
                                    {(0,1), ("NorthCorridor", (1,1))},
                                    {(3,0), ("Room1", (3,3))},
                                    {(7,0), ("Room2", (3,3))},
                                    {(11,0), ("Room3", (3,3))},
                                    {(13,0), ("Room4", (1,3))},
                                    {(14,1), ("FacilitiesCorridor", (1,1))},
                                    {(4,5), ("Room5", (3,3))},
                                    {(6,5), ("Room6", (1,3))},
                                }},
                                {"Security", new Dictionary<Point, (string, Point)>(){
                                    {(0,9), ("EngineRoom", (9,9))},
                                    {(9,9), ("MainCorridor", (1,9))},
                                }},
                                {"SecurityZone1", new Dictionary<Point, (string, Point)>(){
                                    {(0,1), ("Cryo1", (9,1))},
                                    {(8,1), ("SecurityZoneCenter", (1,1))},
                                }},
                                {"SecurityZone2", new Dictionary<Point, (string, Point)>(){
                                    {(8,1), ("Cryo2", (1,1))},
                                    {(0,1), ("SecurityZoneCenter", (3,1))},
                                }},

                                {"SecurityZone3", new Dictionary<Point, (string, Point)>(){
                                    {(7,0), ("SecurityZoneCenter", (1,2))},
                                    {(8,4), ("SecurityZone4", (1,4))},
                                    {(7,7), ("Cryo3", (9,1))},
                                }},
                                {"SecurityZone4", new Dictionary<Point, (string, Point)>(){
                                    {(0,4), ("SecurityZone3", (7,4))},
                                }},
                                {"SecurityZoneCenter", new Dictionary<Point, (string, Point)>(){
                                    {(0,1), ("SecurityZone1", (7,1))},
                                    {(4,1), ("SecurityZone2", (1,1))},
                                    {(1,3), ("SecurityZone3", (7,1))},
                                    {(2,0), ("MainCorridor", (1,9))},
                                }},
                                {"ShowerNorth", new Dictionary<Point, (string, Point)>(){
                                    {(2,4), ("FacilitiesCorridor", (4,1))},
                                }},
                                {"ShowerSouth", new Dictionary<Point, (string, Point)>(){
                                    {(0,3), ("FacilitiesCorridor", (1,5))},
                                }},
                                {"Toilet", new Dictionary<Point, (string, Point)>(){
                                    {(4,3), ("FacilitiesCorridor", (1,5))},
                                }},
                                {"Storage", new Dictionary<Point, (string, Point)>(){
                                    {(4,5), ("YourRoom", (1,2))},
                                    {(3,6), ("MainCorridor", (5,9))},
                                }},
                                {"YourRoom", new Dictionary<Point, (string, Point)>(){
                                    {(0,2), ("Storage", (3,5))},
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

        Interactions = new List<DialogRunner>();
        AvailableInteractions = new List<DialogRunner>();
        CurrentInteractionIndex = -1;

    }
    public string XpMapString { get; set; }
    public int Width => RPImg.Width;
    public int Height => RPImg.Height;
    public REXPaintImage RPImg { get; set; }
    public List<DialogRunner> Interactions { get; set; }
    public List<DialogRunner> AvailableInteractions { get; set; }
    public int CurrentInteractionIndex { get; set; }
    public DialogRunner CurrentInteraction
    {
        get
        {
            if (CurrentInteractionIndex != -1 && CurrentInteractionIndex < AvailableInteractions.Count)
            {
                return AvailableInteractions[CurrentInteractionIndex];
            }
            else
            {
                return null;
            }
        }
    }


    public void LoadInteractionMap()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                LoadSpecificInteractionMap(x, y);
            }
        }
    }
    public abstract void LoadSpecificInteractionMap(int x, int y);
    public virtual void Draw(Console console)
    {
        _visibleMap.Copy(console.Surface, _xOffset, _yOffset);

        for (int i = 0; i < AvailableInteractions.Count; i++)
        {
            var availableInteraction = Interactions[i];

            string interactionOption = i + " - " + availableInteraction.Name;
            Color color = Color.White;
            if (i == CurrentInteractionIndex)
            {
                interactionOption = ">" + interactionOption;
                color = new Color(0, 217, 0);
            }
            int y = i + 1;
            int x = GameSettings.GAME_WIDTH - 2 - interactionOption.Length;
            console.Print(x, y, interactionOption, color);
        }
    }
}

public class Cafeteria : Map
{
    public Cafeteria() : base("Cafeteria")
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class Cryo1 : Map
{
    public Cryo1() : base("Cryo1")
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class Cryo2 : Map
{
    public Cryo2() : base("Cryo2")
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class Cryo3 : Map
{
    public Cryo3() : base("Cryo3")
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class EngineRoom : Map
{
    public EngineRoom() : base("EngineRoom")
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class FacilitiesCorridor : Map
{
    public FacilitiesCorridor() : base("FacilitiesCorridor")
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class Hydroponics : Map
{
    public Hydroponics() : base("Hydroponics")
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class MainCorridor : Map
{
    public MainCorridor() : base("MainCorridor")
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class NorthCorridor : Map
{
    public NorthCorridor() : base("NorthCorridor")
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class Room1 : Map
{
    public Room1() : base("Room1")
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class Room2 : Map
{
    public Room2() : base("Room2")
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class Room3 : Map
{
    public Room3() : base("Room3")
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class Room4 : Map
{
    public Room4() : base("Room4")
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class Room5 : Map
{
    public Room5() : base("Room5")
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class Room6 : Map
{
    public Room6() : base("Room6")
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class RoomsCorridor : Map
{
    public RoomsCorridor() : base("RoomsCorridor")
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class Security : Map
{
    public Security() : base("Security")
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class SecurityZone1 : Map
{
    public SecurityZone1() : base("SecurityZone1")
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class SecurityZone2 : Map
{
    public SecurityZone2() : base("SecurityZone2")
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class SecurityZone3 : Map
{
    public SecurityZone3() : base("SecurityZone3")
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class SecurityZone4 : Map
{
    public SecurityZone4() : base("SecurityZone4")
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class SecurityZoneCenter : Map
{
    public SecurityZoneCenter() : base("SecurityZoneCenter")
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class ShowerNorth : Map
{
    public ShowerNorth() : base("ShowerNorth")
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class ShowerSouth : Map
{
    public ShowerSouth() : base("ShowerSouth")
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class Storage : Map
{
    public Storage() : base("Storage")
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class Toilet : Map
{
    public Toilet() : base("Toilet")
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class YourRoom : Map
{
    public YourRoom() : base("YourRoom")
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}

public class SleepingPodMap : Map
{
    public SleepingPodMap() : base("sleepingPod")
    {
        var bedInteraction = new DialogRunner("Anouk", (1, 2), "Start");//Helper.LoadInteraction(this, "Anouk", (1, 2));
        Interactions.Add(bedInteraction);
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



