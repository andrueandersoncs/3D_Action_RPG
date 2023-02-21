using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AI
{
    public static class PathfindingModule
    {
        public static readonly Vector3Int[] StandardNeighboringPositions;

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

        private static IEnumerable<Vector3Int> GetNeighboringPositions(this Vector3Int position, ICollection<Vector3Int> blockedPositions)
        {
            return StandardNeighboringPositions
                .Select(neighborOffset => position + neighborOffset)
                .Where(neighborPosition => !blockedPositions.Contains(neighborPosition));
        }
            

        private static float GetCost(this Vector3Int a, Vector3Int b) => Mathf.Pow(b.x - a.x, 2f) + Mathf.Pow(b.z - a.z, 2f);

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
            Func<Vector3Int, List<Vector3Int>> getBlockedPositionsNearPosition
        )
        {
            if (!start.GetNeighboringPositions(getBlockedPositionsNearPosition(start)).Any())
            {
                return null;
            }
                
            while (getBlockedPositionsNearPosition(goal).Contains(goal))
            {
                goal =
                    goal
                    .GetNeighboringPositions(getBlockedPositionsNearPosition(goal))
                    .OrderBy(point => start.GetCost(point))
                    .ThenBy(point => goal.GetCost(point))
                    .First();
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
                {start, start.GetCost(goal)}
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
                    
                    if (!fScore.TryAdd(neighbor, tentativeGScore + neighbor.GetCost(goal)))
                    {
                        fScore[neighbor] = tentativeGScore + neighbor.GetCost(goal);
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