using Balance.Ui;

namespace Balance;

public abstract class Action
{
    protected GameUi _ui;
    protected GameEngine _game;
    protected Action(GameUi ui, GameEngine game)
    {
        _ui = ui;
        _game = game;
    }

    public abstract void Execute();
}


public class MoveByAction : Action
{
    private Point _offset;
    private Map _map;
    public MoveByAction(GameUi ui, GameEngine game, Map map, Point offset) : base(ui, game)
    {
        _offset = offset;
        _map = map;
    }

    public override void Execute()
    {
        var newPosition = _game.Player.Position + _offset;

        if ((char)_map.RPImg.ToCellSurface()[1].GetGlyph(newPosition.X, newPosition.Y) == '1')
        {
            _game.Player.Position = newPosition;

            var interMapDict = Map.InterMapDict;
            var xpMapString = _map.XpMapString;

            var interMapHandle = interMapDict[xpMapString];

            if (interMapHandle.ContainsKey(newPosition))
            {
                var oldPosition = _game.Player.Position;
                var interMapTuple = interMapHandle[newPosition];
                var newMap = _map.GetMap(interMapTuple.Item1);
                _game.ChangeMap(newMap);
                _game.Player.Position = interMapTuple.Item2;

                _ui.Console.Clear();
                _ui.GameScreen.CurrentAnimation = new MapTransitionAnimation(_map.VisibleMap, _game.Player, oldPosition, _offset, _ui.Console, _ui);
                _ui.GameScreen.CurrentAnimation.IsRunning = true;
            }

            _map.AvailableInteractions.Clear();
            foreach (var interaction in _map.Interactions)
            {
                if (interaction.IsAvailable(_game))
                {
                    _map.AvailableInteractions.Add(interaction);
                }
            }
            foreach (var entity in _map.Entities)
            {
                if (entity.Interaction != null && entity.Interaction.IsAvailable(_game))
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

