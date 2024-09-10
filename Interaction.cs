using System;
using System.Text;
using Balance.Ui;
using Figgle;
using SadConsole.Input;

namespace Balance;

public class Interaction
{
    GameUi _ui;
    GameEngine _game;
    private Player _player;

    public Interaction(Entity other, GameUi ui, GameEngine game)
    {
        _ui = ui;
        _game = game;
        _player = game.Player;
        Other = other;
        Unbalance = 0;
        CurrentAnimation = null;
        MessageLog = null;


    }

    public Animation? CurrentAnimation { get; set; }
    public Animation? MessageLog { get; set; }
    public Entity Other { get; set; }
    public int Unbalance { get; set; }

    public bool InteractionEnded()
    {
        return _player.Life <= 0 || Other.Life <= 0;
    }

    public void SetOthersNextAction()
    {
        Other.NextAction = new InterfereAction(Other, _player, _ui, _game);
    }

    public static Interaction GenerateRandomInteraction(GameUi ui, GameEngine game)
    {
        var life = Helper.Rnd.Next(20, 80);
        var black = Helper.Rnd.Next(0 + life / 10, life / 2 - life / 10);
        var blackBalance = Helper.Rnd.Next(0 + life / 10, life / 2 - life / 10);
        var white = life / 2 - black;
        var whiteBalance = life / 2 - blackBalance;
        var color = new Color(Helper.Rnd.Next(255), Helper.Rnd.Next(255), Helper.Rnd.Next(255));
        char glyph = (char)('a' + Helper.Rnd.Next(26));

        var other = new Entity(life, black, blackBalance, white, whiteBalance, color, glyph);

        return new Interaction(other, ui, game);

    }
    public static Interaction GenerateRandomInteraction(GameUi ui, GameEngine game, char glyph)
    {
        var life = Helper.Rnd.Next(20, 80);
        var black = Helper.Rnd.Next(0 + life / 10, life / 2 - life / 10);
        var blackBalance = Helper.Rnd.Next(0 + life / 10, life / 2 - life / 10);
        var white = life / 2 - black;
        var whiteBalance = life / 2 - blackBalance;
        var color = new Color(Helper.Rnd.Next(255), Helper.Rnd.Next(255), Helper.Rnd.Next(255));
        char _glyph = glyph;

        var other = new Entity(life, black, blackBalance, white, whiteBalance, color, glyph);

        return new Interaction(other, ui, game);

    }

    public void ProcessKeyboard(Keyboard keyboard)
    {
        if ((CurrentAnimation == null || !CurrentAnimation.IsRunning) && (MessageLog == null || !MessageLog.IsRunning))
        {
            if (keyboard.IsKeyPressed(Keys.Escape))
            {
                Game.Instance.MonoGameInstance.Exit();
            }
            else if (keyboard.IsKeyPressed(Keys.Left))
            {
                new ShiftBlackToWhiteAction(_player, _ui, _game).Execute();
            }
            else if (keyboard.IsKeyPressed(Keys.Right))
            {
                new ShiftWhiteToBlackAction(_player, _ui, _game).Execute();
            }
            else if (keyboard.IsKeyPressed(Keys.Down))
            {
                _player.Black = _player.BlackBalance;
                _player.Black = _player.BlackBalance;

            }
            else if (keyboard.IsKeyPressed(Keys.Z))
            {
                var blanaceAction = new BalanceAction(_player, Other, _ui, _game);
                _player.NextAction = blanaceAction;

            }
            else if (keyboard.IsKeyPressed(Keys.A))
            {
                var interfereAction = new InterfereAction(_player, Other, _ui, _game);
                _player.NextAction = interfereAction;

            }
        }
    }

    public void Update()
    {
        if ((CurrentAnimation == null || !CurrentAnimation.IsRunning) && (MessageLog == null || !MessageLog.IsRunning))
        {
            if (_player.NextAction != null)
            {
                if (!InteractionEnded())
                {
                    _player.NextAction.Execute();
                    _player.NextAction = null;
                }

                if (!InteractionEnded())
                {
                    SetOthersNextAction();
                    Other.NextAction!.Execute();
                    Other.NextAction = null;
                }

                if (Unbalance >= Helper.Rnd.Next(101))
                {
                    _player.Life -= Unbalance;
                    Unbalance = 0;
                }
            }


        }
    }

    public void Draw()
    {
        var _console = _ui.Console;
        _console.Clear();

        // DRAW

        ////FIGLET
        int x_other = 0;
        int y_other = 1;
        int x_figlet_other_offset = 5;
        Helper.DrawFiglet(x_other, y_other, _game.CurrentInteraction.Other.Glyph, _game.CurrentInteraction.Other.Color, _console, x_figlet_other_offset);

        x_other += GameSettings.GAME_WIDTH / 2;
        ////GLYPH
        y_other = GameSettings.GAME_HEIGHT / 2 - 1;
        _console.Print(x_other, y_other, _game.CurrentInteraction.Other.Glyph.ToString(), _game.CurrentInteraction.Other.Color);

        ////LIFE
        x_other += 5;
        string lifeInfo = "life: " + _game.CurrentInteraction.Other.Life + "/" + _game.CurrentInteraction.Other.LifeMax;
        _console.Print(x_other, y_other, lifeInfo, Color.Red);

        int mappedRange = Helper.Map(0, _game.CurrentInteraction.Other.LifeMax, 0, lifeInfo.Length, _game.CurrentInteraction.Other.Life);

        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < mappedRange; i++)
        {
            stringBuilder.Append('#');
        }

        string currentLife = stringBuilder.ToString();

        for (int i = currentLife.Length; i < lifeInfo.Length; i++)
        {
            stringBuilder.Append('#');
        }

        string maxlife = stringBuilder.ToString();

        _console.Print(x_other, y_other + 1, maxlife, Color.Red);
        _console.Print(x_other, y_other + 1, currentLife, Color.Green);

        for (int i = 0; i < currentLife.Length; i++)
        {
            _console.SetForeground(x_other + i, y_other, Color.Green);
        }

        //BALANCE

        x_other += maxlife.Length + 5;

        stringBuilder.Clear();

        stringBuilder.Append("white: " + _game.CurrentInteraction.Other.White + "(" + _game.CurrentInteraction.Other.WhiteBalance + ")");
        stringBuilder.Append("          ");
        stringBuilder.Append("black: " + _game.CurrentInteraction.Other.Black + "(" + _game.CurrentInteraction.Other.BlackBalance + ")");

        if (stringBuilder.Length % 2 == 0)
        {
            stringBuilder.Insert(stringBuilder.Length / 2, ' ');
        }

        int midpoint = stringBuilder.Length / 2 + 1;

        stringBuilder.Insert(midpoint, '|');

        string whiteAndBlackAmount = stringBuilder.ToString();

        _console.Print(x_other, y_other, whiteAndBlackAmount);

        int whiteAmount = Helper.Map(0, _game.CurrentInteraction.Other.LifeMax, 0, whiteAndBlackAmount.Length / 2, _game.CurrentInteraction.Other.White);

        for (int x = x_other + midpoint - whiteAmount; x < x_other + midpoint; x++)
        {
            _console.Print(x, y_other + 1, "#", Color.White, Color.Black);

        }

        int blackAmount = Helper.Map(0, _game.CurrentInteraction.Other.LifeMax, 0, whiteAndBlackAmount.Length / 2, _game.CurrentInteraction.Other.Black);

        for (int x = x_other + midpoint; x < x_other + midpoint + blackAmount; x++)
        {
            _console.Print(x, y_other + 1, "#", Color.Black, Color.White);

        }


        if (_game.CurrentInteraction.Other.White < _game.CurrentInteraction.Other.WhiteBalance)
        {
            int idealWhiteAmount = Helper.Map(0, _game.CurrentInteraction.Other.LifeMax, 0, whiteAndBlackAmount.Length / 2, _game.CurrentInteraction.Other.WhiteBalance);

            for (int x = x_other + midpoint - idealWhiteAmount; x < x_other + midpoint - whiteAmount; x++)
            {
                _console.Print(x, y_other + 1, "#", Color.Yellow, Color.Black);

            }

        }
        else
        {
            int idealBlackAmount = Helper.Map(0, _game.CurrentInteraction.Other.LifeMax, 0, whiteAndBlackAmount.Length / 2, _game.CurrentInteraction.Other.BlackBalance);

            for (int x = x_other + midpoint + blackAmount; x < x_other + midpoint + idealBlackAmount; x++)
            {
                _console.Print(x, y_other + 1, "#", Color.Yellow, Color.Black);

            }

        }

        //// Player
        ///


        x_other = 0;
        y_other = GameSettings.GAME_HEIGHT / 2 + 2;

        Helper.DrawFiglet(x_other, y_other, _game.Player.Glyph, _game.Player.Color, _console, x_figlet_other_offset);

        x_other += GameSettings.GAME_WIDTH / 2;
        y_other = GameSettings.GAME_HEIGHT - 2;

        _console.Print(x_other, y_other, _game.Player.Glyph.ToString(), _game.Player.Color);

        ////LIFE
        x_other += 5;
        lifeInfo = "life: " + _game.Player.Life + "/" + _game.Player.LifeMax;
        _console.Print(x_other, y_other, lifeInfo, Color.Red);

        mappedRange = Helper.Map(0, _game.Player.LifeMax, 0, lifeInfo.Length, _game.Player.Life);

        stringBuilder = new StringBuilder();
        for (int i = 0; i < mappedRange; i++)
        {
            stringBuilder.Append('#');
        }

        currentLife = stringBuilder.ToString();

        for (int i = currentLife.Length; i < lifeInfo.Length; i++)
        {
            stringBuilder.Append('#');
        }

        maxlife = stringBuilder.ToString();

        _console.Print(x_other, y_other + 1, maxlife, Color.Red);
        _console.Print(x_other, y_other + 1, currentLife, Color.Green);

        for (int i = 0; i < currentLife.Length; i++)
        {
            _console.SetForeground(x_other + i, y_other, Color.Green);
        }

        x_other += maxlife.Length + 5;

        stringBuilder.Clear();

        stringBuilder.Append("white: " + _game.Player.White + "(" + _game.Player.WhiteBalance + ")");
        stringBuilder.Append("          ");
        stringBuilder.Append("black: " + _game.Player.Black + "(" + _game.Player.BlackBalance + ")");

        if (stringBuilder.Length % 2 == 0)
        {
            stringBuilder.Insert(stringBuilder.Length / 2, ' ');
        }

        midpoint = stringBuilder.Length / 2 + 1;

        stringBuilder.Insert(midpoint, '|');

        whiteAndBlackAmount = stringBuilder.ToString();

        _console.Print(x_other, y_other, whiteAndBlackAmount);

        whiteAmount = Helper.Map(0, _game.Player.LifeMax, 0, whiteAndBlackAmount.Length / 2, _game.Player.White);

        for (int x = x_other + midpoint - whiteAmount; x < x_other + midpoint; x++)
        {
            _console.Print(x, y_other + 1, "#", Color.White, Color.Black);

        }

        blackAmount = Helper.Map(0, _game.Player.LifeMax, 0, whiteAndBlackAmount.Length / 2, _game.Player.Black);

        for (int x = x_other + midpoint; x < x_other + midpoint + blackAmount; x++)
        {
            _console.Print(x, y_other + 1, "#", Color.Black, Color.White);

        }


        if (_game.Player.White < _game.Player.WhiteBalance)
        {
            int idealWhiteAmount = Helper.Map(0, _game.Player.LifeMax, 0, whiteAndBlackAmount.Length / 2, _game.Player.WhiteBalance);

            for (int x = x_other + midpoint - idealWhiteAmount; x < x_other + midpoint - whiteAmount; x++)
            {
                _console.Print(x, y_other + 1, "#", Color.Yellow, Color.Black);

            }

        }
        else
        {
            int idealBlackAmount = Helper.Map(0, _game.Player.LifeMax, 0, whiteAndBlackAmount.Length / 2, _game.Player.BlackBalance);

            for (int x = x_other + midpoint + blackAmount; x < x_other + midpoint + idealBlackAmount; x++)
            {
                _console.Print(x, y_other + 1, "#", Color.Yellow, Color.Black);

            }

        }

        ////UNBALANCE
        x_other = whiteAndBlackAmount.Length + 5;
        var unbalanceInfo = "unbalance: " + Unbalance + "/" + 100;
        _console.Print(x_other, y_other, unbalanceInfo, Color.Blue);

        mappedRange = Helper.Map(0, 100, 0, unbalanceInfo.Length, Unbalance);

        stringBuilder = new StringBuilder();
        for (int i = 0; i < mappedRange; i++)
        {
            stringBuilder.Append('#');
        }

        var currentUnbalance = stringBuilder.ToString();

        for (int i = currentUnbalance.Length; i < unbalanceInfo.Length; i++)
        {
            stringBuilder.Append('#');
        }

        string maxUnbalance = stringBuilder.ToString();

        _console.Print(x_other, y_other + 1, maxUnbalance, Color.White);
        _console.Print(x_other, y_other + 1, currentUnbalance, Color.Blue);

        for (int i = 0; i < currentLife.Length; i++)
        {
            _console.SetForeground(x_other + i, y_other, Color.Blue);
        }

        if (CurrentAnimation != null && CurrentAnimation.IsRunning)
        {
            CurrentAnimation.Play();
        }

        if (MessageLog != null && MessageLog.IsRunning)
        {
            MessageLog.Play();
        }



    }

}
