using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AI
{
    public static class PathfindingModule
    {
        public static Vector3Int[] StandardNeighboringPositions;

        static PathfindingModule()
        {
            StandardNeighboringPositions = new[] {
                new Vector3Int(1, 0, 0),
                new Vector3Int(-1, 0, 0),
                new Vector3Int(0, 0, 1),
                new Vector3Int(0, 0, -1),
                new Vector3Int(1, 0, 1),
                new Vector3Int(1, 0, -1),
                new Vector3Int(-1, 0, 1),
                new Vector3Int(-1, 0, -1)
            };
        }

        private static IEnumerable<Vector3Int> GetNeighboringPositions(this Vector3Int position,
            List<Vector3Int> blockedPositions)
        {
            var neighboringPositions = new List<Vector3Int>();
            
            foreach (var neighborOffset in StandardNeighboringPositions)
            {
                var neighborPosition = position + neighborOffset;
                if (blockedPositions.Contains(neighborPosition)) continue;
                neighboringPositions.Add(neighborPosition);
            }

            return neighboringPositions;
        }
            

        private static float GetCost(this Vector3Int a, Vector3Int b) =>
            Mathf.Pow(b.x - a.x, 2f) + Mathf.Pow(b.z - a.z, 2f);

        private static List<Vector3Int> ReconstructPath(IReadOnlyDictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current)
        {
            var totalPath = new List<Vector3Int> { current };
            
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                totalPath.Insert(0, current);
            }
            
            return totalPath;
        }

        public static List<Vector3Int> AStar(
            Vector3Int start,
            Vector3Int goal,
            Func<Vector3Int, Vector3Int, float> h,
            Func<Vector3Int, List<Vector3Int>> getBlockedPositionsNearPosition
        )
        {
            if (getBlockedPositionsNearPosition(goal).Contains(goal))
            {
                // Find the goal's neighbor closest to start and move there instead
                var newGoal = goal
                    .GetNeighboringPositions(getBlockedPositionsNearPosition(goal))
                    .OrderBy(position => Vector3Int.Distance(position, start))
                    .ThenBy(position => Vector3Int.Distance(position, goal))
                    .First();
                
                return AStar(start, newGoal, h, getBlockedPositionsNearPosition);
            }
            
            if (start.GetNeighboringPositions(getBlockedPositionsNearPosition(goal)).Contains(goal))
            {
                return new List<Vector3Int> { goal };
            }
            
            var openSet = new HashSet<Vector3Int> { start };
            
            var cameFrom = new Dictionary<Vector3Int, Vector3Int>();
            
            var gScore = new Dictionary<Vector3Int, float>
            {
                {start, 0f}
            };

            var fScore = new Dictionary<Vector3Int, float>
            {
                {start, h(start, goal)}
            };

            while (openSet.Count > 0)
            {
                var current = openSet.OrderBy(n => fScore[n]).First();
                
                if (current == goal)
                {
                    return ReconstructPath(cameFrom, current);
                }

                openSet.Remove(current);
                
                var blockedPositions = getBlockedPositionsNearPosition(current);
                
                var openNeighboringPositions = current.GetNeighboringPositions(blockedPositions); 
                
                foreach (var neighbor in openNeighboringPositions)
                {
                    var tentativeGScore = gScore[current] + current.GetCost(neighbor);
                    
                    var hasGScore = gScore.ContainsKey(neighbor);
                    
                    var gScoreIsNotLess = hasGScore && !(tentativeGScore < gScore[neighbor]);
                    
                    if (gScoreIsNotLess) continue;
                    
                    if (!cameFrom.TryAdd(neighbor, current))
                    {
                        cameFrom[neighbor] = current;    
                    }
                    
                    if (!gScore.TryAdd(neighbor, tentativeGScore))
                    {
                        gScore[neighbor] = tentativeGScore;
                    }
                    
                    if (!fScore.TryAdd(neighbor, tentativeGScore + h(neighbor, goal)))
                    {
                        fScore[neighbor] = tentativeGScore + h(neighbor, goal);
                    }
                    
                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }

            return null; // failure
        }
    }
}