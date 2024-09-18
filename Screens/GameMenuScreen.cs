using System.Diagnostics;
using Balance.Ui;
using SadConsole.Input;

namespace Balance.Screens;

public class MainMenuScreen
{
    private Console _console;
    private GameUi _gameUi;
    private Stopwatch _stopwatch;
    private int _activeSelection;
    public MainMenuScreen(Console console, GameUi ui)
    {
        _console = console;
        console.Clear();
        _gameUi = ui;
        _stopwatch = Stopwatch.StartNew();
        _activeSelection = 0;
    }

    public void ProcessKeyboard(Keyboard keyboard)
    {
        if (keyboard.IsKeyPressed(Keys.Z))
        {
            Game.Instance.MonoGameInstance.Exit();
        }
        else if (keyboard.IsKeyPressed(Keys.A))
        {
            if (_activeSelection == 0)
            {
                _console.Clear();
                _gameUi.ActiveScreen = ScreensEnum.GameScreen;
            }
            else if (_activeSelection == 1)
            {
                Game.Instance.MonoGameInstance.Exit();
            }
        }
        else if (keyboard.IsKeyPressed(Keys.Down))
        {
            if (_activeSelection == 0)
            {
                _activeSelection = 1;
            }
        }
        else if (keyboard.IsKeyPressed(Keys.Up))
        {
            if (_activeSelection == 1)
            {
                _activeSelection = 0;
            }
        }
    }


    public void Draw()
    {
        _console.Clear();
        string menuScreenText = "Perfect Days in Space";
        var x = (GameSettings.GAME_WIDTH / 2) - (menuScreenText.Length / 2);
        var y = GameSettings.GAME_HEIGHT / 2 - 1;
        _console.Print(x, y, menuScreenText);

        if (_activeSelection == 0)
        {
            x = (GameSettings.GAME_WIDTH / 2) - (">Play<".Length / 2);
            y++;
            _console.Print(x, y, ">Play<");
            x = (GameSettings.GAME_WIDTH / 2) - ("Exit".Length / 2);
            y++;
            _console.Print(x, y, "Exit");
        }
        else if (_activeSelection == 1)
        {
            x = (GameSettings.GAME_WIDTH / 2) - ("Play".Length / 2);
            y++;
            _console.Print(x, y, "Play");
            x = (GameSettings.GAME_WIDTH / 2) - (">Exit<".Length / 2);
            y++;
            _console.Print(x, y, ">Exit<");
        }


    }

}