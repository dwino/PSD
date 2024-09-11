using System;
using SadConsole.Readers;
using SharpFont;

namespace Balance;

public class Map : REXPaintImage
{
    public Map(REXPaintImage rpImage) : base(rpImage.Width, rpImage.Height)
    {
        RPImg = rpImage;
    }


    public REXPaintImage RPImg { get; set; }

    public static Map LoadMap(string fileName)
    {
        var rpImg = SadConsole.Readers.REXPaintImage.Load(new FileStream(fileName, FileMode.Open));

        return new Map(rpImg);
    }
}


