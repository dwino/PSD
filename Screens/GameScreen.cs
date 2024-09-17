using Balance.Ui;
using Microsoft.Xna.Framework.Media;
using SadConsole.Input;

namespace Balance.Screens;

public class GameScreen
{
    private Console _console;
    private GameEngine _game;
    private GameUi _ui;
    //private Animation _introAnimation;
    public Animation? CurrentAnimation { get; set; }
    public GameScreen(GameEngine game, GameUi ui, Console console)
    {
        _game = game;
        _ui = ui;
        _console = console;

        CurrentAnimation = new GameScreenIntroAnimation(console, ui);
    }

    public void ProcessKeyboard(Keyboard keyboard)
    {
        // if (_introAnimation.IsRunning)
        // {
        //     _introAnimation.ProcessKeyboard(keyboard);
        // }
        // else 
        if (CurrentAnimation != null && CurrentAnimation.IsRunning)
        {
            CurrentAnimation.ProcessKeyboard(keyboard);
        }
        else
        {
            _game.ProcessKeyboard(keyboard);
        }
    }
    public void DrawGameScreen()
    {
        // if (_introAnimation.IsRunning)
        // {
        //     _introAnimation.Play();

        // }
        // else 
        if (CurrentAnimation != null && CurrentAnimation.IsRunning)
        {
            CurrentAnimation.Play();
        }
        else
        {
            _game.Draw();
        }

    }
}