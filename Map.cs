using System.Diagnostics;
using SadConsole.Readers;

namespace Balance;

public class Map : REXPaintImage
{
    private Stopwatch _stopwatch;
    private Stopwatch _stopwatch1;
    private CellSurface _visibleMap;
    private CellSurface _walkableMap;
    private CellSurface _interactionMap;
    public Map(REXPaintImage rpImage) : base(rpImage.Width, rpImage.Height)
    {
        _stopwatch = new Stopwatch();
        _stopwatch.Start();

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

        }
        _stopwatch1 = new Stopwatch();
        _stopwatch1.Start();

    }

    public REXPaintImage RPImg { get; set; }
    public Point StartingPosition { get; set; }
    public TravelNode TravelNode { get; set; }
    public Point AnimationPosition1 { get; set; }
    public Point AnimationPosition2 { get; set; }
    public Point AnimationPosition3 { get; set; }

    public void Draw(Console console)
    {
        int x = (GameSettings.GAME_WIDTH / 2) - (Width / 2);
        int y = GameSettings.GAME_HEIGHT / 2 - Height / 2;
        _visibleMap.Copy(console.Surface, x, y);


        if (_stopwatch.ElapsedMilliseconds < Helper.Rnd.Next(500, 2500))
        {
            //console.SetForeground(AnimationPosition.X + x, AnimationPosition.Y + y, new SadRogue.Primitives.Color(0, 217, 0));
            console.SetBackground(AnimationPosition2.X + x, AnimationPosition2.Y + y, new SadRogue.Primitives.Color(217, 0, 0));

        }
        else
        {
            //console.SetForeground(AnimationPosition.X + x, AnimationPosition.Y + y, new SadRogue.Primitives.Color(128, 128, 128));
            console.SetBackground(AnimationPosition2.X + x, AnimationPosition2.Y + y, new SadRogue.Primitives.Color(25, 25, 25));
            if (_stopwatch.ElapsedMilliseconds > Helper.Rnd.Next(3000, 4500))
            {
                var sfx = AudioManager.MalfunctionSFX.CreateInstance();
                sfx.Volume = 0.1f;
                sfx.Play();

                _stopwatch.Restart();

            }

        }

        if (_stopwatch1.ElapsedMilliseconds < 4000)
        {
            //console.SetForeground(AnimationPosition.X + x, AnimationPosition.Y + y, new SadRogue.Primitives.Color(0, 217, 0));
            console.SetBackground(AnimationPosition1.X + x, AnimationPosition1.Y + y, new SadRogue.Primitives.Color(0, 217, 0));
            console.SetBackground(AnimationPosition3.X + x, AnimationPosition3.Y + y, new SadRogue.Primitives.Color(0, 217, 0));

        }
        else
        {
            //console.SetForeground(AnimationPosition.X + x, AnimationPosition.Y + y, new SadRogue.Primitives.Color(128, 128, 128));
            console.SetBackground(AnimationPosition1.X + x, AnimationPosition1.Y + y, new SadRogue.Primitives.Color(25, 25, 25));
            console.SetBackground(AnimationPosition3.X + x, AnimationPosition3.Y + y, new SadRogue.Primitives.Color(25, 25, 25));

            if (_stopwatch1.ElapsedMilliseconds > 5000)
            {
                _stopwatch1.Restart();

            }

        }

    }


    public static Map LoadMap(string fileName)
    {
        var rpImg = SadConsole.Readers.REXPaintImage.Load(new FileStream(fileName, FileMode.Open));

        var newMap = new Map(rpImg);

        return newMap;
    }
}


