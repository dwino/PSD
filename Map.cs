using System.Diagnostics;
using System.Text;
using Balance.Ui;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using SadConsole.Readers;
using SadConsole.Renderers;
using Yarn;

namespace Balance;

public abstract class Map
{
    public static Dictionary<string, Func<Map>>? MapDict = new Dictionary<string, Func<Map>>()
    {
        {"Cafeteria",() => new Cafeteria()},
        {"Cryo1",() => new Cryo1()},
        {"Cryo2",() => new Cryo2()},
        {"Cryo3",() => new Cryo3()},
        {"EngineObservationRoom",() => new EngineObservationRoom()},
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
                                    {(10,1), ("SecurityZone1", (3,1))},
                                }},
                                {"Cryo2", new Dictionary<Point, (string, Point)>(){
                                    {(0,1), ("SecurityZone2", (5,1))},
                                }},
                                {"Cryo3", new Dictionary<Point, (string, Point)>(){
                                    {(9,0), ("SecurityZone3", (7,4))},
                                }},
                                {"EngineObservationRoom", new Dictionary<Point, (string, Point)>(){
                                    {(10,9), ("Security", (3,9))},
                                    {(4,9), ("EngineRoom", (3,9))},
                                }},
                                {"EngineRoom", new Dictionary<Point, (string, Point)>(){
                                    {(4,9), ("EngineObservationRoom", (7,9))},
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
                                    {(2,9), ("EngineObservationRoom", (9,9))},
                                    {(9,9), ("MainCorridor", (1,9))},
                                }},
                                {"SecurityZone1", new Dictionary<Point, (string, Point)>(){
                                    {(2,1), ("Cryo1", (9,1))},
                                    {(8,1), ("SecurityZoneCenter", (1,1))},
                                }},
                                {"SecurityZone2", new Dictionary<Point, (string, Point)>(){
                                    {(8,1), ("Cryo2", (1,1))},
                                    {(0,1), ("SecurityZoneCenter", (3,1))},
                                }},

                                {"SecurityZone3", new Dictionary<Point, (string, Point)>(){
                                    {(7,0), ("SecurityZoneCenter", (1,2))},
                                    {(8,4), ("SecurityZone4", (1,4))},
                                    {(7,5), ("Cryo3", (9,1))},
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
    public static Dictionary<string, MemoryVariableStore> MemoryPalace = new Dictionary<string, MemoryVariableStore>();
    public static Map GetMap(string xpMapString)
    {
        // Dark Magic, dont change
        Func<Map> func;
        return MapDict!.TryGetValue(xpMapString, out func!)
            ? func() // invoking the delegate creates the instance of the brand object
            : null!;  // brandName was not in the dictionary
    }

    protected int _xOffset;
    protected int _yOffset;
    public CellSurface VisibleMap { get; set; }
    protected CellSurface _walkableMap;
    protected CellSurface _interactionMap;

    protected Map(string xpMapString)
    {
        ShadowMaps = new List<ShadowMap>();
        XpMapString = xpMapString;
        var path = "Content/Maps/" + xpMapString + ".xp";
        RPImg = SadConsole.Readers.REXPaintImage.Load(new FileStream(path, FileMode.Open));

        VisibleMap = (CellSurface?)RPImg.ToCellSurface()[0]!;
        _walkableMap = (CellSurface?)RPImg.ToCellSurface()[1]!;
        _interactionMap = (CellSurface?)RPImg.ToCellSurface()[2]!;

        LoadInteractionMap();

        _xOffset = (GameSettings.GAME_WIDTH / 2) - (Width / 2);
        _yOffset = GameSettings.GAME_HEIGHT / 2 - Height / 2;

        Entities = new List<Entity>();
        Interactions = new List<MapBoundInteraction>();
        AvailableInteractions = new List<MapBoundInteraction>();
        CurrentInteractionIndex = -1;
        _currentInteraction = null!;

    }
    public List<ShadowMap> ShadowMaps;
    public string XpMapString { get; set; }
    public int Width => RPImg.Width;
    public int Height => RPImg.Height;
    public REXPaintImage RPImg { get; set; }
    public List<Entity> Entities { get; set; }
    public List<MapBoundInteraction> Interactions { get; set; }
    public List<MapBoundInteraction> AvailableInteractions { get; set; }
    public int CurrentInteractionIndex { get; set; }
    private MapBoundInteraction _currentInteraction;
    public MapBoundInteraction CurrentInteraction
    {
        get
        {
            if (_currentInteraction != null)
            {
                return _currentInteraction;
            }
            else if (CurrentInteractionIndex != -1 && CurrentInteractionIndex < AvailableInteractions.Count)
            {
                return AvailableInteractions[CurrentInteractionIndex];
            }
            else
            {
                return null!;
            }
        }
        set
        {
            _currentInteraction = value;
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

    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public virtual void Draw()
    {
        int x1 = GameSettings.GAME_WIDTH / 2 - Program.Engine.Player.Position.X;
        int y1 = GameSettings.GAME_HEIGHT / 2 - Program.Engine.Player.Position.Y;
        foreach (var shadowMap in ShadowMaps)
        {
            var map = GetMap(shadowMap.MapString);

            var _visibleMap = new CellSurface(map.VisibleMap.Width, map.VisibleMap.Height);
            map.VisibleMap.Copy(_visibleMap);

            for (int x = 0; x < _visibleMap.Width; x++)
            {
                for (int y = 0; y < _visibleMap.Height; y++)
                {
                    var fgColor = _visibleMap.GetForeground(x, y);
                    var fgNewColor = fgColor.GetDarkest().GetDarkest();
                    var bgColor = _visibleMap.GetBackground(x, y);
                    var bgNewColor = bgColor.GetDarkest().GetDarkest();
                    _visibleMap.SetForeground(x, y, fgNewColor);
                    _visibleMap.SetBackground(x, y, bgNewColor);
                }
            }
            _visibleMap.Copy(Program.Ui.Surface, shadowMap.X, shadowMap.Y);

            // _visibleMap.Copy(Program.Ui.Surface, shadowMap.XOffset + x1, shadowMap.YOffset + y1);
        }



        // VisibleMap.Copy(Program.Ui.Surface, x1, y1);

        VisibleMap.Copy(Program.Ui.Surface, _xOffset, _yOffset);



        foreach (var entity in Entities)
        {
            int x = entity.Position.X + (GameSettings.GAME_WIDTH / 2) - (Width / 2);
            int y = entity.Position.Y + (GameSettings.GAME_HEIGHT / 2) - (Height / 2);
            Program.Ui.Print(x, y, entity.Glyph.ToString(), entity.Color);
        }

        for (int i = 0; i < AvailableInteractions.Count; i++)
        {
            var availableInteraction = AvailableInteractions[i];

            string interactionOption = i + " - " + availableInteraction.Name;
            Color color = Color.White;
            if (i == CurrentInteractionIndex)
            {
                interactionOption = ">" + interactionOption;
                color = new Color(0, 217, 0);
            }
            int y = i + 1;
            int x = GameSettings.GAME_WIDTH - 2 - interactionOption.Length;
            Program.Ui.Print(x, y, interactionOption, color);
        }
    }

}
