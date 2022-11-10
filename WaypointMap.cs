using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pathfinding;

namespace Pathfinding;

class WaypointMap
{
    private List<WaypointNode> _waypoints;
    private List<int> _waypointIds;

    // ------ getters and setters ------

    public List<WaypointNode> Waypoints { get { return _waypoints; } }
    public List<int> WaypointIds { get { return _waypointIds; } }

    // ---------------------------------

    public WaypointMap()
    {
        _waypoints = new List<WaypointNode>();
        _waypointIds = new List<int>();
    }

    public void AddWaypoint(WaypointNode node)
    {
        // Make sure there is no duplicate of ID
        if (_waypointIds.Contains(node.Id))
            throw new Exception("Current waypoint ID is already on the map");

        _waypoints.Add(node);
        _waypointIds.Add(node.Id);
    }

    public void RemoveWaypoint(WaypointNode node)
    {
        // Remove node from all the nodes' connections
        foreach (WaypointNode tempNode in _waypoints)
        {
            // If there is a node with an Id of a to-be-removed node
            if (tempNode.Connections.Any(node => node.Id == node.Id))
                tempNode.RemoveConnection(node.Id);
        }

        // Remove from the waypoints list
        _waypoints.Remove(node);
    }

    public void AddConnectionBetweenTwoNodes(WaypointNode node1, WaypointNode node2, float distance)
    {
        if (!(_waypoints.Contains(node1) && _waypoints.Contains(node2)))
            throw new Exception("Nodes are not on the map");

        if (node1.Connections.Contains(node2) && node2.Connections.Contains(node1))
            return;

        node1.AddConnection(node2, distance);
        node2.AddConnection(node1, distance);
    }

    public float[,] GetAdjacencyMatrix()
    {
        float[,] result = new float[_waypoints.Count, _waypoints.Count];
        for(int i = 0; i< _waypoints.Count; i++)
        {
            for (int j = 0; j < _waypoints.Count; j++)
            {
                result[i, j] = 0;
            }
        }

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
