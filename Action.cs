using Balance.Ui;

namespace Balance;

public abstract class Action
{
    protected Action()
    {
    }

    public abstract void Execute();
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

            var interMapDict = Map.InterMapDict;
            var xpMapString = _map.XpMapString;

            var interMapHandle = interMapDict[xpMapString];

            if (interMapHandle.ContainsKey(newPosition))
            {
                var oldPosition = Program.Engine.Player.Position;
                var interMapTuple = interMapHandle[newPosition];
                var newMap = Map.GetMap(interMapTuple.Item1);
                Program.Engine.ChangeMap(newMap);
                Program.Engine.Player.Position = interMapTuple.Item2;

                Program.Ui.DrawConsole.Clear();
                Program.Ui.GameScreen.AddAnimation(new MapTransitionAnimation(_map.VisibleMap, Program.Engine.Player, oldPosition, _offset));
            }

            _map.AvailableInteractions.Clear();
            foreach (var interaction in _map.Interactions)
            {
                if (interaction.IsAvailable() && !interaction.AutoActivated)
                {
                    _map.AvailableInteractions.Add(interaction);
                }
            }
            foreach (var entity in _map.Entities)
            {
                if (entity.Interaction != null && entity.Interaction.IsAvailable())
                {
                    _map.AvailableInteractions.Add(entity.Interaction);
                }

            }

            if (_map.AvailableInteractions.Count > 0)
            {
                _map.CurrentInteractionIndex = 0;
            }
            else
            {
                _map.CurrentInteractionIndex = -1;
            }
        }
    }
}

