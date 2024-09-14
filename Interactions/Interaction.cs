

using Balance.Ui;

namespace Balance;

public class Interaction : IEvent
{
    private Map _map;
    private int _minDistToPlayer;
    public Interaction(Map map, string name, Point position, List<InteractionNode> interactionNodes,
                        int minDistToPlayer = 1)
    {
        _map = map;
        Name = name;
        Position = position;
        CurrentNode = -1;
        InteractionNodes = interactionNodes;
        _minDistToPlayer = minDistToPlayer;

        ExitAction = false;
        ExitXpMapString = name;
        ExitPosition = position;

    }

    public string Name { get; set; }

    public Point Position { get; set; }

    public int CurrentNode { get; set; }

    // TODO: dictionary, zodat dit niet van de positie afhangt
    public List<InteractionNode> InteractionNodes { get; set; }

    // Dialog(Interaction)List/Dict op basis van flags (in World/Player?)
    // GameModes: RLMode; InteractionMode => Branched input processing

    public bool ExitAction { get; set; }
    public string ExitXpMapString { get; set; }
    public Point ExitPosition { get; set; }



    public bool IsAvailable(GameEngine game)
    {

        var distanceToPlayer = Helper.Distance(game.Player.Position, this.Position);

        return Math.Floor(distanceToPlayer) <= _minDistToPlayer;
    }

    public void WhenAvailable()
    {
        _map.CurrentAvailableInteractions.Add(this);
    }

    public void WhenClosedLoadMap(GameEngine _game, GameUi _ui)
    {
        var ifreModeHandle = (IFREMode)_game.CurrentMode;
        ifreModeHandle.Map = _map.GetMap(ExitXpMapString);
        _game.Player.Position = ExitPosition;

        _ui.Console.Clear();

    }
}
