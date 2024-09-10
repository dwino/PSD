using System;
using Balance.Ui;

namespace Balance;

public class GameEngine
{
    private GameUi _ui;
    public GameEngine(GameUi ui)
    {
        _ui = ui;
        Player = new Player();
        CurrentInteraction = Interaction.GenerateRandomInteraction(_ui, this);
    }

    public Player Player { get; set; }

    public Interaction CurrentInteraction { get; set; }

    internal void Update()
    {

        if (!CurrentInteraction.InteractionEnded())
        {
            CurrentInteraction.Update();

        }
        else
        {
            Player.Life = Player.LifeMax;
            Player.Black = Player.BlackBalance;
            Player.White = Player.WhiteBalance;

            CurrentInteraction = Interaction.GenerateRandomInteraction(_ui, this);

        }
    }
}