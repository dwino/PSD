using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using SadConsole.Readers;
using SadConsole.Renderers;

namespace Balance;

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
    private Stopwatch _stopwatch;
    private Stopwatch _stopwatch1;
    private Stopwatch _stopwatch2;


    private int _tint;
    private int _tintOffset;
    private int _tint1;
    private int _tintOffset1;

    public Cryo3() : base("Cryo3")
    {
        _stopwatch = new Stopwatch();
        _stopwatch.Start();
        _stopwatch1 = new Stopwatch();
        _stopwatch1.Start();
        _stopwatch2 = new Stopwatch();
        _stopwatch2.Start();


        _tint = 128;
        _tintOffset = 3;
        _tint1 = 128;
        _tintOffset1 = 12;
    }

    public Point AnimationPosition_a { get; set; }
    public Point AnimationPosition_b { get; set; }
    public Point AnimationPosition_c { get; set; }
    public Point AnimationPosition_d { get; set; }
    public Point AnimationPosition_e { get; set; }
    public Point AnimationPosition_f { get; set; }
    public Point AnimationPosition_g { get; set; }
    public Point AnimationPosition_h { get; set; }
    public Point AnimationPosition_i { get; set; }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
        if (_interactionMap.GetGlyph(x, y) == 'a')
        {
            AnimationPosition_a = (x, y);
        }
        if (_interactionMap.GetGlyph(x, y) == 'b')
        {
            AnimationPosition_b = (x, y);
        }
        if (_interactionMap.GetGlyph(x, y) == 'c')
        {
            AnimationPosition_c = (x, y);
        }
        if (_interactionMap.GetGlyph(x, y) == 'd')
        {
            AnimationPosition_d = (x, y);
        }
        if (_interactionMap.GetGlyph(x, y) == 'e')
        {
            AnimationPosition_e = (x, y);
        }
        if (_interactionMap.GetGlyph(x, y) == 'f')
        {
            AnimationPosition_f = (x, y);
        }
        if (_interactionMap.GetGlyph(x, y) == 'g')
        {
            AnimationPosition_g = (x, y);
        }
        if (_interactionMap.GetGlyph(x, y) == 'h')
        {
            AnimationPosition_h = (x, y);
        }
        if (_interactionMap.GetGlyph(x, y) == 'i')
        {
            AnimationPosition_i = (x, y);
        }
    }

    public override void Draw(Console console)
    {
        base.Draw(console);

        if (_stopwatch.ElapsedMilliseconds < Helper.Rnd.Next(500, 2500))
        {
            console.SetBackground(AnimationPosition_a.X + _xOffset, AnimationPosition_a.Y + _yOffset, new SadRogue.Primitives.Color(217, 0, 0));
        }
        else
        {
            console.SetBackground(AnimationPosition_a.X + _xOffset, AnimationPosition_a.Y + _yOffset, new SadRogue.Primitives.Color(25, 25, 25));
            if (_stopwatch.ElapsedMilliseconds > Helper.Rnd.Next(3000, 4500))
            {
                var sfx = AudioManager.MalfunctionSFX!.CreateInstance();
                sfx.Volume = 0.4f;
                sfx.Play();

                _stopwatch.Restart();
            }
        }

        if (_stopwatch2.ElapsedMilliseconds > Helper.Rnd.Next(1000, 2000))
        {
            var rnd = Helper.Rnd.Next(3);
            if (rnd == 0)
            {
                VisibleMap.SetGlyph(AnimationPosition_b.X, AnimationPosition_b.Y, 45);
            }
            if (rnd == 1)
            {
                VisibleMap.SetGlyph(AnimationPosition_b.X, AnimationPosition_b.Y, 61);
            }
            if (rnd == 2)
            {
                VisibleMap.SetGlyph(AnimationPosition_b.X, AnimationPosition_b.Y, 240);
            }
            _stopwatch2.Restart();
        }


        if (_stopwatch1.ElapsedMilliseconds > 5)
        {
            console.SetForeground(AnimationPosition_c.X + _xOffset, AnimationPosition_c.Y + _yOffset, new SadRogue.Primitives.Color(_tint, _tint, _tint));
            console.SetForeground(AnimationPosition_d.X + _xOffset, AnimationPosition_d.Y + _yOffset, new SadRogue.Primitives.Color(_tint, _tint, _tint));
            console.SetForeground(AnimationPosition_e.X + _xOffset, AnimationPosition_e.Y + _yOffset, new SadRogue.Primitives.Color(_tint, _tint, _tint));
            console.SetForeground(AnimationPosition_f.X + _xOffset, AnimationPosition_f.Y + _yOffset, new SadRogue.Primitives.Color(_tint, _tint, _tint));
            console.SetForeground(AnimationPosition_g.X + _xOffset, AnimationPosition_g.Y + _yOffset, new SadRogue.Primitives.Color(_tint, _tint, _tint));
            console.SetForeground(AnimationPosition_h.X + _xOffset, AnimationPosition_h.Y + _yOffset, new SadRogue.Primitives.Color(_tint, _tint, _tint));
            _tint += _tintOffset;

            if (_tint >= 255)
            {
                _tint = 255;
                _tintOffset *= -1;
            }
            if (_tint <= 0)
            {
                _tint = 0;
                _tintOffset *= -1;
            }
            _stopwatch1.Restart();
        }

        console.SetForeground(AnimationPosition_i.X + _xOffset, AnimationPosition_i.Y + _yOffset, new SadRogue.Primitives.Color(_tint1, 0, 0));
        _tint1 += _tintOffset1;

        if (_tint1 >= 255)
        {
            _tint1 = 255;
            _tintOffset1 *= -1;
        }
        if (_tint1 <= 0)
        {
            _tint1 = 0;
            _tintOffset1 *= -1;
        }

    }
}
public class EngineObservationRoom : Map
{
    public EngineObservationRoom() : base("EngineObservationRoom")
    {
        Interactions.Add(new FullScreenInteraction("EngineRoomComputer", (8, 7)));
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class EngineRoom : Map
{
    private SoundEffectInstance _engineSFX;

    public EngineRoom() : base("EngineRoom")
    {
        _engineSFX = AudioManager.SpaceEngineSFX!.CreateInstance();

    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        MediaPlayer.Pause();
        _engineSFX.Play();
        _engineSFX.IsLooped = true;
    }
    public override void OnExit()
    {
        base.OnExit();
        MediaPlayer.Resume();
        _engineSFX.Stop();
    }

    public override void Draw(Console console)
    {
        base.Draw(console);


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
