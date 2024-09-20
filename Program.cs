using Balance.Ui;

namespace Balance;

public class Program
{
    public static GameUi Ui { get; set; }
    public static GameEngine Engine { get; set; }
    private static void Main(string[] args)
    {
        Settings.WindowTitle = "Routine";

        Game.Create(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT, "fonts/cp437_20.font", gameStartup);
        Game.Instance.Run();
        Game.Instance.Dispose();
    }

    static void gameStartup(object? sender, GameHost host)
    {
        Ui = GameUi.Instance();
        Engine = GameEngine.Instance();
        Game.Instance.Screen = Ui;
        Game.Instance.Screen.IsFocused = true;
    }
}