using System;
using System.Diagnostics;
using System.Text;
using Balance.Screens;
using Balance.Ui;
using Microsoft.Xna.Framework.Media;
using SadConsole.Input;

namespace Balance;

public abstract class Animation
{
    protected Console _console;
    protected GameUi _ui;
    protected Stopwatch _stopWatch;

    protected Animation(Console console, GameUi ui)
    {
        _console = console;
        _ui = ui;
        _stopWatch = new Stopwatch();
        _stopWatch.Start();
        IsRunning = true;
    }

    public bool IsRunning { get; set; }

    public abstract void ProcessKeyboard(Keyboard keyboard);
    public abstract void Play();
}

public class FirstEncounterAnimation : Animation
{
    private GameEngine _game;
    private Point _other;
    private List<Point> _previousPoints;
    private Point _player;
    private char _otherGlyph;
    private Color _otherColor;
    public FirstEncounterAnimation(Console console, GameUi ui, GameEngine game) : base(console, ui)
    {
        _game = game;
        _other = new Point(Helper.Rnd.Next(5, GameSettings.GAME_WIDTH - 5), Helper.Rnd.Next(5, GameSettings.GAME_HEIGHT - 5));
        _otherGlyph = 'r';//_game.CurrentMode.Other.Glyph;
        _otherColor = Color.Red;//_game.CurrentMode.Other.Color;
        _player = new Point(Helper.Rnd.Next(5, GameSettings.GAME_WIDTH - 5), Helper.Rnd.Next(5, GameSettings.GAME_HEIGHT - 5));
        _previousPoints = new List<Point>();
    }

    public override void ProcessKeyboard(Keyboard keyboard)
    {
        if (keyboard.IsKeyPressed(Keys.Z))
        {
            _other = new Point(Helper.Rnd.Next(5, GameSettings.GAME_WIDTH - 5), Helper.Rnd.Next(5, GameSettings.GAME_HEIGHT - 5));
            _otherGlyph = 'r';
            _player = new Point(Helper.Rnd.Next(5, GameSettings.GAME_WIDTH - 5), Helper.Rnd.Next(5, GameSettings.GAME_HEIGHT - 5));
            _previousPoints = new List<Point>();

        }
        else if (keyboard.IsKeyPressed(Keys.A))
        {

            _console.Clear();
            IsRunning = false;
        }
    }

    public override void Play()
    {
        if (_stopWatch.ElapsedMilliseconds > 55)
        {
            _stopWatch.Restart();

            foreach (Point point in _previousPoints)
            {
                _console.Print(point.X, point.Y, '.'.ToString());

            }
            _console.Print(_other.X, _other.Y, _otherGlyph.ToString(), _otherColor);
            _console.Print(_player.X, _player.Y, '@'.ToString());

            if (_other == _player)
            {
                _console.Clear();
                IsRunning = false;
            }
            else
            {

                int x = 0;
                int y = 0;

                if (_player.X > _other.X)
                {
                    x = 1;
                }
                else if (_player.X == _other.X)
                {
                    x = 0;
                }
                else
                {
                    x = -1;
                }

                if (_player.Y > _other.Y)
                {
                    y = 1;
                }
                else if (_player.Y == _other.Y)
                {
                    y = 0;
                }
                else
                {
                    y = -1;
                }

                _previousPoints.Add(_other);

                _other = (_other.X + x, _other.Y + y);
            }
        }
    }
}

public class AttackAnimation : Animation
{
    private SRPGMode _currentInteraction;
    public AttackAnimation(Console console, GameUi ui, SRPGMode currentInteraction) : base(console, ui)
    {
        _currentInteraction = currentInteraction;
        AudioManager.HitSFX.CreateInstance().Play();

    }

    public override void Play()
    {
        if (_stopWatch.ElapsedMilliseconds < 200)
        {
            ////FIGLET
            int x_other = 0 + Helper.Rnd.Next(-5, 5);
            int y_other = 1 + Helper.Rnd.Next(-5, 5);
            int x_figlet_other_offset = 5;
            Helper.DrawFiglet(x_other, y_other, _currentInteraction.Other.Glyph, _currentInteraction.Other.Color, _console, x_figlet_other_offset);
        }
        else
        {
            _stopWatch.Stop();
            IsRunning = false;
        }
    }

    public override void ProcessKeyboard(Keyboard keyboard)
    {
        throw new NotImplementedException();
    }
}

public class HealAnimation : Animation
{
    private Player _player;
    private SRPGMode _currentInteraction;
    private Stopwatch _stepStopWatch;

    private int x_start;
    private int x_end;
    private int y_start;
    private int y_end;
    private int y;
    public HealAnimation(Console console, GameUi ui, Player player, SRPGMode currentInteraction) : base(console, ui)
    {
        _player = player;
        _currentInteraction = currentInteraction;
        _stepStopWatch = new Stopwatch();
        _stepStopWatch.Start();

        x_start = 0;
        x_end = GameSettings.GAME_WIDTH / 2;
        y_start = GameSettings.GAME_HEIGHT / 2;
        y_end = GameSettings.GAME_HEIGHT - 2;
        y = y_start;
        AudioManager.HealSFX.CreateInstance().Play();

    }

    public override void Play()
    {
        if (_stopWatch.ElapsedMilliseconds < 500)
        {
            if (_stepStopWatch.ElapsedMilliseconds > 20)
            {
                y++;
                if (y >= y_end)
                {
                    y = y_start;
                }
                _stepStopWatch.Restart();
            }


            for (int x = x_start; x < x_end; x++)
            {
                _console.SetForeground(x, y - 1, new Color(0, 255, 0));
                _console.SetForeground(x, y, new Color(0, 100, 0));
                _console.SetForeground(x, y + 1, new Color(0, 255, 0));

            }
        }
        else
        {
            _stopWatch.Stop();
            IsRunning = false;
        }
    }
    public override void ProcessKeyboard(Keyboard keyboard)
    {
        throw new NotImplementedException();
    }
}

public class MessageLogAnimation : Animation
{
    private string _text;
    private int _delay;
    private int _cursorX;
    private int _cursorY;
    private int _index;
    public MessageLogAnimation(Console console, GameUi ui, string text) : base(console, ui)
    {
        _text = text;
        _delay = 15;
        _cursorX = GameSettings.GAME_WIDTH / 2;
        _cursorY = GameSettings.GAME_HEIGHT / 2 + 5;
        _index = 0;
    }

    public override void Play()
    {

        if (_stopWatch.ElapsedMilliseconds > _delay)
        {

            _console.Print(_cursorX, _cursorY, _text.Substring(0, _index));

            if (_index < _text.Length)
            {
                _index++;
                _stopWatch.Restart();
            }

            if (_stopWatch.ElapsedMilliseconds > 250)
            {
                _stopWatch.Stop();
                IsRunning = false;
                _console.Clear();
            }
        }

    }

    public override void ProcessKeyboard(Keyboard keyboard)
    {
        throw new NotImplementedException();
    }
}

public class GameScreenIntroAnimation : Animation
{
    private int _cursorX;
    private int _cursorY;
    private string _introText;
    private int index = 0;


    public GameScreenIntroAnimation(Console console, GameUi ui) : base(console, ui)
    {
        StringBuilder stringBuilder = new StringBuilder();
        using (StreamReader reader = new StreamReader("Screens/GameScreen.txt"))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                stringBuilder.AppendLine(line);
            }
        }
        _introText = stringBuilder.ToString(); ;
        _cursorX = 5;
        _cursorY = 5;

        _console.Cursor.Position = new Point(5, 5);
    }


    public override void ProcessKeyboard(Keyboard keyboard)
    {
        if (keyboard.IsKeyPressed(Keys.Z))
        {
            _console.Cursor.Print(_introText.Substring(index));
            index = _introText.Length;
            _stopWatch.Stop();
        }

        else if (keyboard.IsKeyPressed(Keys.A))
        {
            MediaPlayer.Stop();
            MediaPlayer.Play(AudioManager.CombatInteractionMusic);
            MediaPlayer.IsRepeating = true;
            IsRunning = false;
            _console.Clear();

        }
    }


    public override void Play()
    {
        if (index < _introText.Length)
        {
            if (_stopWatch.ElapsedMilliseconds > 55)
            {
                if (_cursorX > GameSettings.GAME_WIDTH - 10 || _introText[index] == '\n')
                {
                    _cursorX = 5;
                    _cursorY += 2;
                    if (_introText[index] == '\n' && index < _introText.Length)
                    {
                        index++;
                    }
                }
                if (index < _introText.Length)
                {
                    _console.Print(_cursorX, _cursorY, _introText[index].ToString());
                }

                _cursorX++;
                index++;
                _stopWatch.Restart();
                if (index >= _introText.Length)
                {
                    _stopWatch.Stop();
                }
            }

        }
        else if (!_stopWatch.IsRunning)
        {
            string continueText = "press A to start slaying monsters";
            var x = (GameSettings.GAME_WIDTH / 2) - (continueText.Length / 2);
            var y = GameSettings.GAME_HEIGHT / 2 - 1;
            _console.Print(x, y, continueText);
        }

    }
}
