namespace Pathfinding;

class DijkstraPathfinder
{
    private WaypointMap _map;
    int _v;
    float[,] _graph;

    private static readonly int NO_PARENT = -1;


    public DijkstraPathfinder(WaypointMap map)
	{
        _map = map;
        _v = _map.Waypoints.Count;
        _graph = _map.GetAdjacencyMatrix();
    }

    // Function that implements Dijkstra's
    // single source shortest path
    // algorithm for a graph represented
    // using adjacency matrix
    // representation
    public List<WaypointNode> GetPath(WaypointNode startNode, WaypointNode endNode)
    {
        int startVertex = startNode.Id;

        int nVertices = _graph.GetLength(0);

        // shortestDistances[i] will hold the
        // shortest distance from src to i
        float[] shortestDistances = new float[nVertices];

        // added[i] will true if vertex i is
        // included / in shortest path tree
        // or shortest distance from src to
        // i is finalized
        bool[] added = new bool[nVertices];

        // Initialize all distances as
        // INFINITE and added[] as false
        for (int vertexIndex = 0; vertexIndex < nVertices;
                                            vertexIndex++)
        {
            shortestDistances[vertexIndex] = int.MaxValue;
            added[vertexIndex] = false;
        }

        // Distance of source vertex from
        // itself is always 0
        shortestDistances[startVertex] = 0;

        // Parent array to store shortest
        // path tree
        int[] parents = new int[nVertices];

        // The starting vertex does not
        // have a parent
        parents[startVertex] = NO_PARENT;

        // Find shortest path for all
        // vertices
        for (int i = 1; i < nVertices; i++)
        {

            // Pick the minimum distance vertex
            // from the set of vertices not yet
            // processed. nearestVertex is
            // always equal to startNode in
            // first iteration.
            int nearestVertex = -1;
            float shortestDistance = float.MaxValue;
            for (int vertexIndex = 0;
                    vertexIndex < nVertices;
                    vertexIndex++)
            {
                if (!added[vertexIndex] &&
                    shortestDistances[vertexIndex] <
                    shortestDistance)
                {
                    nearestVertex = vertexIndex;
                    shortestDistance = shortestDistances[vertexIndex];
                }
            }

            // Mark the picked vertex as
            // processed
            added[nearestVertex] = true;

            // Update dist value of the
            // adjacent vertices of the
            // picked vertex.
            for (int vertexIndex = 0;
                    vertexIndex < nVertices;
                    vertexIndex++)
            {
                float edgeDistance = _graph[nearestVertex, vertexIndex];

                if (edgeDistance > 0
                    && ((shortestDistance + edgeDistance) <
                        shortestDistances[vertexIndex]))
                {
                    parents[vertexIndex] = nearestVertex;
                    shortestDistances[vertexIndex] = shortestDistance +
                                                    edgeDistance;
                }
            }
        }

        return GetWaypointPathFromParents(startNode, endNode, parents);
    }


    private List<WaypointNode> GetWaypointPathFromParents(WaypointNode startNode, WaypointNode endNode, int[] parentIds)
    {
        int currentId = endNode.Id;
        List<WaypointNode> result = new List<WaypointNode>() {  };
        
        while (parentIds[currentId] != NO_PARENT)
        {
            result.Add(_map.Waypoints.Find(x => x.Id == currentId));
            currentId = parentIds[currentId];
        }
        result.Add(startNode);
        result.Reverse();
        return result;
    }

}
