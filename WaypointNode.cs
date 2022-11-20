namespace WaypointPathfinding;

/// <summary>
/// Class that implements a custom node data structure. 
/// Used in <see cref="WaypointPathfinding.WaypointMap"/> as a node of a WaypointMap.
/// </summary>
public class WaypointNode
{
    /// <summary>
    /// Private variable for <see cref="ConnectionsAndDistances"/>.
    /// </summary>
    private IDictionary<WaypointNode, float> _connectionsAndDistances;
    /// <summary>
    /// Private variable for <see cref="Id"/>.
    /// </summary>
    private int _id;

    // ------ getters and setters ------

    /// <summary>
    /// Getter for a dictionary containing <see cref="Connections"/> and <see cref="Distances"/>.
    /// Keys are <see cref="WaypointNode"/>s and values are distances to them.
    /// </summary>
    public IDictionary<WaypointNode, float> ConnectionsAndDistances { get { return _connectionsAndDistances; } }
    /// <summary>
    /// Getter for the ordered list containing all the connections from the parent node.
    /// </summary>
    public List<WaypointNode> Connections { get { return _connectionsAndDistances.Keys.ToList(); } }
    /// <summary>
    /// Getter for the ordered list containing all the distances ti the parent node's <see cref="Connections"/>.
    /// </summary>
    public List<float> Distances { get { return _connectionsAndDistances.Values.ToList(); } }

    /// <summary>
    /// Getter and setter for the parent <see cref="WaypointNode"/>'s ID.
    /// It is suggested not to overwrite it when using <see cref="WaypointMap"/>, as it does not throw an exception when the ID already exists.
    /// </summary>
    public int Id { get { return _id; } set { _id = value; } }

    // ---------------------------------

    /// <summary>
    /// Constructor for <see cref="WaypointPathfinding.WaypointNode"/>.
    /// </summary>
    /// <param name="id">ID of a new waypoint. Must be an int type</param>
    public WaypointNode(int id)
    {
        _connectionsAndDistances = new Dictionary<WaypointNode, float>();
        _id = id;
    }

    /// <summary>
    /// Adds a new, one-way connection between the parent <see cref="WaypointNode"/> and the new <paramref name="node"/> at a specified <paramref name="distance"/>.
    /// </summary>
    /// <param name="node">The new <see cref="WaypointNode"/> to add as a connection.</param>
    /// <param name="distance">Distance between the parent node and the new <paramref name="node"/></param>
    public void AddConnection(WaypointNode node, float distance)
    {
        _connectionsAndDistances.Add(node, distance);
    }

    /// <summary>
    /// Removes a one-way connection between the parent <see cref="WaypointNode"/> and another <see cref="WaypointNode"/> with <paramref name="id"/>.
    /// Note, that this method does not remove a connection from a child node to a parent. Only from a parent to a child.
    /// </summary>
    /// <param name="id">ID of a <see cref="WaypointNode"/> to be removed from connection.</param>
    public void RemoveConnection(int id)
    {
        _connectionsAndDistances.Remove(
            _connectionsAndDistances.FirstOrDefault(node => node.Key.Id == id)
            );
    }
}
