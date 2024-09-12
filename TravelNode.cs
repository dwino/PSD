using System;

namespace Balance;

public class TravelNode
{
    public TravelNode(Map currentMap, Point posCurrentMap)
    {
        CurrentMap = currentMap;
        PositionCurrentMap = posCurrentMap;
        TravelToMap = "";
        PositionTravelToMap = (0, 0);
    }

    public Map CurrentMap { get; set; }
    public Point PositionCurrentMap { get; set; }
    public string TravelToMap { get; set; }
    public Point PositionTravelToMap { get; set; }


}
