using System;
using Figgle;

namespace Balance;

public class Helper
{
    public static Interaction LoadInteraction(Map map, string name, Point position)
    {
        var sr = new StreamReader(Path.Combine("Content", "Interactions", name + ".txt"));

        var interactionNodeList = new List<InteractionNode>();
        var line = sr.ReadLine();

        while (line != null)
        {
            if (line.Contains("[node]"))
            {
                line = sr.ReadLine();
                var nodeid = Int32.Parse(line.Split(':')[1]);
                var nodeText = sr.ReadLine();
                var removeFlags = new List<string>();
                var addFlags = new List<string>();
                var nodeOptions = new List<InteractionOption>();

                line = sr.ReadLine();
                if (line.Contains("[flags]"))
                {
                    line = sr.ReadLine();
                    var removeFlagsString = line.Split(':')[1].Split(",");

                    foreach (var flag in removeFlagsString)
                    {
                        addFlags.Add(flag.Trim());
                    }

                    line = sr.ReadLine();
                    var addFlagsString = line.Split(':')[1].Split(",");

                    foreach (var flag in addFlagsString)
                    {
                        addFlags.Add(flag.Trim());
                    }


                    line = sr.ReadLine();

                }
                if (line.Contains("[options]"))
                {
                    line = sr.ReadLine();
                    while (!String.IsNullOrWhiteSpace(line))
                    {
                        var optionText = line.Split('~')[0];
                        int destinationNodeId = Int32.Parse(line.Split('~')[1]);

                        var nodeOption = new InteractionOption(optionText, destinationNodeId);
                        nodeOptions.Add(nodeOption);
                        line = sr.ReadLine();
                    }
                }


                interactionNodeList.Add(new InteractionNode(nodeid, nodeText, nodeOptions, removeFlags, addFlags));
            }

            line = sr.ReadLine();
        }

        return new Interaction(map, name, position, interactionNodeList);
    }

    public static int MapRange(int a1, int a2, int b1, int b2, int s) => b1 + (s - a1) * (b2 - b1) / (a2 - a1);

    public static double Distance(Point pt1, Point pt2)
    {
        var x = Math.Abs(pt1.X - pt2.X);
        var y = Math.Abs(pt1.Y - pt2.Y);

        var xPow = Math.Pow(x, 2);
        var yPow = Math.Pow(y, 2);

        return Math.Sqrt(xPow + yPow);
    }

    public static Random Rnd { get; set; } = new Random();

    public static bool Died(Entity entity)
    {
        return entity.Life <= 0;
    }
    public static void DrawFiglet(int x, int y, char c, Color color, Console cons, int x_offset = 0, int y_offset = 0)
    {
        int _x = x + x_offset;
        int _y = y + y_offset;

        using (StringReader reader = new StringReader(FiggleFonts.Doh.Render(c.ToString())))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (!string.IsNullOrWhiteSpace(line.Trim()))
                {
                    cons.Print(_x, _y, line, color);
                    _y++;
                }
            }
        }
    }

    public static void DrawFiglet(int x, int y, string figletString, Color color, Console cons, int x_offset = 0, int y_offset = 0)
    {
        int _x = x + x_offset;
        int _y = y + y_offset;



        using (StringReader reader = new StringReader(figletString))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (!string.IsNullOrWhiteSpace(line.Trim()))
                {
                    cons.Print(_x, _y, line, color);
                    _y++;
                }
            }
        }
    }
}
