using Balance.Screens;
using Microsoft.Xna.Framework.Media;
using SadConsole.Input;

namespace Balance.Ui;

public class GameUi : ScreenObject
{
    private IntroScreen _introScreen;
    private MainMenuScreen _menuScreen;
    public GameScreen GameScreen { get; set; }

    private GameEngine _game;
    public ScreensEnum ActiveScreen { get; set; }

    public GameUi()
    {
        AudioManager.Initialize();

        Console = new Console(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT, GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT);

        Children.Add(Console);

        _introScreen = new IntroScreen(Console, this);

        _menuScreen = new MainMenuScreen(Console, this);

        _game = new GameEngine(this);

        GameScreen = new GameScreen(_game, this, Console);

        ActiveScreen = ScreensEnum.IntroScreen;
        MediaPlayer.Play(AudioManager.IntroMusic);
        MediaPlayer.IsRepeating = true;

        //Game.Instance.ToggleFullScreen();
    }

    public Console Console { get; set; }


    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        if (ActiveScreen == ScreensEnum.GameScreen)
        {
            GameScreen.ProcessKeyboard(keyboard);
        }
        else if (ActiveScreen == ScreensEnum.GameMenuScreen)
        {
            _menuScreen.ProcessKeyboard(keyboard);
        }
        else if (ActiveScreen == ScreensEnum.IntroScreen)
        {
            _introScreen.ProcessKeyboard(keyboard);
        }


        return base.ProcessKeyboard(keyboard);
    }

    public override void Update(TimeSpan delta)
    {
        _game.Update();
        base.Update(delta);
    }

    public override void Render(TimeSpan delta)
    {
        if (ActiveScreen == ScreensEnum.GameScreen)
        {
            GameScreen.DrawGameScreen();
        }
        else if (ActiveScreen == ScreensEnum.GameMenuScreen)
        {
            _menuScreen.Draw();
        }
        else if (ActiveScreen == ScreensEnum.IntroScreen)
        {
            _introScreen.Play();
        }

        base.Render(delta);
    }



}
