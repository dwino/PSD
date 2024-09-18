using Balance.Ui;
using SadConsole.Input;

namespace Balance.Screens;

public class GameScreen
{
    private GameEngine _game;
    private GameUi _ui;
    public Animation? CurrentAnimation { get; set; }
    public GameScreen(GameEngine game, GameUi ui, Console console)
    {
        _game = game;
        _ui = ui;

        CurrentAnimation = new GameScreenIntroAnimation(console, ui);
    }

    public void ProcessKeyboard(Keyboard keyboard)
    {
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