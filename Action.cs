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
        }

        var interMapHandle = Map.InterMapDict[_map.XpMapString];

        if (interMapHandle.ContainsKey(newPosition))
        {

            var interMapTuple = interMapHandle[newPosition];
            _game.Map = _map.GetMap(interMapTuple.Item1);
            _game.Player.Position = interMapTuple.Item2;

            _ui.Console.Clear();

        }

    }
}
