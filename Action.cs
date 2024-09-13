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

public class BalanceAction : Action
{
    public BalanceAction(Entity balancer, Entity balancee, GameUi ui, GameEngine game) : base(ui, game)
    {
        _balancer = balancer;
        _balancee = balancee;
    }

    private Entity _balancer;
    private Entity _balancee;

    public override void Execute()
    {
        if (_balancer.White > _balancer.WhiteBalance)
        {
            int amount = _balancer.White - _balancer.WhiteBalance;
            if (_balancee.Black - amount >= 0)
            {
                _balancee.White += amount;
                _balancee.Black -= amount;
            }
        }
        else if (_balancer.Black > _balancer.BlackBalance)
        {

            int amount = _balancer.Black - _balancer.BlackBalance;
            if (_balancee.White - amount >= 0)
            {
                _balancee.White -= amount;
                _balancee.Black += amount;
            }
        }
    }
}

public class InterfereAction : Action
{
    public InterfereAction(Entity interferer, Entity interferee, SRPGMode mode, GameUi ui, GameEngine game) : base(ui, game)
    {
        _mode = mode;
        _interferer = interferer;
        _interferee = interferee;
    }

    private SRPGMode _mode;
    private Entity _interferer;
    private Entity _interferee;

    public override void Execute()
    {
        if (_interferer.White > _interferer.WhiteBalance)
        {
            var amountWhiteUnbalance = _interferer.White - _interferer.WhiteBalance;
            _mode.Unbalance += amountWhiteUnbalance;
            _interferee.Life -= amountWhiteUnbalance;

            if (_interferer == _game.Player)
            {
                _mode.CurrentAnimation = new AttackAnimation(_ui.Console, _ui, _mode);
                string messageLog = "you hit the " + _mode.Other.Glyph.ToString() + " for " + amountWhiteUnbalance + " damage.";
                _mode.MessageLog = new MessageLogAnimation(_ui.Console, _ui, messageLog);
            }


        }
        else if (_interferer.Black > _interferer.BlackBalance)
        {
            if (_interferer.Life < _interferer.LifeMax)
            {
                int amountBlackUnbalance = _interferer.Black - _interferer.BlackBalance;
                _mode.Unbalance += amountBlackUnbalance;
                _interferer.Life += Math.Min(amountBlackUnbalance, _interferer.LifeMax - _interferer.Life);

                if (_interferer == _game.Player)
                {
                    _mode.CurrentAnimation = new HealAnimation(_ui.Console, _ui, _game.Player, _mode);
                }

            }
        }
    }
}

public class ShiftWhiteToBlackAction : Action
{
    public ShiftWhiteToBlackAction(Entity entity, GameUi ui, GameEngine game) : base(ui, game)
    {
        _entity = entity;
    }

    private Entity _entity;

    public override void Execute()
    {
        if (_entity.White > 0)
        {
            int amount = 1;
            _entity.White -= amount;
            _entity.Black += amount;
        }
    }
}

public class ShiftBlackToWhiteAction : Action
{
    public ShiftBlackToWhiteAction(Entity entity, GameUi ui, GameEngine game) : base(ui, game)
    {
        _entity = entity;
    }

    private Entity _entity;

    public override void Execute()
    {
        if (_entity.Black > 0)
        {
            int amount = 1;
            _entity.Black -= amount;
            _entity.White += amount;
        }
    }
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

            var ifreModeHandle = (IFREMode)_game.CurrentMode;
            var interMapTuple = interMapHandle[newPosition];
            ifreModeHandle.Map = _map.GetMap(interMapTuple.Item1);
            _game.Player.Position = interMapTuple.Item2;

            _ui.Console.Clear();

        }

    }
}
