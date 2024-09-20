using Balance.Ui;
using SadConsole.Input;

namespace Balance.Screens;

public class GameScreen
{

    public Animation? CurrentAnimation { get; set; }
    public GameScreen()
    {
        CurrentAnimation = new GameScreenIntroAnimation();
    }

    public void ProcessKeyboard(Keyboard keyboard)
    {
        if (CurrentAnimation != null && CurrentAnimation.IsRunning)
        {
            CurrentAnimation.ProcessKeyboard(keyboard);
        }
        else
        {
            Program.Engine.ProcessKeyboard(keyboard);
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
            Program.Engine.Draw();
        }

    }
}