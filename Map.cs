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

    public int XOffset;
    public int YOffset;
    public CellSurface VisibleMap { get; set; }
    public CellSurface WalkableMap { get; set; }
    protected CellSurface _interactionMap;

    protected Map(string xpMapString)
    {
        ShadowMaps = new List<ShadowMap>();
        XpMapString = xpMapString;
        var path = "Content/Maps/" + xpMapString + ".xp";
        RPImg = SadConsole.Readers.REXPaintImage.Load(new FileStream(path, FileMode.Open));

        VisibleMap = (CellSurface?)RPImg.ToCellSurface()[0]!;
        WalkableMap = (CellSurface?)RPImg.ToCellSurface()[1]!;
        _interactionMap = (CellSurface?)RPImg.ToCellSurface()[2]!;

        MapTouchUp();

        XOffset = (GameSettings.GAME_WIDTH / 2) - (Width / 2);
        YOffset = GameSettings.GAME_HEIGHT / 2 - Height / 2;

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

    public void MapTouchUp()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if ((char)WalkableMap.GetGlyph(x, y) == '2')
                {
                    VisibleMap.SetBackground(x, y, Color.Transparent);
                    VisibleMap.SetForeground(x, y, Color.Transparent);

                }
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
            var map = MapMemoryHelper.GetMap(shadowMap.MapString);

            var _visibleMap = new CellSurface(map.VisibleMap.Width, map.VisibleMap.Height);
            map.VisibleMap.Copy(_visibleMap);

            for (int x = 0; x < _visibleMap.Width; x++)
            {
                for (int y = 0; y < _visibleMap.Height; y++)
                {
                    var fgColor = _visibleMap.GetForeground(x, y);
                    var fgNewColor = fgColor.GetDarkest().GetDark();
                    var bgColor = _visibleMap.GetBackground(x, y);
                    var bgNewColor = bgColor.GetDarkest().GetDark();
                    _visibleMap.SetForeground(x, y, fgNewColor);
                    _visibleMap.SetBackground(x, y, bgNewColor);
                }
            }
            _visibleMap.Copy(Program.Ui.DrawConsole.Surface, shadowMap.X, shadowMap.Y);

            // _visibleMap.Copy(Program.Ui.DrawConsole.Surface, shadowMap.XOffset + x1, shadowMap.YOffset + y1);
        }



        // VisibleMap.Copy(Program.Ui.DrawConsole.Surface, x1, y1);

        VisibleMap.Copy(Program.Ui.DrawConsole.Surface, XOffset, YOffset);



        foreach (var entity in Entities)
        {
            int x = entity.Position.X + (GameSettings.GAME_WIDTH / 2) - (Width / 2);
            int y = entity.Position.Y + (GameSettings.GAME_HEIGHT / 2) - (Height / 2);
            Program.Ui.DrawConsole.Print(x, y, entity.Glyph.ToString(), entity.Color);
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
            Program.Ui.DrawConsole.Print(x, y, interactionOption, color);
        }
    }

}
