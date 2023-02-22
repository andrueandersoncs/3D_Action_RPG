using System.Collections.Generic;
using Movement;
using UnityEngine;

namespace AI
{
    public class Pathfinder : MonoBehaviour
    {
        private static LayerMask _obstacleLayer;

        private readonly Collider[] _obstacleColliders = new Collider[25];
        private const float SpherecastRadius = 0.25f;
        
        private void OnEnable()
        {
            _obstacleLayer = LayerMask.GetMask("PathfindingObstacle");
        }

        public bool IsPositionBlocked(Vector3Int position)
        {
            var numCurrentPositionObstacles = Physics.OverlapSphereNonAlloc(
                position,
                SpherecastRadius,
                _obstacleColliders,
                _obstacleLayer
            );
            if (numCurrentPositionObstacles <= 0) return false;
            var collider = _obstacleColliders[0];
            return !collider.transform.HasParent(transform);
        }

        private List<Vector3Int> GetBlockedNeighborsFromPosition(Vector3Int position)
        {
            var obstacles = new List<Vector3Int>();
            
            // Make sure to check the current position otherwise
            // we end up looking for a path to a location that can't be reached
            if (IsPositionBlocked(position))
            {
                obstacles.Add(position);
            }
            
            // This is the actual check for the neighboring positions
            foreach (var neighborDirection in PathfindingModule.StandardNeighboringPositions)
            {
                var neighborPosition = position + neighborDirection;
                if (IsPositionBlocked(neighborPosition))
                {
                    obstacles.Add(neighborPosition);
                }
            }

            return obstacles;
        }

        public List<Vector3Int> FindPath(Vector3 start, Vector3 destination)
        {
            var roundStart = start.RoundToPosition();
            
            var path = PathfindingModule.AStar(
                roundStart,
                destination.RoundToPosition(),
                GetBlockedNeighborsFromPosition
            );

            if (path != null && path.Contains(roundStart))
            {
                path.Remove(roundStart);
            }
            
            return path;
        }
    }
}