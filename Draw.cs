using System;
using System.Text;

namespace Balance;

public static class Draw
{

    public static void DrawGame(GameEngine _game, Console _console)
    {
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



        //_console.Print(0, GameSettings.GAME_HEIGHT - 2, _game.Player.Glyph + "    life: " + _game.Player.Life + "    white: " + _game.Player.White + "    black: " + _game.Player.Black);


    }

}
