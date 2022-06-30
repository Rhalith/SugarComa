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

        platforms.Add(destination);
    }

    /// <summary>
    /// Deletes the target from source.
    /// </summary>
    public void Remove(Platform source, Platform destination)
    {
        if (!_platforms.TryGetValue(source, out List<Platform> platforms))
            return;

        platforms.Remove(destination);

        if (platforms.Count == 0)
            _platforms.Remove(source);
    }

    #region Thread Safe

    /// <summary>
    /// Adds edge to the source as thread safe.
    /// </summary>
    public void AddEdgeThreadSafe(Platform source, Platform destination)
    {
        lock (_platforms)
        {
            if (!_platforms.TryGetValue(source, out List<Platform> platforms))
            {
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
    public void RemoveThreadSafe(Platform source, Platform destination)
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
    public Platform[] GetPlatformsBySpec(PlatformSpec spec)
    {
        var platforms = new List<Platform>();

        var keys = _platforms.Keys.ToArray();
        for (int i = 0; i < keys.Length; ++i)
        {
            var key = keys[i];
            if (key.specification == spec && !platforms.Contains(key)) platforms.Add(key);
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
    public bool DepthFirstSearch(Platform source, Platform destination, List<Platform> path, int maxStep = -1)
    {
        // stop searching and return true when source is equal to destination.
        if (source.Equals(destination))
        {
            path.Add(destination);
            return true;
        }

        int step = path.Count;
        // stop the searching, if the step greater than maximum step.
        // return false
        if (maxStep != -1 && step >= maxStep) return false;

        if (_platforms.TryGetValue(source, out List<Platform> platforms))
        {
            int count = platforms.Count;
            // if the source platform has already been added, there is no path to the destination platform.
            // return false
            if (path.Contains(source)) return false;

            path.Add(source);
            
            if (count == 1)
            {
                return DepthFirstSearch(platforms[0], destination, path, maxStep);
            }
            else
            {
                var alternativePaths = new List<Platform>[count];
                for (int i = 0; i < count; ++i)
                {
                    // create alternative path
                    var alternative = new List<Platform>();
                    alternative.AddRange(path);

                    bool found = DepthFirstSearch(platforms[i], destination, alternative, maxStep);

                    // if the destination is found.
                    if (found)
                    {
                        alternativePaths[i] = alternative;
                        maxStep = alternative.Count;
                    }
                }
                // find best path.
                var index = BestPathIndex(alternativePaths);
                if (index == -1) return false;

                // add alternative path to path.
                path.Clear();
                path.AddRange(alternativePaths[index]);

                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Show path.
    /// </summary>
    public void OnDrawGizmosSelected()
    {
        foreach (var platform in _platforms)
        {
            var key = platform.Key;
            var value = platform.Value;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(key.position, 0.05f);
            Gizmos.color = Color.green;
            for (int i = 0; i < value.Count; ++i)
            {
                Gizmos.DrawLine(key.position, value[i].position);
            }
        }
    }

    private int BestPathIndex(List<Platform>[] platforms)
    {
        if (platforms == null || platforms.Length == 0) return -1;

        int index = -1;
        int bestValue = int.MaxValue;
        for (int i = 0; i < platforms.Length; i++)
        {
            var plats = platforms[i];
            if (plats == null) continue;

            var value = plats.Count;
            if (value < bestValue)
            {
                index = i;
                bestValue = value;
            }
        }
        return index;
    }

    [SerializeField] private PlatformDictionary _platforms;
}
