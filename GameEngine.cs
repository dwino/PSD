using Assimp.Unmanaged;
using Balance.Ui;
using SadConsole.Input;

namespace Balance;

public class GameEngine
{
    /// <summary>
    /// 
    /// </summary>
    static GameEngine? instance;
    public static GameEngine Instance()
    {
        // Uses lazy initialization.
        // Note: this is not thread safe.
        if (instance == null)
        {
            instance = new GameEngine();
        }
        return instance;
    }
    /// <summary>
    /// /
    /// </summary>


    private GameEngine()
    {
        Player = new Player();

        var condition = () => { return Map!.XpMapString == "EngineRoom"; };
        var consequence = () => { Program.Ui.GameScreen.AddAnimation(new BasicTextScreenAnimation("PeaceAtLast")); };
        var trigger = new Trigger(condition, consequence, true);

        Player.Triggers.Add(trigger);

        Map = new YourRoom();
        Player.Position = (1, 2);
    }

    public Player Player { get; set; }
    public Map Map { get; set; }

    public void ChangeMap(Map newMap)
    {
        var oldMap = Map;
        if (oldMap != null)
        {
            oldMap.OnExit();
        }
        Map = newMap;
        if (newMap != null)
        {
            newMap.OnEnter();
        }
    }

    public void ProcessKeyboard(Keyboard keyboard)
    {
        if (keyboard.IsKeyPressed(Keys.Escape))
        {
            Game.Instance.MonoGameInstance.Exit();
        }
        if ((Map.CurrentInteraction != null || Map.CurrentInteractionIndex != -1) && Map.CurrentInteraction.IsActive)
        {
            if (Map.CurrentInteraction.OptionRequired)
            {
                if (keyboard.IsKeyPressed(Keys.Tab) || keyboard.IsKeyPressed(Keys.Down))  //L1 / UP
                {
                    if (Map.CurrentInteraction.IsActive)
                    {
                        Map.CurrentInteraction.SelectedOptionIndex++;
                        if (Map.CurrentInteraction.SelectedOptionIndex
                            >= Map.CurrentInteraction.OptionsToDraw.Count)
                        {
                            Map.CurrentInteraction.SelectedOptionIndex = 0;
                        }
                    }
                }
                if (keyboard.IsKeyPressed(Keys.Up))  //L1 / DOWN
                {
                    if (Map.CurrentInteraction.OptionsToDraw.Count > 0)
                    {
                        Map.CurrentInteraction.SelectedOptionIndex--;
                        if (Map.CurrentInteraction.SelectedOptionIndex < 0)
                        {
                            Map.CurrentInteraction.SelectedOptionIndex = Map.CurrentInteraction.OptionsToDraw.Count - 1;
                        }
                    }
                }
                if (keyboard.IsKeyPressed(Keys.A))
                {
                    if (Map.CurrentInteraction.SelectedOptionIndex >= 0
                    && Map.CurrentInteraction.SelectedOptionIndex < Map.CurrentInteraction.OptionsToDraw.Count)
                    {
                        Map.CurrentInteraction.SetSelectedOption();
                    }
                }
            }
            else
            {
                if (keyboard.IsKeyPressed(Keys.A))
                {
                    Map.CurrentInteraction.ContinueDialog();
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
                    Map.CurrentInteraction!.ContinueDialog();
                }
            }


            if (keyboard.IsKeyPressed(Keys.Left))
            {
                var moveByAction = new MoveByAction(Map, (-1, 0));
                Player.NextAction = moveByAction;
            }
            if (keyboard.IsKeyPressed(Keys.Right))
            {
                var moveByAction = new MoveByAction(Map, (1, 0));
                Player.NextAction = moveByAction;
            }
            if (keyboard.IsKeyPressed(Keys.Up))
            {
                var moveByAction = new MoveByAction(Map, (0, -1));
                Player.NextAction = moveByAction;
            }
            if (keyboard.IsKeyPressed(Keys.Down))
            {
                var moveByAction = new MoveByAction(Map, (0, 1));
                Player.NextAction = moveByAction;
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

        var removeTrigers = new List<Trigger>();

        foreach (var trigger in Player.Triggers)
        {
            trigger.Run();
            if (trigger.Remove)
            {
                removeTrigers.Add(trigger);
            }
        }
        foreach (var trigger in removeTrigers)
        {
            Player.Triggers.Remove(trigger);
        }
    }
    public void Draw()
    {
        // Program.Ui.Clear();

        if ((Map.CurrentInteraction != null || Map.CurrentInteractionIndex != -1) && Map.CurrentInteraction.IsActive)
        {
            if (Map.CurrentInteraction.IsMapDrawn)
            {
                Map.Draw();

                int x = Player.Position.X + (GameSettings.GAME_WIDTH / 2) - (Map.Width / 2);
                int y = Player.Position.Y + (GameSettings.GAME_HEIGHT / 2) - (Map.Height / 2);

                Program.Ui.Print(x, y, Player.Glyph.ToString(), Player.Color);
            }

            Map.CurrentInteraction.Draw();
        }
        else
        {
            Map.Draw();

            int x = Player.Position.X + (GameSettings.GAME_WIDTH / 2) - (Map.Width / 2);
            int y = Player.Position.Y + (GameSettings.GAME_HEIGHT / 2) - (Map.Height / 2);

            // int x = GameSettings.GAME_WIDTH / 2;
            // int y = GameSettings.GAME_HEIGHT / 2;

            Program.Ui.Print(x, y, Player.Glyph.ToString(), Player.Color);
        }
    }
}