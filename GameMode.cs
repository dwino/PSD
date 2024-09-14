using System;
using System.Text;
using Balance.Ui;
using Figgle;
using Microsoft.Xna.Framework.Media;
using SadConsole.Input;
using SadConsole.Readers;

namespace Balance;

public abstract class GameMode
{
    protected GameUi _ui;
    protected GameEngine _game;
    protected Player _player;

    protected GameMode(GameUi ui, GameEngine game)
    {
        _ui = ui;
        _game = game;
        _player = game.Player;
    }
    public abstract void ProcessKeyboard(Keyboard keyboard);
    public abstract void Update();
    public abstract void Draw();


}

public class IFREMode : GameMode
{
    public IFREMode(GameUi ui, GameEngine game) : base(ui, game)
    {
        Map = new SleepingPodMap();
        game.Player.Position = Map.StartingPosition;

    }
    public Map Map { get; set; }

    public override void ProcessKeyboard(Keyboard keyboard)
    {
        if (keyboard.IsKeyPressed(Keys.Escape))
        {
            Game.Instance.MonoGameInstance.Exit();
        }
        if (keyboard.IsKeyPressed(Keys.Left))
        {
            var moveByAction = new MoveByAction(_ui, _game, Map, (-1, 0));
            _player.NextAction = moveByAction;
        }
        if (keyboard.IsKeyPressed(Keys.Right))
        {
            var moveByAction = new MoveByAction(_ui, _game, Map, (1, 0));
            _player.NextAction = moveByAction;
        }
        if (keyboard.IsKeyPressed(Keys.Up))
        {
            var moveByAction = new MoveByAction(_ui, _game, Map, (0, -1));
            _player.NextAction = moveByAction;
        }
        if (keyboard.IsKeyPressed(Keys.Down))
        {
            var moveByAction = new MoveByAction(_ui, _game, Map, (0, 1));
            _player.NextAction = moveByAction;
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
    public override void Update()
    {
        if (_player.NextAction != null)
        {
            _player.NextAction.Execute();
            _player.NextAction = null;
        }
    }
    public override void Draw()
    {
        Map.Draw(_ui.Console);

        int x = _player.Position.X + (GameSettings.GAME_WIDTH / 2) - (Map.Width / 2);
        int y = _player.Position.Y + (GameSettings.GAME_HEIGHT / 2) - (Map.Height / 2);

        _ui.Console.Print(x, y, _player.Glyph.ToString(), Color.White);

    }
}


