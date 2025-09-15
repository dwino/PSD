using System.Diagnostics;
using System.Text;
using Balance.Ui;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using SadConsole.Readers;
using SadConsole.Renderers;
using Yarn;

namespace Balance;


public static class MapMemoryHelper
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
                                    {(6,1), ("Cryo2", (1,1))},
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

    public static Map GetMap(string xpMapString)
    {
        // Dark Magic, dont change
        Func<Map> func;
        return MapDict!.TryGetValue(xpMapString, out func!)
            ? func() // invoking the delegate creates the instance of the brand object
            : null!;  // brandName was not in the dictionary
    }

    public static Dictionary<string, CustomMemoryVariableStore> DialogueMemory = new Dictionary<string, CustomMemoryVariableStore>();
    public static void LoadDialogueMemory()
    {
        if (File.Exists("DialogueMemory.txt"))
        {
            string saveFile = File.ReadAllText("DialogueMemory.txt");
            StringReader strReader = new StringReader(saveFile);

            string? line = strReader.ReadLine();
            string key = "";


            while (line != null)
            {
                var lineParts = line.Split(':');

                if (lineParts[0] == "Dialogue")
                {
                    key = lineParts[1];
                }
                else
                {
                    DialogueMemory[key] = new CustomMemoryVariableStore();
                    DialogueMemory[key].DialogueVariables.Add(lineParts[0], bool.Parse(lineParts[1]));
                }
                line = strReader.ReadLine();

            }
        }
    }


    public static List<Trigger> Triggers = new List<Trigger>(){
        new Trigger(() => { return Program.Engine!.Map.XpMapString == "EngineRoom" &&
                            !TriggerMemory.Contains("visitedEngineRoomFirstTime"); },
                    () => { Program.Ui!.GameScreen.AddAnimation(new BasicTextScreenAnimation("PeaceAtLast"));
                            TriggerMemory.Add("visitedEngineRoomFirstTime"); }
                    ),
        new Trigger(() => { return Program.Engine!.Map.XpMapString == "Hydroponics" &&
                            !TriggerMemory.Contains("visitedHydroponicsFirstTime"); },
                    () => { Program.Ui!.GameScreen.AddAnimation(new ColoredBackgroundTextScreen("HydroponicsFirstTime"));
                            TriggerMemory.Add("visitedHydroponicsFirstTime"); }
                    ),

    };

    public static HashSet<string> TriggerMemory = new HashSet<string>();

    public static void LoadTriggerMemory()
    {
        if (File.Exists("TriggerMemory.txt"))
        {
            string saveFile = File.ReadAllText("TriggerMemory.txt");
            StringReader strReader = new StringReader(saveFile);

            string? line = strReader.ReadLine();

            while (line != null)
            {
                TriggerMemory.Add(line);
                line = strReader.ReadLine();
            }
        }
    }
}