using System.Globalization;

namespace WaypointPathfinding;

/// <summary>
/// Class containing useful conversions from and to a <see cref="WaypointMap"/>.
/// </summary>
public static class WaypointMapParser
{
    /// <summary>
    /// Parses a table in a form of an array of string to a WaypointMap.
    /// </summary>
    /// <param name="table">
    /// The table should contain three columns in order. WaypointID;ConnectionsArray (in a string form);and a DistancesArray (in a string form).
    /// The arrays should be in form: connectionIds: [2, 1, 4] and distances: [2.1, 4.2, 2.2]
    /// </param>
    /// <param name="delimiter"></param>
    /// <returns><see cref="WaypointMap"/> representation of the array of strings.</returns>
    public static WaypointMap ParseArrayOfStrings(
        List<string> table,
        string delimiter=";")
    {
        // Initialize WaypointMap
        var result = new WaypointMap();

        // Iter over all the rows in the array, where row is the string containing WaypointID;ConnectionsArray;DistancesArray.
        foreach (var row in table)
        {
            // Split the values and convert string representations of arrays to arrays
            string[] values = row.Split(delimiter);
            int waypointId = int.Parse(values[0]);
            string[] connectionIds = values[1].Substring(1, values[1].Length-2).Split(",");
            string[] distances = values[2].Substring(1, values[2].Length - 2).Split(",");

            // Initialize a WaypointNode to add
            WaypointNode tempNode = null;

            // Add the new WaypointNode if it doesn't exist.
            // Otherwise assign tempNode to existing one in the WaypointMap result.
            if (!result.WaypointIds.Contains(waypointId))
            {
                tempNode = new WaypointNode(waypointId);
                result.AddWaypoint(tempNode);
            } else
            {
                tempNode = result.Waypoints.First(x => x.Id == waypointId);
            }

            // Iterate over all the connections
            for(int i = 0; i < connectionIds.Length; i++)
            {
                WaypointNode connectionNode = null;
                // Add each connection to the connections at the specified distance.
                // If already exists, select it as the current connectionNode and add connection
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
