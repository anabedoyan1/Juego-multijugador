using System.Collections.Generic;

public static class RailManager
{
    private static List<(RailNode, RailNode)> railTuples = new List<(RailNode, RailNode)>();

    public static RailedObject localPlayerRailedObject = null;

    public static List<(RailNode, RailNode)> GetRails() => railTuples;

    public static void AddRailroad((RailNode, RailNode) newRail)
    {
        railTuples.Add(newRail);
    }
}
