using System.Globalization;

namespace WaypointPathfinding;


public static class WaypointMapParser
{
    /// <summary>
    /// Parses a table in a form of an array of string to a WaypointMap.
    /// </summary>
    /// <param name="table">
    /// The table should contain three columns in order. WaypointID, ConnectionsArray (in a string form), and a DistancesArray (in a string form).
    /// The arrays should be in form: connectionIds: [2, 1, 4] and distances: [2.1, 4.2, 2.2]
    /// </param>
    /// <param name="delimiter"></param>
    /// <returns></returns>
    public static WaypointMap ParseArrayOfStrings(
        List<string> table,
        string delimiter=";")
    {
        var result = new WaypointMap();

        foreach (var row in table)
        {
            string[] values = row.Split(delimiter);
            int waypointId = int.Parse(values[0]);
            string[] connectionIds = values[1].Substring(1, values[1].Length-2).Split(",");
            string[] distances = values[2].Substring(1, values[2].Length - 2).Split(",");

            WaypointNode tempNode = null;

            if (!result.WaypointIds.Contains(waypointId))
            {
                tempNode = new WaypointNode(waypointId);
                result.AddWaypoint(tempNode);
            } else
            {
                tempNode = result.Waypoints.First(x => x.Id == waypointId);
            }

            for(int i = 0; i < connectionIds.Length; i++)
            {
                WaypointNode connectionNode = null;
                if (!result.WaypointIds.Contains(int.Parse(connectionIds[i])))
                {
                    connectionNode = new WaypointNode(int.Parse(connectionIds[i]));
                    result.AddWaypoint(connectionNode);
                } else
                {
                    connectionNode = result.Waypoints.First(x => x.Id == int.Parse(connectionIds[i]));
                }
                tempNode.AddConnection(connectionNode, float.Parse(distances[i], NumberStyles.Any, CultureInfo.InvariantCulture));
            }
        }

        return result;
    }
}
