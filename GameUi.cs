using Balance.Screens;
using Microsoft.Xna.Framework.Media;
using SadConsole.Input;

namespace Balance.Ui;

public class GameUi : Console
{
    /// <summary>
    /// 
    /// </summary>
    static GameUi? instance;
    public static GameUi Instance()
    {
        // Uses lazy initialization.
        // Note: this is not thread safe.
        if (instance == null)
        {
            instance = new GameUi();
        }
        return instance;
    }
    /// <summary>
    /// /
    /// </summary>

    private IntroScreen _introScreen;
    private MainMenuScreen _menuScreen;
    public GameScreen GameScreen { get; set; }

    public Console AnimationConsole { get; set; }

    public ScreensEnum ActiveScreen { get; set; }

    private GameUi() : base(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT)
    {
        AnimationConsole = new Console(Width, Height);
        Children.Add(AnimationConsole);

        SpaceShipGenerator.Init(41, 41);
        SpaceShipGenerator.Generate();

        AudioManager.Initialize();

        _introScreen = new IntroScreen();

        _menuScreen = new MainMenuScreen();

        GameScreen = new GameScreen();

        ActiveScreen = ScreensEnum.IntroScreen;
        AudioManager.PlaySong("intro");

        //Game.Instance.ToggleFullScreen();
    }



    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        if (ActiveScreen == ScreensEnum.MapGen)
        {
            if (keyboard.IsKeyPressed(Keys.A))
            {
                SpaceShipGenerator.Generate();
            }
            else if (keyboard.IsKeyPressed(Keys.Escape))
            {
                Game.Instance.MonoGameInstance.Exit();
            }
        }

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
        if (ActiveScreen == ScreensEnum.GameScreen)
        {
            GameScreen.Update();
        }
        Program.Engine.Update();
        base.Update(delta);
    }

    public override void Render(TimeSpan delta)
    {
        if (ActiveScreen == ScreensEnum.MapGen)
        {
            SpaceShipGenerator.DrawFullMap();
        }

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
