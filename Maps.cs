using System.Diagnostics;
using System.Text;
using Balance.Ui;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using SadConsole.Readers;
using SadConsole.Renderers;

namespace Balance;

public class Cafeteria : Map
{
    public Cafeteria(GameEngine game, GameUi ui) : base("Cafeteria", game, ui)
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class Cryo1 : Map
{
    public Cryo1(GameEngine game, GameUi ui) : base("Cryo1", game, ui)
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class Cryo2 : Map
{
    public Cryo2(GameEngine game, GameUi ui) : base("Cryo2", game, ui)
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

    public Cryo3(GameEngine game, GameUi ui) : base("Cryo3", game, ui)
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
    public EngineObservationRoom(GameEngine game, GameUi ui) : base("EngineObservationRoom", game, ui)
    {
        Interactions.Add(new FullScreenInteraction("EngineRoomComputer", this, (8, 7)));

        var gaurdedAction = new MoveByAction(_ui, _game, this, (-1, 0));
        var alternativeAction = new MoveByAction(_ui, _game, this, (1, 0));
        var condition = new Condition { Dialogue = "Victor", Variable = "$permissionToEnterEngineRoom" };

        Interactions.Add(new GaurdingMapAutoInteraction("Door", this, (6, 9), gaurdedAction, alternativeAction, condition));


        var victorInteraction = new MapInteraction("Victor", this, (8, 8));
        var victor = new Entity("Victor", Color.Blue, 'V');
        victor.Position = victorInteraction.Position;
        victor.Interaction = victorInteraction;
        Entities.Add(victor);
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class EngineRoom : Map
{
    private SoundEffectInstance _engineSFX;

    public EngineRoom(GameEngine game, GameUi ui) : base("EngineRoom", game, ui)
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
    public FacilitiesCorridor(GameEngine game, GameUi ui) : base("FacilitiesCorridor", game, ui)
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class Hydroponics : Map
{
    public Hydroponics(GameEngine game, GameUi ui) : base("Hydroponics", game, ui)
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class MainCorridor : Map
{
    public MainCorridor(GameEngine game, GameUi ui) : base("MainCorridor", game, ui)
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class NorthCorridor : Map
{
    public NorthCorridor(GameEngine game, GameUi ui) : base("NorthCorridor", game, ui)
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class Room1 : Map
{
    public Room1(GameEngine game, GameUi ui) : base("Room1", game, ui)
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class Room2 : Map
{
    public Room2(GameEngine game, GameUi ui) : base("Room2", game, ui)
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class Room3 : Map
{
    public Room3(GameEngine game, GameUi ui) : base("Room3", game, ui)
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class Room4 : Map
{
    public Room4(GameEngine game, GameUi ui) : base("Room4", game, ui)
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class Room5 : Map
{
    public Room5(GameEngine game, GameUi ui) : base("Room5", game, ui)
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class Room6 : Map
{
    public Room6(GameEngine game, GameUi ui) : base("Room6", game, ui)
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class RoomsCorridor : Map
{
    public RoomsCorridor(GameEngine game, GameUi ui) : base("RoomsCorridor", game, ui)
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class Security : Map
{
    public Security(GameEngine game, GameUi ui) : base("Security", game, ui)
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class SecurityZone1 : Map
{
    public SecurityZone1(GameEngine game, GameUi ui) : base("SecurityZone1", game, ui)
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class SecurityZone2 : Map
{
    public SecurityZone2(GameEngine game, GameUi ui) : base("SecurityZone2", game, ui)
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class SecurityZone3 : Map
{
    public SecurityZone3(GameEngine game, GameUi ui) : base("SecurityZone3", game, ui)
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class SecurityZone4 : Map
{
    public SecurityZone4(GameEngine game, GameUi ui) : base("SecurityZone4", game, ui)
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class SecurityZoneCenter : Map
{
    public SecurityZoneCenter(GameEngine game, GameUi ui) : base("SecurityZoneCenter", game, ui)
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class ShowerNorth : Map
{
    public ShowerNorth(GameEngine game, GameUi ui) : base("ShowerNorth", game, ui)
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class ShowerSouth : Map
{
    public ShowerSouth(GameEngine game, GameUi ui) : base("ShowerSouth", game, ui)
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class Storage : Map
{
    public Storage(GameEngine game, GameUi ui) : base("Storage", game, ui)
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class Toilet : Map
{
    public Toilet(GameEngine game, GameUi ui) : base("Toilet", game, ui)
    {
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
public class YourRoom : Map
{
    public YourRoom(GameEngine game, GameUi ui) : base("YourRoom", game, ui)
    {
        var anoukInteraction = new MapInteraction("Anouk", this, (2, 2));
        var anouk = new Entity("Anouk", Color.Blue, 'A');
        anouk.Position = anoukInteraction.Position;
        anouk.Interaction = anoukInteraction;
        Entities.Add(anouk);
    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
    }
}
