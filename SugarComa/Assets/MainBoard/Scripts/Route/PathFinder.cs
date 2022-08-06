using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Assets.MainBoard.Scripts.Utils.PlatformUtils;

namespace Assets.MainBoard.Scripts.Route
{
    public class PathFinder : MonoBehaviour
    {
        [SerializeField] Graph _graph;

        /// <summary>
        /// Finds and returns the best path between paths from the given source to the specifications.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="spec"></param>
        /// <param name="take"></param>
        public Platform[] FindBest(Platform source, PlatformSpec spec, int take = -1)
        {
            if (source == null || take == 0) return null;

            var paths = new List<List<Platform>>();

            var index = 0;
            var maxDepth = -1;
            // find destinations platforms by spec.
            var destinations = _graph.GetPlatformsBySpec(spec);
            for (int i = 0; i < destinations.Length; i++)
            {
                var found = FindPrivate(source, destinations[i], out List<Platform> path, maxDepth);
                if (!found) continue;

                paths.Add(path);
                var tempMaxDepth = path.Count;

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

            if (paths.Count == 0) return null;

            var bestPath = paths[index];
            if (take > 0) return bestPath.Take(take).ToArray();
            return bestPath.ToArray();
        }

        /// <summary>
        /// Finds and returns paths from the given source to the destination.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="spec"></param>
        /// <param name="take"></param>
        public List<Platform[]> GetPaths(Platform source, PlatformSpec spec, int take = -1)
        {
            if (source == null || take == 0) return null;

            var paths = new List<Platform[]>();

            var maxDepth = -1;
            // find destinations platforms by spec.
            var destinations = _graph.GetPlatformsBySpec(spec);
            for (int i = 0; i < destinations.Length; i++)
            {
                var found = FindPrivate(source, destinations[i], out List<Platform> path, maxDepth);
                if (!found) continue;

                if (take != -1) paths.Add(path.Take(take).ToArray());
                else paths.Add(path.ToArray());
            }

            return paths;
        }

        /// <summary>
        /// Returns the path from the position to the selector or the given count of steps.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="step"></param>
        public Platform[] ToSelector(Platform source, int step = -1, RouteSelectorDirection direction = RouteSelectorDirection.None)
        {
            if (source == null || step == 0) return null;

            var path = new List<Platform>();
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
        /// Find the first path from given source to a destination.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="maxDepth"></param>
        public Platform[] Find(Platform source, Platform destination, int maxDepth = -1)
        {
            if (source == null || destination == null || maxDepth == 0) return null;

            FindPrivate(source, destination, out List<Platform> path, maxDepth);
            return path.ToArray();
        }

        private bool FindPrivate(Platform source, Platform destination, out List<Platform> path, int maxDepth = -1)
        {
            path = new List<Platform>();
            if (source == null || destination == null) return false;
            var result = _graph.DepthFirstSearch(source, destination, path, maxDepth);
            // remove source
            path.Remove(source);
            return result;
        }

        public Platform ChooseGrave()
        {
            int i = Random.Range(0, 2);
            if (i == 0) return _graph.TopGrave;
            return _graph.BottomGrave;
        }
    }
}