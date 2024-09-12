using System;
using SadConsole.Readers;
using SharpFont;

namespace Balance;

public class Map : REXPaintImage
{
    private CellSurface _visibleMap;
    private CellSurface _walkableMap;
    private CellSurface _interactionMap;
    public Map(REXPaintImage rpImage) : base(rpImage.Width, rpImage.Height)
    {
        RPImg = rpImage;
        _visibleMap = (CellSurface?)rpImage.ToCellSurface()[0]!;
        _walkableMap = (CellSurface?)rpImage.ToCellSurface()[1]!;
        _interactionMap = (CellSurface?)rpImage.ToCellSurface()[2]!;

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
                    TravelNode = new TravelNode(this, (x, y), this, (0, 0));
                }
            }

        }
    }

    public REXPaintImage RPImg { get; set; }
    public Point StartingPosition { get; set; }
    public TravelNode TravelNode { get; set; }

    public void Draw(Console console)
    {
        int x = (GameSettings.GAME_WIDTH / 2) - (Width / 2);
        int y = GameSettings.GAME_HEIGHT / 2 - Height / 2;
        _visibleMap.Copy(console.Surface, x, y);
    }

    public static Map LoadMap(string fileName)
    {
        var rpImg = SadConsole.Readers.REXPaintImage.Load(new FileStream(fileName, FileMode.Open));

        var newMap = new Map(rpImg);

        return newMap;
    }
}


