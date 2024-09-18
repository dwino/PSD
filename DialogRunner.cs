
using Yarn;
using Yarn.Compiler;

namespace Balance;

public abstract class DialogueRunner
{
    private string _startNode;
    protected DialogueRunner(string name, string startNode = "Start")
    {
        Name = name;
        _startNode = startNode;
        IsActive = false;
        IsMapDrawn = true;
        OptionRequired = false;
        string[] sourceFiles = { "Content/Yarn/" + name + ".yarn" };
        var compilationJob = CompilationJob.CreateFromFiles(sourceFiles);
        CompilationResult = Compiler.Compile(compilationJob);
        Program = CompilationResult.Program;
        if (Program.Nodes.ContainsKey(startNode))
        {
            var storage = new Yarn.MemoryVariableStore();
            Dialogue = new Yarn.Dialogue(storage);

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
    public bool IsActive { get; set; }
    public bool IsMapDrawn { get; set; }
    public bool OptionRequired { get; set; }
    public CompilationResult CompilationResult { get; set; }
    public Yarn.Program Program { get; set; }
    public Dialogue Dialogue { get; set; }
    public int SelectedOptionIndex { get; set; }
    public List<String> LinesToDraw { get; set; }
    public List<String> OptionsToDraw { get; set; }

    public string TextForLine(string lineID)
    {
        return CompilationResult.StringTable[lineID].text;
    }

    public void CommandHandler(Yarn.Command command)
    {
    }

    public void LineHandler(Yarn.Line line)
    {
        LinesToDraw.Add(TextForLine(line.ID));
    }

    void OptionsHandler(Yarn.OptionSet options)
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

    public void DialogueCompleteHandler()
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

    public void Reset()
    {
        OptionRequired = false;
        if (Program.Nodes.ContainsKey(_startNode))
        {
            var storage = new Yarn.MemoryVariableStore();
            Dialogue = new Yarn.Dialogue(storage);

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

    public abstract void Draw(Console console);
    public abstract bool IsAvailable(GameEngine game);
}

public class IntroTextScreen : DialogueRunner
{
    public IntroTextScreen(string name) : base(name)
    {
    }

    public override void Draw(Console console)
    {
    }

    public override bool IsAvailable(GameEngine game)
    {
        return true;
    }

}

public class FullScreenInteraction : DialogueRunner
{
    private int _x;
    private int _y;
    private int _minDistToPlayer;

    public FullScreenInteraction(string name, Point position, int minDistToPlayer = 1) : base(name)
    {
        IsMapDrawn = false;
        _x = 1;
        _y = 1;
        _minDistToPlayer = minDistToPlayer;

        Position = position;
    }
    public Point Position { get; set; }

    public override bool IsAvailable(GameEngine game)
    {
        var distanceToPlayer = Distance.Manhattan.Calculate(game.Player.Position, this.Position);

        return Math.Floor(distanceToPlayer) <= _minDistToPlayer;
    }

    public override void Draw(Console console)
    {
        int i = 1;
        foreach (var line in LinesToDraw)
        {
            console.Print(_x, _y + i, line);
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

            console.Print(_x + 5, _y + i, option, color);

            i++;
            j++;
        }

    }
}

public class MapInteraction : DialogueRunner
{
    private int _x;
    private int _y;
    private int _minDistToPlayer;

    public MapInteraction(string name, Point position, int minDistToPlayer = 1) : base(name)
    {
        _x = 1;
        _y = 1;
        _minDistToPlayer = minDistToPlayer;

        Position = position;
    }
    public Point Position { get; set; }

    public override bool IsAvailable(GameEngine game)
    {
        var distanceToPlayer = Distance.Manhattan.Calculate(game.Player.Position, this.Position);

        return Math.Floor(distanceToPlayer) <= _minDistToPlayer;
    }

    public override void Draw(Console console)
    {
        int i = 1;
        foreach (var line in LinesToDraw)
        {
            console.Print(_x, _y + i, line);
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

            console.Print(_x + 5, _y + i, option, color);

            i++;
            j++;
        }

    }
}

