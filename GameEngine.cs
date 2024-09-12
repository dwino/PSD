using System;
using Balance.Ui;
using Microsoft.Xna.Framework.Input;

namespace Balance;

public class GameEngine
{
    private GameUi _ui;
    public GameEngine(GameUi ui)
    {
        _ui = ui;
        Player = new Player();
        // CurrentMode = SRPGMode.GenerateRandomInteraction(_ui, this);
        CurrentMode = new IFREMode(_ui, this);

    }

    public Player Player { get; set; }

    public GameMode CurrentMode { get; set; }

    internal void Update()
    {

        if (!CurrentMode.InteractionEnded())
        {
            CurrentMode.Update();

        }
        else
        {
            Player.Life = Player.LifeMax;
            Player.Black = Player.BlackBalance;
            Player.White = Player.WhiteBalance;

            CurrentMode = SRPGMode.GenerateRandomInteraction(_ui, this);

        }
    }
}