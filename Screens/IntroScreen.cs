using System.Diagnostics;
using System.Text;
using Balance.Ui;
using Microsoft.Xna.Framework.Media;
using SadConsole.Input;

namespace Balance.Screens;

public class IntroScreen
{
    public IntroScreen()
    {
        string sourceFile = "Content/ScreenText/IntroScreen" + "_SSBalance" + ".txt";
        StringBuilder stringBuilder = new StringBuilder();
        using (StreamReader reader = new StreamReader(sourceFile))
        {
            string line;
            while ((line = reader.ReadLine()!) != null)
            {
                stringBuilder.AppendLine(line);
            }
        }
        _introText = stringBuilder.ToString(); ;
        _cursorX = 5;
        _cursorY = 5;
        _stopWatch = new Stopwatch();
        _stopWatch.Start();

    }

    private int _cursorX;
    private int _cursorY;

    private string _introText;
    private int index = 0;

    private Stopwatch _stopWatch;

    public void ProcessKeyboard(Keyboard keyboard)
    {
        if (keyboard.IsKeyPressed(Keys.A))
        {
            AudioManager.PlaySong("menu");
            Program.Ui.Clear();
            Program.Ui.ActiveScreen = ScreensEnum.GameMenuScreen;
        }
    }

    public void Play()
    {
        if (index < _introText.Length)
        {
            if (_stopWatch.ElapsedMilliseconds > 55)
            {
                if (Program.Ui.Cursor.Position.X > GameSettings.GAME_WIDTH - 10 || _introText[index] == '\n')
                {
                    _cursorX = 5;
                    _cursorY += 2;
                    if (_introText[index] == '\n' && index < _introText.Length - 1)
                    {
                        index++;
                    }
                }
                Program.Ui.Print(_cursorX, _cursorY, _introText[index].ToString());

                _cursorX++;
                index++;
                _stopWatch.Restart();

                if (index == _introText.Length)
                {
                    _stopWatch.Stop(); ;
                    AudioManager.PlaySong("menu");
                    Program.Ui.Clear();

                    Program.Ui.ActiveScreen = ScreensEnum.GameMenuScreen;

                }
            }
        }
    }
}
