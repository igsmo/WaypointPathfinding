namespace Pathfinding;

class WaypointNode
{
    private IDictionary<WaypointNode, float> _connectionsAndDistances;
    private int _id;

    // ------ getters and setters ------

    // ConnectionsAndDistances -> dictionary of an ID of another waypoint mapping to their distance
    public IDictionary<WaypointNode, float> ConnectionsAndDistances { get { return _connectionsAndDistances; } }
    public List<WaypointNode> Connections { get { return _connectionsAndDistances.Keys.ToList(); } }
    public List<float> Distances { get { return _connectionsAndDistances.Values.ToList(); } }

    // ID
    public int Id { get { return _id; } set { _id = value; } }

    // ---------------------------------

    public WaypointNode(int id)
    {
        _connectionsAndDistances = new Dictionary<WaypointNode, float>();
        _id = id;
    }

    public void AddConnection(WaypointNode node, float distance)
    {
        _connectionsAndDistances.Add(node, distance);
    }

    public void RemoveConnection(int id)
    {
        _connectionsAndDistances.Remove(
            _connectionsAndDistances.FirstOrDefault(node => node.Key.Id == id)
            );
    }
}
