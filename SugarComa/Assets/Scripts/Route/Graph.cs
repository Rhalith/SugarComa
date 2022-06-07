using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Graph : MonoBehaviour
{
    /// <summary>
    /// Adds edge to the source. 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    public void AddEdge(Platform source, Platform destination)
    {
        // kaynak kenar olarak daha önceden eklenmemiş ise 
        if (!_platforms.TryGetValue(source, out List<Platform> platforms))
        {
            platforms = new List<Platform>();
            _platforms.Add(source, platforms);
        }

        if (!_visited.ContainsKey(source))
            _visited.Add(source, false);

        if (!_visited.ContainsKey(destination))
            _visited.Add(destination, false);

        platforms.Add(destination);
    }

    /// <summary>
    /// Deletes the target from source.
    /// </summary>
    public void Remove(Platform source, Platform destination)
    {
        if (!_platforms.TryGetValue(source, out List<Platform> platforms))
            return;

        _visited.Remove(source);
        platforms.Remove(destination);

        if (platforms.Count == 0)
            _platforms.Remove(source);
    }

    #region Thread Safe

    /// <summary>
    /// Adds edge to the source as thread safe.
    /// </summary>
    public void AddEdgeSave(Platform source, Platform destination)
    {
        lock (_platforms)
        {
            if (!_platforms.TryGetValue(source, out List<Platform> platforms))
            {
                lock (_visited)
                {
                    _visited.Add(source, false);
                }

                platforms = new List<Platform>();
                _platforms.Add(source, platforms);
            }

            lock (platforms)
            {
                platforms.Add(destination);
            }
        }
    }

    /// <summary>
    /// Deletes the target from source as thread safe.
    /// </summary>
    public void RemoveSave(Platform source, Platform destination)
    {
        lock (_platforms)
        {
            if (!_platforms.TryGetValue(source, out List<Platform> platforms))
                return;

            lock (platforms)
            {
                platforms.Remove(destination);

                if (platforms.Count == 0)
                    _platforms.Remove(source);
            }
        }
    }
    #endregion

    /// <summary>
    /// Find platforms by the give specification.
    /// </summary>
    /// <param name="spec"></param>
    public Platform[] GetPlatformsBySpec(PlatformSpecification spec)
    {
        var platforms = new List<Platform>();
        foreach (var el in _platforms)
        {
            var key = el.Key;
            var values = el.Value;

            if (key.specification == spec && !platforms.Contains(key)) platforms.Add(key);

            for (int i = 0; i < values.Count; i++)
            {
                var value = values[i];
                if (value.specification == spec && !platforms.Contains(value)) platforms.Add(value);
            }
        }
        return platforms.ToArray();
    }

    /// <summary>
    /// Returns platforms connected to the source.
    /// </summary>
    /// <param name="source"></param>
    public Platform[] NextPlatforms(Platform source)
    {
        if (!_platforms.TryGetValue(source, out List<Platform> platforms))
            return null;
        return platforms.ToArray();
    }

    /// <summary>
    /// Find path from the given source to the destination. If it find returns step count otherwise return -1.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    /// <param name="path">The path to the destination.</param>
    /// <param name="maxStep"></param>
    /// <param name="step"></param>
    public int DepthFirstSearch(Platform source, Platform destination, List<Platform> path, int maxStep = -1, int step = 0)
    {
        if (_platforms.TryGetValue(source, out List<Platform> platforms))
        {
            if (platforms == null)
            {
                ResetVisited();
                return -1;
            }

            _visited[source] = true;

            if (source.Equals(destination))
            {
                ResetVisited();
                return step;
            }

            if (maxStep != -1 && step >= maxStep)
            {
                ResetVisited();
                return maxStep + 1;
            }

            int count = platforms.Count;
            int[] steps = new int[count];
            var tempPaths = new List<Platform>[count];
            for (int i = 0; i < count; ++i)
            {
                var key = platforms[i];

                if (!_visited.TryGetValue(key, out bool visited))
                {
                    visited = false;
                    _visited[key] = visited;
                }

                var tempStep = step;
                var tempPath = new List<Platform>();
                if (!visited)
                {
                    tempPaths[i] = tempPath;
                    tempStep = DepthFirstSearch(key, destination, tempPath, maxStep, ++tempStep);

                    // finded destination.
                    if (tempStep != -1)
                    {
                        if (maxStep == -1)
                        {
                            tempPath.Add(key);
                            maxStep = tempStep;
                        }
                        else if (tempStep < maxStep)
                        {
                            tempPath.Add(key);
                            maxStep = tempStep;
                        }
                    }
                }
                else tempStep = -1;
                steps[i] = tempStep;
            }

            var minIndex = MinIndex(steps);
            if (minIndex == -1)
            {
                ResetVisited();
                return -1;
            }

            if (minIndex < tempPaths.Length && tempPaths[minIndex] != null) path.AddRange(tempPaths[minIndex]);
            return steps[minIndex];
        }
        ResetVisited();
        return -1;
    }

    /// <summary>
    /// All visited values are set to false.
    /// </summary>
    public void ResetVisited()
    {
        var keys = _visited.Keys.ToArray();
        for (int i = 0; i < keys.Length; ++i) _visited[keys[i]] = false;
    }

    private int MinIndex(int[] arr)
    {
        if (arr == null || arr.Length == 0) return -1;

        if (arr.Length == 1) return 0;

        int index = 0;
        int bestValue = int.MaxValue;
        for (int i = 0; i < arr.Length; i++)
        {
            var value = arr[i];
            if (value != -1 && value < bestValue)
            {
                index = i;
                bestValue = value;
            }
        }
        return index;
    }

    [SerializeField] private PlatformDictionary _platforms;
    [System.NonSerialized] private readonly Dictionary<Platform, bool> _visited = new();
}
