using System.Diagnostics;
using Balance.Ui;
using Microsoft.Xna.Framework.Media;
using SadConsole.Input;
using SharpDX.D3DCompiler;

namespace Balance;

public enum BackGroundPermisson
{
    None,
    Full,
}

public abstract class Animation
{
    protected Stopwatch _stopWatch;

    protected Animation(BackGroundPermisson backGroundPermisson)
    {
        BackGroundPermisson = backGroundPermisson;
        _stopWatch = new Stopwatch();
        _stopWatch.Start();
        IsRunning = true;
    }
    public BackGroundPermisson BackGroundPermisson;


    public bool IsRunning { get; set; }

    public abstract void ProcessKeyboard(Keyboard keyboard);
    public abstract void Play();
}

public class MapTransitionAnimation : Animation
{
    private CellSurface _visibleMap;
    private List<Entity> _entities;
    private int _xOffset;
    private int _yOffset;
    private Player _player;
    private Point _oldPosition;
    private Point _offset;
    private Color _color;
    public MapTransitionAnimation(Map oldMap, Player player, Point oldPosition, Point offset) : base(BackGroundPermisson.Full)
    {
        _visibleMap = new CellSurface(oldMap.Width, oldMap.Height);
        oldMap.VisibleMap.Copy(_visibleMap);
        _entities = new List<Entity>();
        _entities.AddRange(oldMap.Entities);
        _xOffset = (GameSettings.GAME_WIDTH / 2) - (_visibleMap.Width / 2);
        _yOffset = (GameSettings.GAME_HEIGHT / 2) - (_visibleMap.Height / 2);

        _player = player;
        _oldPosition = oldPosition;
        _offset = offset;
        _color = _player.Color;


        var doorOpenSFX = AudioManager.GetSFX("doorOpen");
        doorOpenSFX.Volume = 0.2f;
        doorOpenSFX.Play();

        Program.Ui.Clear();
    }

    public override void Play()
    {
        int playerX = _oldPosition.X + (GameSettings.GAME_WIDTH / 2) - (_visibleMap.Width / 2);
        int playerY = _oldPosition.Y + (GameSettings.GAME_HEIGHT / 2) - (_visibleMap.Height / 2);
        int entityoffsetX = (GameSettings.GAME_WIDTH / 2) - (_visibleMap.Width / 2);
        int entityoffsetY = (GameSettings.GAME_HEIGHT / 2) - (_visibleMap.Height / 2);
        if (_stopWatch.ElapsedMilliseconds < 500)
        {
            _visibleMap.Copy(Program.Ui.DrawConsole.Surface, _xOffset, _yOffset);

            if (_stopWatch.ElapsedMilliseconds < 300)
            {
                Program.Ui.DrawConsole.Print(playerX - _offset.X, playerY - _offset.Y, _player.Glyph.ToString(), _color.GetDark());
                Program.Ui.DrawConsole.Print(playerX, playerY, " ", Color.White.GetDark());
                foreach (var entity in _entities)
                {
                    Program.Ui.DrawConsole.Print(entity.Position.X + entityoffsetX, entity.Position.Y + entityoffsetY, entity.Glyph.ToString(), entity.Color.GetDark());
                }
            }
            else if (_stopWatch.ElapsedMilliseconds >= 300)
            {
                Program.Ui.DrawConsole.Print(playerX, playerY, _player.Glyph.ToString(), _color.GetDarker());
                foreach (var entity in _entities)
                {
                    Program.Ui.DrawConsole.Print(entity.Position.X + entityoffsetX, entity.Position.Y + entityoffsetY, entity.Glyph.ToString(), entity.Color.GetDarker());
                }
            }
        }
        else if (_stopWatch.ElapsedMilliseconds >= 500 && _stopWatch.ElapsedMilliseconds < 750)
        {
            for (int x = 0; x < _visibleMap.Width; x++)
            {
                for (int y = 0; y < _visibleMap.Height; y++)
                {
                    var fgColor = _visibleMap.GetForeground(x, y);
                    var fgNewColor = fgColor.GetDark();
                    var bgColor = _visibleMap.GetBackground(x, y);
                    var bgNewColor = bgColor.GetDark();
                    _visibleMap.SetForeground(x, y, fgNewColor);
                    _visibleMap.SetBackground(x, y, bgNewColor);
                }
            }
            _visibleMap.Copy(Program.Ui.DrawConsole.Surface, _xOffset, _yOffset);

            _color = _color.GetDarker();
            Program.Ui.DrawConsole.Print(playerX + _offset.X, playerY + _offset.Y, _player.Glyph.ToString(), _color.GetDarkest());
            Program.Ui.DrawConsole.Print(playerX, playerY, " ", Color.White.GetDarkest());
            foreach (var entity in _entities)
            {
                Program.Ui.DrawConsole.Print(entity.Position.X + entityoffsetX, entity.Position.Y + entityoffsetY, entity.Glyph.ToString(), entity.Color.GetDarkest());
            }
        }
        else if (_stopWatch.ElapsedMilliseconds >= 750)
        {
            Program.Ui.DrawConsole.Clear();
            var doorCloseSFX = AudioManager.GetSFX("doorClose");
            doorCloseSFX.Volume = 0.2f;
            doorCloseSFX.Play();
            IsRunning = false;
        }
    }

    public override void ProcessKeyboard(Keyboard keyboard)
    {
    }
}

public class GameScreenIntroAnimation : Animation
{
    private BasicTextScreenDialog _diaglogRunner;
    private int _cursorX;
    private int _cursorY;
    private int _linesIndex;
    private string _currentLine;
    private int _currentLineIndex;

    public GameScreenIntroAnimation() : base(BackGroundPermisson.None)
    {
        _diaglogRunner = new BasicTextScreenDialog("GameScreenIntro");
        _diaglogRunner.IsActive = true;

        _diaglogRunner.ContinueDialog();

        _cursorX = 5;
        _cursorY = 5;

        _linesIndex = 0;
        _currentLine = "";
        _currentLineIndex = 0;
    }

    public override void ProcessKeyboard(Keyboard keyboard)
    {
        if (keyboard.IsKeyPressed(Keys.Z))
        {
            _cursorX = 5;
            _cursorY = 5;
            Program.Ui.DrawConsole.Clear();
            foreach (var line in _diaglogRunner.LinesToDraw)
            {
                Program.Ui.DrawConsole.Print(_cursorX, _cursorY, line);
                _cursorY += 2;
            }
            string continueText = "press A to get up";
            var x = (GameSettings.GAME_WIDTH / 2) - (continueText.Length / 2);
            var y = GameSettings.GAME_HEIGHT / 2 - 1;
            Program.Ui.DrawConsole.Print(x, y, continueText);
            _stopWatch.Stop();
        }

        else if (keyboard.IsKeyPressed(Keys.A))
        {
            AudioManager.PlaySong("leaving_home");
            IsRunning = false;
            Program.Ui.DrawConsole.Clear();
        }
    }


    public override void Play()
    {

        if (_linesIndex < _diaglogRunner.LinesToDraw.Count)
        {
            _currentLine = _diaglogRunner.LinesToDraw[_linesIndex];

            if (_currentLineIndex < _currentLine.Length)
            {
                if (_stopWatch.ElapsedMilliseconds > 55)
                {
                    Program.Ui.DrawConsole.Print(_cursorX, _cursorY, _currentLine.Substring(0, _currentLineIndex), Color.White, Color.Black);

                    //_cursorX++;
                    _currentLineIndex++;
                    _stopWatch.Restart();
                }
            }
            else
            {
                //_cursorX = 5;
                _cursorY += 2;
                _linesIndex++;
                _currentLineIndex = 0;
                _diaglogRunner.ContinueDialog();
            }
        }
        else
        {
            _stopWatch.Stop();
        }
    }
}

public class BasicTextScreenAnimation : Animation
{
    private BasicTextScreenDialog _diaglogRunner;
    private int _cursorX;
    private int _cursorY;
    private int _linesIndex;
    private string _currentLine;
    private int _currentLineIndex;
    private string _continueText;

    public BasicTextScreenAnimation(string dialogString, bool autoRun = false, string continueText = "") : base(BackGroundPermisson.None)
    {
        _diaglogRunner = new BasicTextScreenDialog(dialogString);
        _diaglogRunner.IsActive = true;

        _diaglogRunner.ContinueDialog();

        _cursorX = 5;
        _cursorY = 5;

        _linesIndex = 0;
        _currentLine = "";
        _currentLineIndex = 0;

        IsRunning = autoRun;
        _continueText = continueText;
    }

    public override void ProcessKeyboard(Keyboard keyboard)
    {
        if (keyboard.IsKeyPressed(Keys.Z))
        {
            _cursorX = 5;
            _cursorY = 5;
            Program.Ui.DrawConsole.Clear();
            foreach (var line in _diaglogRunner.LinesToDraw)
            {
                Program.Ui.DrawConsole.Print(_cursorX, _cursorY, line);
                _cursorY += 2;
            }
            var x = (GameSettings.GAME_WIDTH / 2) - (_continueText.Length / 2);
            var y = GameSettings.GAME_HEIGHT / 2 - 1;
            Program.Ui.DrawConsole.Print(x, y, _continueText);
            _stopWatch.Stop();
        }

        else if (keyboard.IsKeyPressed(Keys.A))
        {
            IsRunning = false;
            Program.Ui.DrawConsole.Clear();
        }
    }


    public override void Play()
    {
        if (_linesIndex < _diaglogRunner.LinesToDraw.Count)
        {
            _currentLine = _diaglogRunner.LinesToDraw[_linesIndex];

            if (_currentLineIndex < _currentLine.Length)
            {
                if (_stopWatch.ElapsedMilliseconds > 55)
                {
                    Program.Ui.DrawConsole.Print(_cursorX, _cursorY, _currentLine.Substring(0, _currentLineIndex), Color.White, Color.Black);

                    //_cursorX++;
                    _currentLineIndex++;
                    _stopWatch.Restart();
                }
            }
            else
            {
                //_cursorX = 5;
                _cursorY += 2;
                _linesIndex++;
                _currentLineIndex = 0;
                _diaglogRunner.ContinueDialog();
            }
        }
        else
        {
            _stopWatch.Stop();
        }
    }
}

public class ColoredBackgroundTextScreen : BasicTextScreenAnimation
{
    private float r;
    private float g;
    private float b;
    private float incR;
    private float incG;
    private float incB;

    public ColoredBackgroundTextScreen(string name) : base(name)
    {
        r = Helper.Rnd.NextSingle();
        g = Helper.Rnd.NextSingle();
        b = Helper.Rnd.NextSingle();
        incR = (Helper.Rnd.NextSingle() - 0.5f) / 50;
        incG = (Helper.Rnd.NextSingle() - 0.5f) / 50;
        incB = (Helper.Rnd.NextSingle() - 0.5f) / 50;
    }

    public override void Play()
    {
        base.Play();

        r += incR;
        g += incG;
        b += incB;
        Math.Clamp(r, 0, 1);
        Math.Clamp(g, 0, 1);
        Math.Clamp(b, 0, 1);

        if (r < 0.1 || r > 0.9)
        {
            incR *= -1;
        }
        if (g < 0.1 || g > 0.9)
        {
            incG *= -1;

        }
        if (b < 0.1 || b > 0.9)
        {
            incB *= -1;

        }

        var color = new Color(r, g, b);

        for (int x = 0; x < GameSettings.GAME_WIDTH; x++)
        {
            for (int y = 0; y < GameSettings.GAME_HEIGHT; y++)
            {
                Program.Ui.DrawConsole.SetBackground(x, y, color);
            }

        }
    }


}

public class ParticleEmmitter : Animation
{
    private Point _position;
    private List<Particle> _particles;
    private int _emitterInterval;
    private (int, int) _lifeStepsRange;
    private int _particleAmount;
    private (float, float) _velocityRangeX;
    private (float, float) _velocityRangeY;

    private Color _fgColor;
    private Color _bgColor;

    public ParticleEmmitter(Point position, (int, int) emitterInterval, (int, int) lifeStepRange, int particleAmount, (float, float) velocityRangeX, (float, float) velocityRangeY, Color fgColor, Color bgColor) : base(BackGroundPermisson.Full)
    {
        _position = position;
        _particles = new List<Particle>();
        _emitterInterval = Helper.Rnd.Next(emitterInterval.Item1, emitterInterval.Item2);
        _lifeStepsRange = lifeStepRange;
        _particleAmount = particleAmount;
        _velocityRangeX = velocityRangeX;
        _velocityRangeY = velocityRangeY;
        _fgColor = fgColor;
        _bgColor = bgColor;

        _stopWatch.Start();
    }

    public override void ProcessKeyboard(Keyboard keyboard)
    {
    }

    public override void Play()
    {

        if (_stopWatch.ElapsedMilliseconds > _emitterInterval)
        {
            AudioManager.PlaySFX("spray");

            while (_particles.Count < 15)
            {
                var lifeSteps = Helper.Rnd.Next(_lifeStepsRange.Item1, _lifeStepsRange.Item2);
                var position = new PointFloat((float)_position.X, (float)_position.Y);
                var velocity = new PointFloat((Helper.Rnd.NextSingle() - _velocityRangeX.Item1) / _velocityRangeX.Item2, (Helper.Rnd.NextSingle() - _velocityRangeY.Item1) / _velocityRangeY.Item2);
                var glyph = '.';
                var fgColor = _fgColor;
                var bgColor = _bgColor;

                _particles.Add(new Particle(lifeSteps, position, velocity, glyph, fgColor, bgColor));
            }
            _stopWatch.Restart();
        }

        if (_particles.Count > 0)
        {
            var removeList = new List<Particle>();

            foreach (var particle in _particles)
            {
                if (particle.Died)
                {
                    removeList.Add(particle);
                }
                else
                {
                    var point = particle.Position.GetPoint();
                    Program.Ui.FGAnimationConsole.Print(point.X + Program.Engine.Map.XOffset, point.Y + Program.Engine.Map.YOffset, particle.Glyph.ToString(), particle.FGColor, particle.BGColor);
                    particle.Update();
                }
            }

            foreach (var particle in removeList)
            {
                _particles.Remove(particle);
            }
        }
    }
}
