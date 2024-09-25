
using Yarn;
using Yarn.Compiler;

namespace Balance;

public abstract class DialogueRunner
{
    protected string _startNode;
    protected DialogueRunner(string name, string startNode = "Start", bool autoActivated = false)
    {
        Name = name;
        _startNode = startNode;
        AutoActivated = autoActivated;
        _isActive = false;
        OptionRequired = false;
        string[] sourceFiles = { "Content/Yarn/" + name + ".yarn" };
        var compilationJob = CompilationJob.CreateFromFiles(sourceFiles);
        CompilationResult = Compiler.Compile(compilationJob);
        Program = CompilationResult.Program;
        MemoryVariableStore = new Yarn.MemoryVariableStore();

        if (Program.Nodes.ContainsKey(startNode))
        {
            if (Map.MemoryPalace.ContainsKey(name))
            {
                MemoryVariableStore = Map.MemoryPalace[name];
            }

            Dialogue = new Yarn.Dialogue(MemoryVariableStore);

            Dialogue.SetProgram(Program);
            Dialogue.SetNode(startNode);
        }

        Dialogue!.LineHandler = LineHandler;
        Dialogue.CommandHandler = CommandHandler;
        Dialogue.OptionsHandler = OptionsHandler;
        Dialogue.NodeCompleteHandler = NodeCompleteHandler;
        Dialogue.DialogueCompleteHandler = DialogueCompleteHandler;

        SelectedOptionIndex = -1;
        LinesToDraw = new List<string>();
        OptionsToDraw = new List<string>();
    }
    public string Name { get; set; }
    public CompilationResult CompilationResult { get; set; }
    public Yarn.Program Program { get; set; }
    public Dialogue Dialogue { get; set; }
    public MemoryVariableStore MemoryVariableStore { get; set; }

    public bool AutoActivated { get; set; }
    protected bool _isActive;
    public virtual bool IsActive { get => _isActive; set => _isActive = value; }
    public bool OptionRequired { get; set; }

    public int SelectedOptionIndex { get; set; }
    public List<String> LinesToDraw { get; set; }
    public List<String> OptionsToDraw { get; set; }

    public string TextForLine(string lineID)
    {
        return CompilationResult.StringTable[lineID].text;
    }

    public void CommandHandler(Yarn.Command command)
    {
        CommandParser.Parse(command.Text);
        ContinueDialog();
    }

    public void LineHandler(Yarn.Line line)
    {
        var newLine = TextForLine(line.ID);

        LinesToDraw.Add(newLine);
    }

    public void OptionsHandler(Yarn.OptionSet options)
    {
        OptionRequired = true;
        SelectedOptionIndex = 0;

        int count = 0;
        foreach (var option in options.Options)
        {
            OptionsToDraw.Add(TextForLine(option.Line.ID));
            count += 1;

        }
    }

    public void SetSelectedOption()
    {
        Dialogue.SetSelectedOption(SelectedOptionIndex);
        LinesToDraw.Clear();
        OptionsToDraw.Clear();
        OptionRequired = false;
        ContinueDialog();
    }

    public void NodeCompleteHandler(string completedNodeName)
    {
    }

    public virtual void DialogueCompleteHandler()
    {
        IsActive = false;
        Reset();
    }

    public void ContinueDialog()
    {
        if (!OptionRequired)
        {
            Dialogue.Continue();
        }
    }

    public virtual void Reset()
    {
        OptionRequired = false;
        if (Program.Nodes.ContainsKey(_startNode))
        {
            if (Map.MemoryPalace.ContainsKey(Name))
            {
                MemoryVariableStore = Map.MemoryPalace[Name];
            }

            Dialogue = new Yarn.Dialogue(MemoryVariableStore);
            Dialogue.SetProgram(Program);
            Dialogue.SetNode(_startNode);
        }

        Dialogue!.LineHandler = LineHandler;
        Dialogue.CommandHandler = CommandHandler;
        Dialogue.OptionsHandler = OptionsHandler;
        Dialogue.NodeCompleteHandler = NodeCompleteHandler;
        Dialogue.DialogueCompleteHandler = DialogueCompleteHandler;

        SelectedOptionIndex = -1;
        LinesToDraw = new List<string>();
        OptionsToDraw = new List<string>();
    }

    public abstract void Draw();
    public abstract bool IsAvailable();
}

public abstract class TextScreenDialog : DialogueRunner
{
    protected TextScreenDialog(string name, string startNode = "Start", bool autoActivated = false) : base(name, startNode, autoActivated)
    {
    }
}

public class BasicTextScreenDialog : TextScreenDialog
{
    public BasicTextScreenDialog(string name) : base(name)
    {
    }

    public override void Draw()
    {
    }

    public override bool IsAvailable()
    {
        return true;
    }

}

public abstract class MapBoundInteraction : DialogueRunner
{
    protected Map _map;
    protected Func<bool> _isAvailable;

    protected MapBoundInteraction(string name, Map map, Point position, Func<bool> isAvailable, string startNode = "Start", bool autoActivated = false) : base(name, startNode, autoActivated)
    {
        _map = map;
        Position = position;
        _isAvailable = isAvailable;
    }

    public Point Position { get; set; }
    public bool IsMapDrawn { get; set; }

    public override bool IsActive
    {
        get => _isActive;
        set
        {
            // if (_map != null)
            // {
            if (value)
            {
                _map.CurrentInteraction = this;
                _map.CurrentInteractionIndex = int.MaxValue;
                if (Map.MemoryPalace.ContainsKey(Name))
                {
                    MemoryVariableStore = Map.MemoryPalace[Name];
                }
            }
            else
            {
                _map.CurrentInteraction = null!;
                _map.CurrentInteractionIndex = -1;
            }
            // }
            _isActive = value;

        }
    }

    public override void DialogueCompleteHandler()
    {
        if (_map != null)
        {
            _map.CurrentInteraction = null!;
        }

        Map.MemoryPalace[Name] = MemoryVariableStore;
        base.DialogueCompleteHandler();
    }

    public override void Reset()
    {
        base.Reset();
    }
}



public class FullScreenInteraction : MapBoundInteraction
{
    private int _x;
    private int _y;
    private int _minDistToPlayer;

    public FullScreenInteraction(string name, Map map, Point position, Func<bool> isAvailable = null, int minDistToPlayer = 1) : base(name, map, position, isAvailable)
    {
        IsMapDrawn = false;
        _x = 1;
        _y = 1;
        _minDistToPlayer = minDistToPlayer;

        Position = position;
    }

    public override bool IsAvailable()
    {
        if (_isAvailable != null)
        {
            return _isAvailable();
        }
        else
        {
            var distanceToPlayer = Distance.Manhattan.Calculate(Balance.Program.Engine.Player.Position, this.Position);

            return Math.Floor(distanceToPlayer) <= _minDistToPlayer;
        }
    }

    public override void Draw()
    {
        int i = 1;
        foreach (var line in LinesToDraw)
        {
            Balance.Program.Ui.DrawConsole.Print(_x, _y + i, line);
            i++;
        }
        int j = 0;
        var color = Color.White;
        foreach (var option in OptionsToDraw)
        {
            if (j == SelectedOptionIndex)
            {
                color = new Color(0, 217, 0);
            }
            else
            {
                color = Color.White;
            }

            Balance.Program.Ui.DrawConsole.Print(_x + 5, _y + i, option, color);

            i++;
            j++;
        }

    }
}

public class MapAutoInteraction : MapBoundInteraction
{
    private int _x;
    private int _y;

    public MapAutoInteraction(string name, Map map, Point position, Func<bool> isAvailable = null) : base(name, map, position, isAvailable)
    {
        _x = 1;
        _y = 1;

        _map = map;
        IsMapDrawn = true;
        Position = position;
        AutoActivated = true;
    }

    public override bool IsAvailable()
    {
        if (_isAvailable != null)
        {
            var result = _isAvailable();

            if (result)
            {
                IsActive = true;
                ContinueDialog();
            }

            return result;
        }
        else
        {
            var result = Balance.Program.Engine.Player.Position == this.Position;

            if (result)
            {
                IsActive = true;
                ContinueDialog();
            }

            return result;
        }
    }

    public override void Draw()
    {
        int i = 1;
        foreach (var line in LinesToDraw)
        {
            Balance.Program.Ui.DrawConsole.Print(_x, _y + i, line);
            i++;
        }
        int j = 0;
        var color = Color.White;
        foreach (var option in OptionsToDraw)
        {
            if (j == SelectedOptionIndex)
            {
                color = new Color(0, 217, 0);
            }
            else
            {
                color = Color.White;
            }

            Balance.Program.Ui.DrawConsole.Print(_x + 5, _y + i, option, color);

            i++;
            j++;
        }
    }
}

public class GaurdingMapAutoInteraction : MapBoundInteraction
{
    private int _x;
    private int _y;
    private Condition _condition;
    private bool passedGaurd;

    public GaurdingMapAutoInteraction(string name, Map map, Point position, Action gaurdedAction, Action alternativeAction, Condition condition, Func<bool> isAvailable = null)
    : base(name, map, position, isAvailable)
    {
        _x = 1;
        _y = 1;
        _condition = condition;

        _map = map;
        IsMapDrawn = true;
        Position = position;
        AutoActivated = true;

        GuardedAction = gaurdedAction;
        AlternativeAction = alternativeAction;

        passedGaurd = false;
    }

    public Action GuardedAction { get; set; }
    public Action AlternativeAction { get; set; }

    public override bool IsAvailable()
    {
        if (_isAvailable != null)
        {
            var result = _isAvailable();

            if (result)
            {
                IsActive = true;

                if (Map.MemoryPalace.ContainsKey(_condition.Dialogue))
                {
                    Map.MemoryPalace[_condition.Dialogue].TryGetValue(_condition.Variable, out passedGaurd);
                }

                MemoryVariableStore.SetValue(_condition.Variable, passedGaurd);
                ContinueDialog();
            }

            return result;
        }
        else
        {
            var result = Balance.Program.Engine.Player.Position == this.Position;

            if (result)
            {
                IsActive = true;

                if (Map.MemoryPalace.ContainsKey(_condition.Dialogue))
                {
                    Map.MemoryPalace[_condition.Dialogue].TryGetValue(_condition.Variable, out passedGaurd);
                }

                MemoryVariableStore.SetValue(_condition.Variable, passedGaurd);
                ContinueDialog();
            }

            return result;
        }
    }

    public override void DialogueCompleteHandler()
    {
        if (passedGaurd)
        {
            Balance.Program.Engine.Player.NextAction = GuardedAction;
        }
        else
        {
            Balance.Program.Engine.Player.NextAction = AlternativeAction;

        }


        base.DialogueCompleteHandler();
    }

    public override void Draw()
    {
        int i = 1;
        foreach (var line in LinesToDraw)
        {
            Balance.Program.Ui.DrawConsole.Print(_x, _y + i, line);
            i++;
        }
        int j = 0;
        var color = Color.White;
        foreach (var option in OptionsToDraw)
        {
            if (j == SelectedOptionIndex)
            {
                color = new Color(0, 217, 0);
            }
            else
            {
                color = Color.White;
            }

            Balance.Program.Ui.DrawConsole.Print(_x + 5, _y + i, option, color);

            i++;
            j++;
        }
    }
}

public class MapInteraction : MapBoundInteraction
{
    private int _x;
    private int _y;
    private int _minDistToPlayer;

    public MapInteraction(string name, Map map, Point position, Func<bool> isAvailable = null, int minDistToPlayer = 1) : base(name, map, position, isAvailable)
    {
        _x = 1;
        _y = 1;
        _minDistToPlayer = minDistToPlayer;
        IsMapDrawn = true;

        Position = position;
    }

    public override bool IsAvailable()
    {
        if (_isAvailable != null)
        {
            return _isAvailable();
        }
        else
        {
            var distanceToPlayer = Distance.Manhattan.Calculate(Balance.Program.Engine.Player.Position, this.Position);

            return Math.Floor(distanceToPlayer) <= _minDistToPlayer;
        }
    }

    public override void Draw()
    {
        int i = 1;
        foreach (var line in LinesToDraw)
        {
            Balance.Program.Ui.DrawConsole.Print(_x, _y + i, line);
            i++;
        }
        int j = 0;
        var color = Color.White;
        foreach (var option in OptionsToDraw)
        {
            if (j == SelectedOptionIndex)
            {
                color = new Color(0, 217, 0);
            }
            else
            {
                color = Color.White;
            }

            Balance.Program.Ui.DrawConsole.Print(_x + 5, _y + i, option, color);

            i++;
            j++;
        }

    }
}