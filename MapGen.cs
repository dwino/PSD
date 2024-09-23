using System.Runtime.InteropServices;
using DeBroglie;
using DeBroglie.Models;
using DeBroglie.Rot;
using DeBroglie.Topo;
using SadConsole.Readers;

namespace Balance;

public static class SpaceShipGenerator
{
    public static int[,] MainGrid { get; set; }
    public static int Width { get; set; }
    public static int Height { get; set; }
    public static int[,] CenterGrid { get; set; }
    public static int CenterGridWith => Width / 4;
    public static int CenterGridHeight => Height / 4;
    public static int GirdInterval;

    public static void Init(int width, int height)
    {
        Width = width;
        Height = height;
        MainGrid = new int[Width, Height];
        CenterGrid = new int[Width / 4, Height / 4];
        GirdInterval = 10;
    }

    public static void Generate()
    {
        MainGrid = new int[Width, Height];
        CenterGrid = new int[Width / 4, Height / 4];

        RectangleFill();

        CircleFill();
        SeparateCenter();
        FillShipCenter();

        MirrorRefGrid();

        ApplyGrid();
    }

    private static void RectangleFill()
    {
        var minRect = 10;
        var maxRect = 15;
        var minWidth = Width / 10;
        var maxWidth = Width / 5;
        var minHeight = Width / 10;
        var maxHeight = Width / 5;

        var width1 = Helper.Rnd.Next(minWidth, maxWidth);
        var height1 = Helper.Rnd.Next(minHeight, maxHeight);

        var x1 = Helper.Rnd.Next(2, (Width / 4));
        var y1 = Helper.Rnd.Next(Height / 3, Height / 2);

        FillRect(x1, y1, width1, height1);

        var width2 = Helper.Rnd.Next(minWidth, maxWidth);
        var height2 = Helper.Rnd.Next(minHeight, maxHeight);

        var x2 = Helper.Rnd.Next(Width / 3, (Width / 2));
        var y2 = Helper.Rnd.Next(2, (Height / 4));
        FillRect(x2, y2, width2, height2);


        int limit = Helper.Rnd.Next(minRect, maxRect);
        int i = 0;

        while (i < limit)
        {
            var width = Helper.Rnd.Next(minWidth, maxWidth);
            var height = Helper.Rnd.Next(minHeight, maxHeight);

            var x = Helper.Rnd.Next(2, (Width / 2) - minWidth);
            var y = Helper.Rnd.Next(2, (Height / 2) - minHeight);

            FillRect(x, y, width, height);

            i++;

        }

    }

    private static void FillRect(int i, int j, int width, int height)
    {
        for (int x = i; x < i + width; x++)
        {
            for (int y = j; y < j + height; y++)
            {
                MainGrid[x, y] = 1;
            }
        }
    }

    private static void CircleFill()
    {
        FillCircle(CenterGridWith, CenterGridHeight, Helper.Rnd.Next(CenterGridWith / 3, CenterGridWith / 2));

        int limit = Helper.Rnd.Next(1, 3);
        int i = 0;

        while (i < limit)
        {
            var x = Helper.Rnd.Next(CenterGridWith / 2, CenterGridWith + CenterGridWith / 4);
            var y = Helper.Rnd.Next(CenterGridHeight / 2, CenterGridHeight + CenterGridHeight / 4);
            var radius = Helper.Rnd.Next(CenterGridWith / 5, CenterGridWith / 2);
            i++;

            FillCircle(x, y, radius);
        }

        var x1 = Helper.Rnd.Next(0, CenterGridWith);
        var y1 = Helper.Rnd.Next(0, CenterGridHeight);
        var radius1 = Helper.Rnd.Next(CenterGridWith / 5, CenterGridWith / 2);

        FillCircle(x1, y1, Helper.Rnd.Next(CenterGridWith / 3, CenterGridWith / 2));

    }

    private static void FillCircle(double centerX, double centerY, double radius)
    {
        int top = (int)Math.Ceiling(centerY - radius);
        int bottom = (int)Math.Floor(centerY + radius);

        for (int y = top; y <= bottom; y++)
        {
            int dy = (int)(y - centerY);
            double dx = Math.Sqrt(radius * radius - dy * dy);
            int left = (int)Math.Ceiling(centerX - dx);
            int right = (int)Math.Floor(centerX + dx);
            for (int x = left; x <= right; x++)
            {
                if (InBounds(x, y, CenterGridWith, CenterGridHeight))
                {
                    CenterGrid[x, y] = 1;

                }
            }
        }
    }


    private static void RandomFill()
    {
        for (int x = 0; x < CenterGridWith; x++)
        {
            for (int y = 0; y < CenterGridHeight; y++)
            {
                if (Helper.Rnd.Next(2) == 0)
                {
                    CenterGrid[x, y] = 1;
                }
                else
                {
                    CenterGrid[x, y] = 0;
                }
            }
        }
    }

    private static void SeparateCenter()
    {
        var y = 0;
        for (int x = 0; x < CenterGridWith; x++)
        {
            CenterGrid[x, y] = 0;
        }
        var x1 = 0;
        for (int y1 = 0; y1 < CenterGridHeight; y1++)
        {
            CenterGrid[x1, y1] = 0;
        }
    }

    private static void FillShipCenter()
    {
        for (int x = 0; x < CenterGridWith; x++)
        {
            for (int y = 0; y < CenterGridHeight; y++)
            {
                MainGrid[x + CenterGridWith, y + CenterGridHeight] = CenterGrid[x, y];
            }
        }
    }

    private static void MirrorRefGrid()
    {
        for (int x = 0; x < Width / 2; x++)
        {
            for (int y = 0; y < Height / 2; y++)
            {
                MainGrid[Width - 1 - x, y] = MainGrid[x, y];
            }
        }

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height / 2; y++)
            {
                MainGrid[x, Height - 1 - y] = MainGrid[x, y];
            }
        }
    }

    private static void ApplyGrid()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y += GirdInterval)
            {
                MainGrid[x, y] = 0;
            }
        }

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x += GirdInterval)
            {
                MainGrid[x, y] = 0;
            }
        }
    }

    public static void DrawFullMap()
    {
        Program.Ui.Clear();
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                if (MainGrid[x, y] == 1)
                {
                    Program.Ui.Print(x, y, ((char)219).ToString(), Color.White);
                }
            }
        }
    }

    public static bool InBounds(int x, int y, int width, int height)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

}

public static class WFCMap
{
    public static int[,] Output { get; set; }
    static int _xoutput = 40;
    static int _youtput = 40;
    public static void Generate()
    {

        var xpMapString = "WFCMap";
        var path = "Content/Maps/" + xpMapString + ".xp";
        var RPImg = SadConsole.Readers.REXPaintImage.Load(new FileStream(path, FileMode.Open));

        var VisibleMap = (CellSurface?)RPImg.ToCellSurface()[0]!;

        int[,] array = new int[VisibleMap.Width, VisibleMap.Height];

        for (int x = 0; x < VisibleMap.Width; x++)
        {
            for (int y = 0; y < VisibleMap.Height; y++)
            {
                array[x, y] = VisibleMap.GetGlyph(x, y);
            }
        }



        // Define some sample data
        ITopoArray<int> sample = TopoArray.Create(array, periodic: false);



        // Specify the model used for generation
        // var model = new OverlappingModel(3);
        var model = new OverlappingModel(2);

        //var rotations = new TileRotation(4, true);
        model.AddSample(sample.ToTiles());//, rotations);

        // Set the output dimensions
        var topology = new GridTopology(_xoutput, _youtput, periodic: false);
        // Acturally run the algorithm
        var propagator = new TilePropagator(model, topology);
        var status = propagator.Run();
        if (status != Resolution.Decided) throw new Exception("Undecided");
        var output = propagator.ToValueArray<int>();

        Output = output.ToArray2d();
    }

    public static void Draw()
    {
        // Display the results
        for (var x = 0; x < _xoutput; x++)
        {
            for (var y = 0; y < _youtput; y++)
            {
                if (x == 0 || x == _xoutput - 1 || y == 0 || y == _youtput - 1)
                {
                    Program.Ui.Print(x, y, ((char)219).ToString(), Color.White);
                }
                else
                {
                    Program.Ui.Print(x, y, ((char)Output[x, y]).ToString());
                }
            }
        }
    }


}
