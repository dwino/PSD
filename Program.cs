using Balance;
using Balance.Ui;
using SadConsole.Configuration;

Settings.WindowTitle = "Balance";

Builder gameStartup = new Builder()
    .SetScreenSize(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT)
    .SetStartingScreen<GameUi>()
    .IsStartingScreenFocused(true)
    .ConfigureFonts("fonts/Nagidal24.font")
    ;

Game.Create(gameStartup);
Game.Instance.Run();
Game.Instance.Dispose();
