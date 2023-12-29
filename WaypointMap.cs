namespace WaypointPathfinding;

/// <summary>
/// Exception if ID exists in the <see cref="WaypointMap"/>.
/// </summary>
public class WaypointIdExistsException : Exception
{
    public override string Message
    {
        get
        {
            return "Current waypoint ID is already in the map";
        }
    }

    public WaypointIdExistsException()
    {
        Console.WriteLine(Message);
    }
}
/// <summary>
/// Exception if a WaypointNode does not exist in the <see cref="WaypointMap"/>.
/// </summary>
public class WaypointNodeDoesntExist : Exception
{
    public override string Message
    {
        get
        {
            return "WaypointNode does not exist in the WaypointMap.";
        }
    }

    public WaypointNodeDoesntExist()
    {
        Console.WriteLine(Message);
    }
}

/// <summary>
/// Class that implements a custom graph data structure. 
/// A node is a <see cref="WaypointPathfinding.WaypointNode"/>.
/// </summary>
public class WaypointMap
{
    /// <summary>
    /// Private variable for <see cref="Waypoints"/>.
    /// </summary>
    private List<WaypointNode> _waypoints;
    /// <summary>
    /// Private variable for <see cref="WaypointIds"/>.
    /// </summary>
    private List<int> _waypointIds;

    // ------ getters and setters ------

    /// <summary>
    /// Getter for a list containing all the <see cref="WaypointNode"/>s inside the parent <see cref="WaypointMap"/>.
    /// </summary>
    public List<WaypointNode> Waypoints { get { return _waypoints; } }
    /// <summary>
    /// Getter for a list containing all the IDs of <see cref="WaypointNode"/>s inside the parent <see cref="WaypointMap"/>.
    /// </summary>
    public List<int> WaypointIds { get { return _waypointIds; } }

    // ---------------------------------

    /// <summary>
    /// Constructor for <see cref="WaypointMap"/>.
    /// </summary>
    public WaypointMap()
    {
        _waypoints = new List<WaypointNode>();
        _waypointIds = new List<int>();
    }

    /// <summary>
    /// Adds a new <see cref="WaypointNode"/> to the parent.
    /// </summary>
    /// <param name="node">New <see cref="WaypointNode"/> to add to the parent.</param>
    /// <exception cref="WaypointIdExistsException">Throws an exception when a <see cref="WaypointNode"/> with the specified ID already exists.</exception>
    public void AddWaypoint(WaypointNode node)
    {
        // Make sure there is no duplicate of ID
        if (_waypointIds.Contains(node.Id))
            throw new WaypointIdExistsException();

        _waypoints.Add(node);
        _waypointIds.Add(node.Id);
    }

    /// <summary>
    /// Removes a <see cref="WaypointNode"/> from the parent and removes connections to it from all others.
    /// Does nothing if <see cref="WaypointNode"/> is not found.
    /// </summary>
    /// <param name="node"><see cref="WaypointNode"/> to remove from the parent.</param>
    public void RemoveWaypoint(WaypointNode node)
    {
        // Remove node from all the nodes' connections
        foreach (WaypointNode tempNode in _waypoints)
        {
            // If there is a node with an Id of a to-be-removed node
            if (tempNode.Connections.Any(node => node.Id == node.Id))
                tempNode.RemoveConnection(node.Id);
        }

        // Remove from the waypoints list if not null
        if (node != null)
            _waypoints.Remove(node);
    }

    /// <summary>
    /// Adds a two-way connection between <paramref name="node1"/> and <paramref name="node2"/> at the specified <paramref name="distance"/>.
    /// Does nothing if already connected.
    /// If a one-way connection exists, overwrites it to a two-way one.
    /// </summary>
    /// <param name="node1">First <see cref="WaypointNode"/>.</param>
    /// <param name="node2">Second <see cref="WaypointNode"/>.</param>
    /// <param name="distance">Distance between the two <see cref="WaypointNode"/>s.</param>
    /// <exception cref="WaypointNodeDoesntExist">Throws an exception when either <paramref name="node1"/> or <paramref name="node2"/> doesn't exist.</exception>
    public void AddConnectionBetweenTwoNodes(WaypointNode node1, WaypointNode node2, float distance)
    {
        // Check if exist in the parent
        if (!(_waypoints.Contains(node1) && _waypoints.Contains(node2)))
            throw new WaypointNodeDoesntExist();

        // Add connection if does not contain
        if (!node1.Connections.Contains(node2))
            node1.AddConnection(node2, distance);
        if (!node2.Connections.Contains(node1))
            node2.AddConnection(node1, distance);
    }

    /// <summary>
    /// Represent the parent as a adjacency matrix.
    /// </summary>
    /// <returns>An adjacency matrix representation of the parent. If IDs are not coninuous (e.g.: 1,3,4,5), fills the index 2 with 0.</returns>
    public float[,] GetAdjacencyMatrix()
    {
        // Get size of the adjacency matrix and initialize to 0s
        int lastId = _waypointIds.Max() + 1;
        float[,] result = new float[lastId, lastId];
        for(int i = 0; i < lastId; i++)
        {
            for (int j = 0; j < lastId; j++)
            {
                result[i, j] = 0;
            }
        }

        // For every node, replace the adjacency matrix input.
        foreach(WaypointNode node in _waypoints)
        {
            foreach(var connectionAndDistance in node.ConnectionsAndDistances)
            {
                result[node.Id, connectionAndDistance.Key.Id] = connectionAndDistance.Value;
            }
        }

        return result;
    }
}

