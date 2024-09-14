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
public class GameScreenIntroAnimation : Animation
{
    private int _cursorX;
    private int _cursorY;
    private string _introText;
    private int index = 0;


    public GameScreenIntroAnimation(Console console, GameUi ui) : base(console, ui)
    {
        StringBuilder stringBuilder = new StringBuilder();
        using (StreamReader reader = new StreamReader("Content/ScreenText/GameScreen_SSBalance.txt"))
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
            MediaPlayer.Play(AudioManager.LeavingHome);
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
            string continueText = "press A to get up";
            var x = (GameSettings.GAME_WIDTH / 2) - (continueText.Length / 2);
            var y = GameSettings.GAME_HEIGHT / 2 - 1;
            _console.Print(x, y, continueText);
        }
    }
}
