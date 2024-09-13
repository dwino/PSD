using System;

namespace Balance;

public class TravelNode
{
    public static Dictionary<string, Dictionary<Point, (string, Point)>> InterMapDict
                            = new Dictionary<string, Dictionary<Point, (string, Point)>>(){
                                {"cryo", new Dictionary<Point, (string, Point)>(){
                                    {(24,6), ("sleepingPod", (2,3))},
                                }},
                                {"sleepingPod", new Dictionary<Point, (string, Point)>(){
                                    {(3,3), ("cryo", (23,6))},
                                }},
                            };
}
