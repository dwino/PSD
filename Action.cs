using Balance.Ui;

namespace Balance;

public abstract class Action
{
    protected Action()
    {
    }

    public abstract void Execute();
    public void ReloadInteractions()
    {
        Program.Engine.Map.AvailableInteractions.Clear();
        foreach (var interaction in Program.Engine.Map.Interactions)
        {
            if (interaction.IsAvailable() && !interaction.AutoActivated)
            {
                Program.Engine.Map.AvailableInteractions.Add(interaction);
            }
        }
        foreach (var entity in Program.Engine.Map.Entities)
        {
            if (entity.Interaction != null && entity.Interaction.IsAvailable())
            {
                Program.Engine.Map.AvailableInteractions.Add(entity.Interaction);
            }

        }

        if (Program.Engine.Map.AvailableInteractions.Count > 0)
        {
            Program.Engine.Map.CurrentInteractionIndex = 0;
        }
        else
        {
            Program.Engine.Map.CurrentInteractionIndex = -1;
        }
    }
}

public class DummyAction : Action
{
    public override void Execute()
    {
        ReloadInteractions();
    }
}

public class MoveByAction : Action
{
    private Point _offset;
    private Map _map;
    public MoveByAction(Map map, Point offset) : base()
    {
        _offset = offset;
        _map = map;
    }

    public override void Execute()
    {
        var newPosition = Program.Engine.Player.Position + _offset;

        if ((char)_map.WalkableMap.GetGlyph(newPosition.X, newPosition.Y) == '1')
        {
            Program.Engine.Player.Position = newPosition;

            var interMapDict = MapMemoryHelper.InterMapDict;
            var xpMapString = _map.XpMapString;

            var interMapHandle = interMapDict[xpMapString];

            if (interMapHandle.ContainsKey(newPosition))
            {
                var oldPosition = Program.Engine.Player.Position;
                var interMapTuple = interMapHandle[newPosition];
                var newMap = MapMemoryHelper.GetMap(interMapTuple.Item1);
                Program.Engine.ChangeMap(newMap);
                Program.Engine.Player.Position = interMapTuple.Item2;

                Program.Ui.DrawConsole.Clear();
                Program.Ui.GameScreen.AddAnimation(new MapTransitionAnimation(_map, Program.Engine.Player, oldPosition, _offset));
                _map = newMap;
            }
        }

        ReloadInteractions();
    }
}

