using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour
{
    [HideInInspector] private Graph _graph;

    private void Awake()
    {
        _graph = GetComponent<Graph>();
    }

    /// <summary>
    /// Finds and returns the best path between paths from the given source to the specifications.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="spec"></param>
    public Platform[] FindBest(Platform source, PlatformSpecification spec)
    {
        return FindBest(source, spec, -1);
    }

    /// <summary>
    /// Finds and returns the best path between paths from the given source to the specifications.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="spec"></param>
    /// <param name="maxDepth"></param>
    public Platform[] FindBest(Platform source, PlatformSpecification spec, int maxDepth)
    {
        if (source == null) return null;

        var take = maxDepth;
        var pathes = new List<Platform[]>();

        var index = 0;
        maxDepth = -1;
        var destinations = _graph.GetPlatformsBySpec(spec);
        for (int i = 0; i < destinations.Length; i++)
        {
            var tuple = FindPrivate(source, destinations[i], maxDepth);
            var path = tuple.Item1;
            int tempMaxDepth = tuple.Item2;

            if (tempMaxDepth == -1) continue;

            pathes.Add(path);
            if (maxDepth == -1)
            {
                index = i;
                maxDepth = tempMaxDepth;
            }
            else if (tempMaxDepth < maxDepth)
            {
                index = i;
                maxDepth = tempMaxDepth;
            }
        }

        if (pathes.Count == 0) return null;

        var bestPath = pathes[index].ToList();
        if (take > 0) return Take(bestPath, take);
        return bestPath.ToArray();
    }

    /// <summary>
    /// Returns the path from the position to the selector or the given count of steps.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="step"></param>
    public Platform[] ToSelector(Platform source, int step = -1, RouteSelectorDirection direction = RouteSelectorDirection.None)
    {
        var path = new List<Platform>() { source };
        Platform next = source;
        do
        {
            var platforms = _graph.NextPlatforms(next);

            if (direction != RouteSelectorDirection.None && next.HasSelector)
            {
                switch (direction)
                {
                    case RouteSelectorDirection.Left: next = platforms[0]; break;
                    case RouteSelectorDirection.Right: next = platforms[1]; break;
                }
                direction = RouteSelectorDirection.None;
            }
            else if (platforms.Length > 0) next = platforms[0];
            else break;

            if (path.Contains(next)) break;


            path.Add(next);
            step--;
        }
        while (next != null && (step >= 1 || step < 0) && !next.HasSelector);

        return path.ToArray();
    }

    /// <summary>
    /// Find path from given source to a destination.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    /// <param name="maxDepth"></param>
    public Platform[] Find(Platform source, Platform destination, int maxDepth = -1)
    {
        var tuple = FindPrivate(source, destination, maxDepth);
        return tuple.Item1;
    }

    private Platform[] Take(List<Platform> path, int take)
    {
        take = Mathf.Min(path.Count, take);
        var platforms = new Platform[take];
        take = path.Count - take;
        take = Mathf.Max(take, 0);

        int index = platforms.Length - 1;
        for (int i = path.Count - 1; i >= take; --i)
        {
            platforms[index] = path[i];
            index--;
        }
        return platforms;
    }

    private (Platform[], int) FindPrivate(Platform source, Platform destination, int maxDepth = -1)
    {
        if (source == null || destination == null) return (null, -1);

        var path = new List<Platform>();
        var depth = _graph.DepthFirstSearch(source, destination, path, maxDepth);
        // _graph.ResetVisited();
        return (path.ToArray(), depth);
    }
}