using System;
using System.IO;
using System.Linq;
using Yarn;
using Yarn.Compiler;

namespace Balance;

public static class YarnSpinner
{
    public static CompilationResult CompileFile(string file)
    {
        string[] sourceFiles = { file };
        var compilationJob = CompilationJob.CreateFromFiles(sourceFiles);
        return Compiler.Compile(compilationJob);
    }
    public static void RunFiles(CompilationResult results, string startNode, Console console)
    {
        int x = 1;
        int y = 1;

        string TextForLine(string lineID)
        {
            return results.StringTable[lineID].text;
        }

        var program = results.Program;

        if (program.Nodes.ContainsKey(startNode))
        {
            var storage = new Yarn.MemoryVariableStore();
            var dialogue = new Yarn.Dialogue(storage);

            dialogue.SetProgram(program);
            dialogue.SetNode(startNode);

            void CommandHandler(Yarn.Command command)
            {
            }

            void LineHandler(Yarn.Line line)
            {
                console.Print(x, y, TextForLine(line.ID));
                y++;
                //Console.ReadLine();
            }

            void OptionsHandler(Yarn.OptionSet options)
            {

                int count = 0;
                foreach (var option in options.Options)
                {
                    console.Print(10, y, $"{count}: {TextForLine(option.Line.ID)}");
                    count += 1;
                }

                //Console.WriteLine("Select an option to continue");

                int number;
                while (int.TryParse(System.Console.ReadLine(), out number) == false)
                {
                    System.Console.WriteLine($"Select an option between 0 and {options.Options.Length - 1} to continue");
                }

                // rather than just trapping every possibility we
                // just mash it into shape
                number %= options.Options.Length;

                if (number < 0)
                {
                    number *= -1;
                }


                dialogue.SetSelectedOption(number);
            }

            void NodeCompleteHandler(string completedNodeName)
            {
            }

            void DialogueCompleteHandler()
            {
            }

            dialogue.LineHandler = LineHandler;
            dialogue.CommandHandler = CommandHandler;
            dialogue.OptionsHandler = OptionsHandler;
            dialogue.NodeCompleteHandler = NodeCompleteHandler;
            dialogue.DialogueCompleteHandler = DialogueCompleteHandler;

            try
            {
                do
                {
                    dialogue.Continue();
                }
                while (dialogue.IsActive);
            }
            catch (InvalidOperationException ex)
            {
            }
        }
        else
        {
        }
    }
}

