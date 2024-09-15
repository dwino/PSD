

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
        _isActive = false;
        Name = name;
        Position = position;
        CurrentNodeIndex = -1;
        InteractionNodes = interactionNodes;
        _minDistToPlayer = minDistToPlayer;

        ExitAction = false;
        ExitXpMapString = name;
        ExitPosition = position;

    }

    private bool _isActive;
    public bool IsActive
    {
        get { return _isActive; }
        set
        {
            _isActive = value;
            if (_isActive)
            {
                CurrentNodeIndex = 0;
                CurrentNode.CurrentOptionIndex = 0;
            }
        }
    }
    public string Name { get; set; }

    public Point Position { get; set; }


    // TODO: dictionary, zodat dit niet van de positie afhangt
    public List<InteractionNode> InteractionNodes { get; set; }
    public int CurrentNodeIndex { get; set; }
    public InteractionNode CurrentNode
    {
        get
        {
            if (CurrentNodeIndex != -1 && CurrentNodeIndex < InteractionNodes.Count)
            {
                return InteractionNodes[CurrentNodeIndex];
            }
            else
            {
                return null;
            }
        }
    }


    // Dialog(Interaction)List/Dict op basis van flags (in World/Player?)

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
        //_map.Interactions.Add(this);
    }

    public void WhenClosedLoadMap(GameEngine _game, GameUi _ui)
    {
        _game.Map = _map.GetMap(ExitXpMapString);
        _game.Player.Position = ExitPosition;

        _ui.Console.Clear();

    }

    public void Draw(Console console)
    {
        int x = 1;
        int y = 1;
        console.Print(x, y, InteractionNodes[CurrentNodeIndex].Text);
        y++;

        int i = 0;

        foreach (var option in InteractionNodes[CurrentNodeIndex].InteractionOptions)
        {
            string optionString = option.Text;
            if (InteractionNodes[CurrentNodeIndex].CurrentOptionIndex == i)
            {
                console.Print(10, y, ">" + option.Text, new Color(0, 217, 0));
            }
            else
            {
                console.Print(10, y, option.Text);
            }
            y++;
            i++;
        }

    }
}
