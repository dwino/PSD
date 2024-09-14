using System;
using Balance.Ui;
using Microsoft.Xna.Framework.Media;
using SadConsole.Input;

namespace Balance;

public class GameEngine
{
    private GameUi _ui;
    public GameEngine(GameUi ui)
    {
        _ui = ui;
        Player = new Player();
        Map = new SleepingPodMap();
        Player.Position = Map.StartingPosition;
    }

    public Player Player { get; set; }
    public Map Map { get; set; }

    public void ProcessKeyboard(Keyboard keyboard)
    {
        if (keyboard.IsKeyPressed(Keys.Escape))
        {
            Game.Instance.MonoGameInstance.Exit();
        }
        if (keyboard.IsKeyPressed(Keys.Left))
        {
            var moveByAction = new MoveByAction(_ui, this, Map, (-1, 0));
            Player.NextAction = moveByAction;
        }
        if (keyboard.IsKeyPressed(Keys.Right))
        {
            var moveByAction = new MoveByAction(_ui, this, Map, (1, 0));
            Player.NextAction = moveByAction;
        }
        if (keyboard.IsKeyPressed(Keys.Up))
        {
            var moveByAction = new MoveByAction(_ui, this, Map, (0, -1));
            Player.NextAction = moveByAction;
        }
        if (keyboard.IsKeyPressed(Keys.Down))
        {
            var moveByAction = new MoveByAction(_ui, this, Map, (0, 1));
            Player.NextAction = moveByAction;
        }
        if (keyboard.IsKeyPressed(Keys.Escape))
        {
            Game.Instance.MonoGameInstance.Exit();
        }
        if (keyboard.IsKeyPressed(Keys.J))
        {
            MediaPlayer.Play(AudioManager.SpaceEngine);
        }
        if (keyboard.IsKeyPressed(Keys.K))
        {
            MediaPlayer.Stop();
        }

    }
    public void Update()
    {
        if (Player.NextAction != null)
        {
            Player.NextAction.Execute();
            Player.NextAction = null;
        }
    }
    public void Draw()
    {
        Map.Draw(_ui.Console);

        int x = Player.Position.X + (GameSettings.GAME_WIDTH / 2) - (Map.Width / 2);
        int y = Player.Position.Y + (GameSettings.GAME_HEIGHT / 2) - (Map.Height / 2);

        _ui.Console.Print(x, y, Player.Glyph.ToString(), Color.White);
    }
}