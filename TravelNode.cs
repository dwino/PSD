using System;

namespace Balance;

public class TravelNode
{
    public TravelNode(Map currentMap, Point posCurrentMap, Map travelToMap, Point posTravelToMap)
    {
        CurrentMap = currentMap;
        PositionCurrentMap = posCurrentMap;
        TravelToMap = travelToMap;
        PositionTravelToMap = posTravelToMap;

    }

    public Map CurrentMap { get; set; }
    public Point PositionCurrentMap { get; set; }
    public Map TravelToMap { get; set; }
    public Point PositionTravelToMap { get; set; }


}
