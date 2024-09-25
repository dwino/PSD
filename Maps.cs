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

    public override void Draw()
    {
        base.Draw();

        if (_stopwatch.ElapsedMilliseconds < Helper.Rnd.Next(500, 2500))
        {
            Program.Ui.SetBackground(AnimationPosition_a.X + XOffset, AnimationPosition_a.Y + YOffset, new SadRogue.Primitives.Color(217, 0, 0));
        }
        else
        {
            Program.Ui.SetBackground(AnimationPosition_a.X + XOffset, AnimationPosition_a.Y + YOffset, new SadRogue.Primitives.Color(25, 25, 25));
            if (_stopwatch.ElapsedMilliseconds > Helper.Rnd.Next(3000, 4500))
            {
                var sfx = AudioManager.GetSFX("malfunction");
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
            Program.Ui.SetForeground(AnimationPosition_c.X + XOffset, AnimationPosition_c.Y + YOffset, new SadRogue.Primitives.Color(_tint, _tint, _tint));
            Program.Ui.SetForeground(AnimationPosition_d.X + XOffset, AnimationPosition_d.Y + YOffset, new SadRogue.Primitives.Color(_tint, _tint, _tint));
            Program.Ui.SetForeground(AnimationPosition_e.X + XOffset, AnimationPosition_e.Y + YOffset, new SadRogue.Primitives.Color(_tint, _tint, _tint));
            Program.Ui.SetForeground(AnimationPosition_f.X + XOffset, AnimationPosition_f.Y + YOffset, new SadRogue.Primitives.Color(_tint, _tint, _tint));
            Program.Ui.SetForeground(AnimationPosition_g.X + XOffset, AnimationPosition_g.Y + YOffset, new SadRogue.Primitives.Color(_tint, _tint, _tint));
            Program.Ui.SetForeground(AnimationPosition_h.X + XOffset, AnimationPosition_h.Y + YOffset, new SadRogue.Primitives.Color(_tint, _tint, _tint));
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

        Program.Ui.SetForeground(AnimationPosition_i.X + XOffset, AnimationPosition_i.Y + YOffset, new SadRogue.Primitives.Color(_tint1, 0, 0));
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
        Interactions.Add(new FullScreenInteraction("EngineRoomComputer", this, (8, 7)));

        var gaurdedAction = new MoveByAction(this, (-2, 0));
        var alternativeAction = new MoveByAction(this, (1, 0));
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

    public EngineRoom() : base("EngineRoom")
    {
        _engineSFX = AudioManager.GetSFX("space_engine");

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

    public override void Draw()
    {
        base.Draw();
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
    private List<ParticleEmmitter> _particleEmitters;
    public Hydroponics() : base("Hydroponics")
    {
        var isAvailable =
        () => { return Program.Engine.Player.Position.Y + YOffset >= this.Height / 2 + YOffset && Program.Engine.Player.Position.X + XOffset < this.Width + XOffset - 4; };
        var interaction = new MapInteraction("TransparantBoxes", this, new Point(10, 8), isAvailable);
        Interactions.Add(interaction);

        var isAvailable1 =
        () => { return Program.Engine.Player.Position.Y + YOffset < this.Height / 2 + YOffset && Program.Engine.Player.Position.X + XOffset < this.Width + XOffset - 4; };
        var interaction1 = new MapInteraction("Soil", this, new Point(10, 8), isAvailable1);
        Interactions.Add(interaction1);

    }

    public override void LoadSpecificInteractionMap(int x, int y)
    {
        if (_particleEmitters == null)
        {
            _particleEmitters = new List<ParticleEmmitter>();

        }

        if (_interactionMap.GetGlyph(x, y) == 'a')
        {
            var position = new Point(x + 1, y);
            var emmitterInterval = (15000, 30000);
            var lifeStepRange = (100, 150);
            var particleAmount = 20;
            var velocityRangeX = (0.0f, 25);
            var velocityRangeY = (0.5f, 15);
            var fgColor = new Color(new Color(0, 0, 220), 0.75f);
            var bgColor = new Color(new Color(0, 0, 110), 0.55f);
            var particleEmmitter = new ParticleEmmitter(position, emmitterInterval, lifeStepRange, particleAmount, velocityRangeX, velocityRangeY, fgColor, bgColor);
            _particleEmitters.Add(particleEmmitter);
        }
        if (_interactionMap.GetGlyph(x, y) == 'b')
        {
            var position = new Point(x, y + 1);
            var emmitterInterval = (15000, 30000);
            var lifeStepRange = (100, 150);
            var particleAmount = 20;
            var velocityRangeX = (0.5f, 10);
            var velocityRangeY = (0.0f, 20);
            var fgColor = new Color(new Color(0, 0, 220), 0.75f);
            var bgColor = new Color(new Color(0, 0, 110), 0.55f);
            var particleEmmitter = new ParticleEmmitter(position, emmitterInterval, lifeStepRange, particleAmount, velocityRangeX, velocityRangeY, fgColor, bgColor);
            _particleEmitters.Add(particleEmmitter);
        }
        if (_interactionMap.GetGlyph(x, y) == 'c')
        {
            var position = new Point(x, y + 1);
            var emmitterInterval = (15000, 30000);
            var lifeStepRange = (100, 150);
            var particleAmount = 20;
            var velocityRangeX = (0.5f, 10);
            var velocityRangeY = (0.0f, 20);
            var fgColor = new Color(new Color(0, 0, 220), 0.75f);
            var bgColor = new Color(new Color(0, 0, 110), 0.55f);
            var particleEmmitter = new ParticleEmmitter(position, emmitterInterval, lifeStepRange, particleAmount, velocityRangeX, velocityRangeY, fgColor, bgColor);
            _particleEmitters.Add(particleEmmitter);
        }
        if (_interactionMap.GetGlyph(x, y) == 'd')
        {

            var position = new Point(x - 1, y);
            var emmitterInterval = (15000, 30000);
            var lifeStepRange = (100, 150);
            var particleAmount = 20;
            var velocityRangeX = (1.0f, 25);
            var velocityRangeY = (0.5f, 15);
            var fgColor = new Color(new Color(0, 0, 220), 0.75f);
            var bgColor = new Color(new Color(0, 0, 110), 0.55f);
            var particleEmmitter = new ParticleEmmitter(position, emmitterInterval, lifeStepRange, particleAmount, velocityRangeX, velocityRangeY, fgColor, bgColor);
            _particleEmitters.Add(particleEmmitter);

            position = new Point(x + 1, y);
            emmitterInterval = (15000, 30000);
            lifeStepRange = (100, 150);
            particleAmount = 20;
            velocityRangeX = (0.0f, 25);
            velocityRangeY = (0.5f, 15);
            fgColor = new Color(new Color(0, 0, 220), 0.75f);
            bgColor = new Color(new Color(0, 0, 110), 0.55f);
            particleEmmitter = new ParticleEmmitter(position, emmitterInterval, lifeStepRange, particleAmount, velocityRangeX, velocityRangeY, fgColor, bgColor);
            _particleEmitters.Add(particleEmmitter);

        }
        if (_interactionMap.GetGlyph(x, y) == 'e')
        {
            var position = new Point(x, y + 1);
            var emmitterInterval = (15000, 30000);
            var lifeStepRange = (100, 150);
            var particleAmount = 20;
            var velocityRangeX = (0.5f, 10);
            var velocityRangeY = (0.0f, 20);
            var fgColor = new Color(new Color(0, 0, 220), 0.75f);
            var bgColor = new Color(new Color(0, 0, 110), 0.55f);
            var particleEmmitter = new ParticleEmmitter(position, emmitterInterval, lifeStepRange, particleAmount, velocityRangeX, velocityRangeY, fgColor, bgColor);
            _particleEmitters.Add(particleEmmitter);
        }
        if (_interactionMap.GetGlyph(x, y) == 'f')
        {
            var position = new Point(x - 1, y);
            var emmitterInterval = (15000, 30000);
            var lifeStepRange = (100, 150);
            var particleAmount = 20;
            var velocityRangeX = (1.0f, 25);
            var velocityRangeY = (0.5f, 15);
            var fgColor = new Color(new Color(0, 0, 220), 0.75f);
            var bgColor = new Color(new Color(0, 0, 110), 0.55f);
            var particleEmmitter = new ParticleEmmitter(position, emmitterInterval, lifeStepRange, particleAmount, velocityRangeX, velocityRangeY, fgColor, bgColor);
            _particleEmitters.Add(particleEmmitter);

            position = new Point(x + 1, y);
            emmitterInterval = (15000, 30000);
            lifeStepRange = (100, 150);
            particleAmount = 20;
            velocityRangeX = (0.0f, 25);
            velocityRangeY = (0.5f, 15);
            fgColor = new Color(new Color(0, 0, 220), 0.75f);
            bgColor = new Color(new Color(0, 0, 110), 0.55f);
            particleEmmitter = new ParticleEmmitter(position, emmitterInterval, lifeStepRange, particleAmount, velocityRangeX, velocityRangeY, fgColor, bgColor);
            _particleEmitters.Add(particleEmmitter);
        }
        if (_interactionMap.GetGlyph(x, y) == 'g')
        {
            var position = new Point(x, y + 1);
            var emmitterInterval = (15000, 30000);
            var lifeStepRange = (100, 150);
            var particleAmount = 20;
            var velocityRangeX = (0.5f, 10);
            var velocityRangeY = (0.0f, 20);
            var fgColor = new Color(new Color(0, 0, 220), 0.75f);
            var bgColor = new Color(new Color(0, 0, 110), 0.55f);
            var particleEmmitter = new ParticleEmmitter(position, emmitterInterval, lifeStepRange, particleAmount, velocityRangeX, velocityRangeY, fgColor, bgColor);
            _particleEmitters.Add(particleEmmitter);
        }
        if (_interactionMap.GetGlyph(x, y) == 'h')
        {
            var position = new Point(x, y + 1);
            var emmitterInterval = (15000, 30000);
            var lifeStepRange = (100, 150);
            var particleAmount = 20;
            var velocityRangeX = (0.5f, 10);
            var velocityRangeY = (0.0f, 20);
            var fgColor = new Color(new Color(0, 0, 220), 0.75f);
            var bgColor = new Color(new Color(0, 0, 110), 0.55f);
            var particleEmmitter = new ParticleEmmitter(position, emmitterInterval, lifeStepRange, particleAmount, velocityRangeX, velocityRangeY, fgColor, bgColor);
            _particleEmitters.Add(particleEmmitter);
        }
    }

    public override void Draw()
    {
        base.Draw();
        Program.Ui.AnimationConsole.Clear();
        foreach (var particleEmmitter in _particleEmitters)
        {
            particleEmmitter.Play();
        }
    }
}
public class MainCorridor : Map
{
    public MainCorridor() : base("MainCorridor")
    {
        ShadowMaps.Add(new ShadowMap { MapString = "Security", XRef = XOffset, YRef = YOffset, XOffset = -9, YOffset = 0 });
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
        var sinkInteraction = new MapInteraction("Sink", this, (1, 1));
        Interactions.Add(sinkInteraction);

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
