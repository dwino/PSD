using System.Diagnostics;
using Balance.Ui;
using SadConsole.Input;

namespace Balance.Screens;

public class MainMenuScreen
{
    private int _activeSelection;
    public MainMenuScreen()
    {
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
                Program.Ui.Clear();
                Program.Ui.ActiveScreen = ScreensEnum.GameScreen;
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
        Program.Ui.Clear();
        string menuScreenText = "Perfect Days in Space";
        var x = (GameSettings.GAME_WIDTH / 2) - (menuScreenText.Length / 2);
        var y = GameSettings.GAME_HEIGHT / 2 - 1;
        Program.Ui.Print(x, y, menuScreenText);

        if (_activeSelection == 0)
        {
            x = (GameSettings.GAME_WIDTH / 2) - (">Play<".Length / 2);
            y++;
            Program.Ui.Print(x, y, ">Play<");
            x = (GameSettings.GAME_WIDTH / 2) - ("Exit".Length / 2);
            y++;
            Program.Ui.Print(x, y, "Exit");
        }
        else if (_activeSelection == 1)
        {
            x = (GameSettings.GAME_WIDTH / 2) - ("Play".Length / 2);
            y++;
            Program.Ui.Print(x, y, "Play");
            x = (GameSettings.GAME_WIDTH / 2) - (">Exit<".Length / 2);
            y++;
            Program.Ui.Print(x, y, ">Exit<");
        }


    }

}