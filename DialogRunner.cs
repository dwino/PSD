using System;
using System.IO;
using System.Linq;
using Balance.Ui;
using Yarn;
using Yarn.Compiler;

namespace Balance;

public class DialogRunner
{
    private int _x;
    private int _y;
    private GameUi _ui;
    private bool _optionRequired;
    public DialogRunner(string file, string startNode, GameUi ui)
    {
        _x = 1;
        _y = 1;
        _ui = ui;
        _optionRequired = false;
        string[] sourceFiles = { file };
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

        Dialogue.LineHandler = LineHandler;
        Dialogue.CommandHandler = CommandHandler;
        Dialogue.OptionsHandler = OptionsHandler;
        Dialogue.NodeCompleteHandler = NodeCompleteHandler;
        Dialogue.DialogueCompleteHandler = DialogueCompleteHandler;

        SelectedOptionIndex = -1;
        LinesToDraw = new List<string>();
        OptionsToDraw = new List<string>();

    }
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
        _optionRequired = true;
        SelectedOptionIndex = 0;

        int count = 0;
        foreach (var option in options.Options)
        {
            OptionsToDraw.Add($"{count}: {TextForLine(option.Line.ID)}");
            count += 1;
        }

        // //Console.WriteLine("Select an option to continue");

        // int number;
        // while (int.TryParse(System.Console.ReadLine(), out number) == false)
        // {
        //     System.Console.WriteLine($"Select an option between 0 and {options.Options.Length - 1} to continue");
        // }

        // // rather than just trapping every possibility we
        // // just mash it into shape
        // number %= options.Options.Length;

        // if (number < 0)
        // {
        //     number *= -1;
        // }


        // Dialogue.SetSelectedOption(number);
    }

    public void NodeCompleteHandler(string completedNodeName)
    {
    }

    public void DialogueCompleteHandler()
    {
    }

    public void ContinueDialog()
    {
        if (!_optionRequired)
        {
            Dialogue.Continue();
        }
    }

    public void Draw(Console console)
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

