using Balance.Ui;
using Microsoft.Xna.Framework.Media;
using SadConsole.Input;

namespace Balance.Screens;

internal class GameScreen
{
    private Console _console;
    private GameEngine _game;
    private GameUi _ui;
    private Animation _introAnimation;
    private Animation? CurrentAnimation;
    public GameScreen(GameEngine game, GameUi ui, Console console)
    {
        _game = game;
        _ui = ui;
        _console = console;

        _introAnimation = new GameScreenIntroAnimation(console, ui);
        //CurrentAnimation = new FirstEncounterAnimation(console, ui, _game);
        CurrentAnimation = null;
    }

    public void ProcessKeyboard(Keyboard keyboard)
    {
        if (_introAnimation.IsRunning)
        {
            _introAnimation.ProcessKeyboard(keyboard);
        }
        else if (CurrentAnimation != null && CurrentAnimation.IsRunning)
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
        if (_introAnimation.IsRunning)
        {
            _introAnimation.Play();

        }
        else if (CurrentAnimation != null && CurrentAnimation.IsRunning)
        {
            CurrentAnimation.Play();
        }
        else
        {
            _game.Draw();
        }

    }
}