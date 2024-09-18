using System.Diagnostics;
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

public class MapTransitionAnimation : Animation
{
    private CellSurface _visibleMap;
    private int _xOffset;
    private int _yOffset;
    private Player _player;
    private Point _oldPosition;
    private Point _offset;
    private Color _color;
    public MapTransitionAnimation(CellSurface visibleMap, Player player, Point oldPosition, Point offset, Console console, GameUi ui) : base(console, ui)
    {
        _visibleMap = new CellSurface(visibleMap.Width, visibleMap.Height);
        visibleMap.Copy(_visibleMap);
        _xOffset = (GameSettings.GAME_WIDTH / 2) - (_visibleMap.Width / 2);
        _yOffset = (GameSettings.GAME_HEIGHT / 2) - (_visibleMap.Height / 2);

        _player = player;
        _oldPosition = oldPosition;
        _offset = offset;
        _color = _player.Color;


        var doorOpenSFX = AudioManager.DoorOpenSFX!.CreateInstance();
        doorOpenSFX.Volume = 0.2f;
        doorOpenSFX.Play();
    }

    public override void Play()
    {
        int playerX = _oldPosition.X + (GameSettings.GAME_WIDTH / 2) - (_visibleMap.Width / 2);
        int playerY = _oldPosition.Y + (GameSettings.GAME_HEIGHT / 2) - (_visibleMap.Height / 2);
        if (_stopWatch.ElapsedMilliseconds < 500)
        {
            _visibleMap.Copy(_console.Surface, _xOffset, _yOffset);

            if (_stopWatch.ElapsedMilliseconds < 300)
            {
                _ui.Console.Print(playerX - _offset.X, playerY - _offset.Y, _player.Glyph.ToString(), _color.GetDark());
                _ui.Console.Print(playerX, playerY, " ", Color.White.GetDark());
            }
            else if (_stopWatch.ElapsedMilliseconds >= 300)
            {
                _ui.Console.Print(playerX, playerY, _player.Glyph.ToString(), _color.GetDarker());
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
            _visibleMap.Copy(_console.Surface, _xOffset, _yOffset);

            _color = _color.GetDarker();
            _ui.Console.Print(playerX + _offset.X, playerY + _offset.Y, _player.Glyph.ToString(), _color.GetDarkest());
            _ui.Console.Print(playerX, playerY, " ", Color.White.GetDarkest());
        }
        else if (_stopWatch.ElapsedMilliseconds >= 750)
        {
            var doorCloseSFX = AudioManager.DoorCloseSFX!.CreateInstance();
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
    private IntroTextScreen _diaglogRunner;
    private int _cursorX;
    private int _cursorY;
    private int _linesIndex;
    private string _currentLine;
    private int _currentLineIndex;

    public GameScreenIntroAnimation(Console console, GameUi ui) : base(console, ui)
    {
        _diaglogRunner = new IntroTextScreen("GameScreenIntro");
        _diaglogRunner.IsActive = true;

        while (_diaglogRunner.IsActive)
        {
            _diaglogRunner.ContinueDialog();
        }
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
            _console.Clear();
            foreach (var line in _diaglogRunner.LinesToDraw)
            {
                _console.Print(_cursorX, _cursorY, line);
                _cursorY += 2;
            }
            string continueText = "press A to get up";
            var x = (GameSettings.GAME_WIDTH / 2) - (continueText.Length / 2);
            var y = GameSettings.GAME_HEIGHT / 2 - 1;
            _console.Print(x, y, continueText);
            _stopWatch.Stop();
        }

        else if (keyboard.IsKeyPressed(Keys.A))
        {
            MediaPlayer.Stop();
            MediaPlayer.Play(AudioManager.LeavingHome);
            MediaPlayer.IsRepeating = true;
            IsRunning = false;
            _console.Clear();
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
                    _console.Print(_cursorX, _cursorY, _currentLine[_currentLineIndex].ToString());

                    _cursorX++;
                    _currentLineIndex++;
                    _stopWatch.Restart();
                }
            }
            else
            {
                _cursorX = 5;
                _cursorY += 2;
                _linesIndex++;
                _currentLineIndex = 0;
            }
        }
        else
        {
            string continueText = "press A to get up";
            var x = (GameSettings.GAME_WIDTH / 2) - (continueText.Length / 2);
            var y = GameSettings.GAME_HEIGHT / 2 - 1;
            _console.Print(x, y, continueText);
            _stopWatch.Stop();
        }
    }
}
