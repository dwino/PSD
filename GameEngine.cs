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
        if (Map.CurrentInteraction != null && Map.CurrentInteractionIndex != -1 && Map.CurrentInteraction.IsActive)
        {
            if (keyboard.IsKeyPressed(Keys.Tab) || keyboard.IsKeyPressed(Keys.Down))  //L1 / UP
            {
                if (Map.CurrentInteraction.CurrentNode.InteractionOptions.Count > 0)
                {
                    Map.CurrentInteraction.CurrentNode.CurrentOptionIndex++;
                    if (Map.CurrentInteraction.CurrentNode.CurrentOptionIndex
                        >= Map.CurrentInteraction.CurrentNode.InteractionOptions.Count)
                    {
                        Map.CurrentInteraction.CurrentNode.CurrentOptionIndex = 0;
                    }
                }
            }
            if (keyboard.IsKeyPressed(Keys.Tab) || keyboard.IsKeyPressed(Keys.Up))  //L1 / DOWN
            {
                if (Map.CurrentInteraction.CurrentNode.InteractionOptions.Count > 0)
                {
                    Map.CurrentInteraction.CurrentNode.CurrentOptionIndex--;
                    if (Map.CurrentInteraction.CurrentNode.CurrentOptionIndex < 0)
                    {
                        Map.CurrentInteraction.CurrentNode.CurrentOptionIndex = Map.CurrentInteraction.CurrentNode.InteractionOptions.Count - 1;
                    }
                }
            }
            if (keyboard.IsKeyPressed(Keys.A))
            {
                var currentInteractionNodeOption = Map.CurrentInteraction.CurrentNode.CurrentOption;
                if (currentInteractionNodeOption != null)
                {
                    Map.CurrentInteraction.CurrentNodeIndex = currentInteractionNodeOption.DestinationNodeID;
                    if (Map.CurrentInteraction.CurrentNodeIndex == -1)
                    {
                        Map.CurrentInteraction.IsActive = false;
                    }
                    else
                    {
                        Map.CurrentInteraction.CurrentNode.CurrentOptionIndex = 0;
                    }
                }
            }

        }
        else
        {
            if (keyboard.IsKeyPressed(Keys.Tab)) // L1
            {
                if (Map.AvailableInteractions.Count > 0)
                {
                    Map.CurrentInteractionIndex++;
                    if (Map.CurrentInteractionIndex >= Map.AvailableInteractions.Count)
                    {
                        Map.CurrentInteractionIndex = 0;
                    }
                }
            }
            if (keyboard.IsKeyPressed(Keys.A))
            {
                var currentInteraction = Map.CurrentInteraction;
                if (currentInteraction != null)
                {
                    currentInteraction.IsActive = true;
                }
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


            if (keyboard.IsKeyPressed(Keys.J))
            {
                MediaPlayer.Play(AudioManager.SpaceEngine);
            }
            if (keyboard.IsKeyPressed(Keys.K))
            {
                MediaPlayer.Stop();
            }
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
        _ui.Console.Clear();

        Map.Draw(_ui.Console);

        int x = Player.Position.X + (GameSettings.GAME_WIDTH / 2) - (Map.Width / 2);
        int y = Player.Position.Y + (GameSettings.GAME_HEIGHT / 2) - (Map.Height / 2);

        _ui.Console.Print(x, y, Player.Glyph.ToString(), Color.White);

        if (Map.CurrentInteraction != null && Map.CurrentInteractionIndex != -1 && Map.CurrentInteraction.IsActive)
        {
            Map.CurrentInteraction.Draw(_ui.Console);
        }
    }
}