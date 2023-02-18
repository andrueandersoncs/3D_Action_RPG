using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class Pathfinder : MonoBehaviour
    {
        private static LayerMask obstacleLayer;

        public float speed;
        public Vector3 velocity;

        private readonly Collider[] _obstacleColliders = new Collider[25];
        private readonly RaycastHit[] _spherecastResults = new RaycastHit[25];
        [SerializeField] private Vector3 _target;
        [SerializeField] private Vector3Int _node;
        [SerializeField] private Vector3 _destination;
        
        private const float spherecastRadius = 0.5f;
        private const float spherecastDistance = 1f;
        
        private void OnEnable()
        {
            obstacleLayer = LayerMask.GetMask("PathfindingObstacle");
        }

        private List<Vector3Int> GetBlockedPositionsNearPositionSpherecast(Vector3Int position)
        {
            var obstacles = new List<Vector3Int>();
            var colliders = new List<Collider>();

            // IMPORTANT: don't forget to check if the position itself is blocked with this method
            // spherecast doesn't work if the origin is inside the collider
            foreach (var neighborDirection in PathfindingModule.StandardNeighboringPositions)
            {
                var numObstacles = Physics.SphereCastNonAlloc(
                    position + neighborDirection,
                    spherecastRadius,
                    -neighborDirection,
                    _spherecastResults,
                    spherecastDistance,
                    obstacleLayer
                );
                var selfBlocked = false;
                for (var o = 0; o < numObstacles; o++)
                {
                    var result = _spherecastResults[o];
                    if (result.collider.transform.IsChildOf(transform) || result.collider.transform == transform) continue;
                    selfBlocked = true;
                    colliders.Add(result.collider);
                }
                if (selfBlocked) obstacles.Add(position);
            }
            
            foreach (var neighborDirection in PathfindingModule.StandardNeighboringPositions)
            {
                var numObstacles = Physics.SphereCastNonAlloc(
                    position,
                    spherecastRadius,
                    neighborDirection,
                    _spherecastResults,
                    spherecastDistance,
                    obstacleLayer
                );
                for (var o = 0; o < numObstacles; o++)
                {
                    var result = _spherecastResults[o];
                    if (result.collider.transform.IsChildOf(transform) || result.collider.transform == transform) continue;
                    obstacles.Add(position + neighborDirection);
                    colliders.Add(result.collider);
                }
            }

            return obstacles;
        }
        
        // private List<Vector3Int> GetBlockedPositionsNearPosition(Vector3Int position)
        // {
        //     var numObstacles = Physics.OverlapSphereNonAlloc(position, 5f, _obstacleColliders, obstacleLayer);
        //     
        //     var obstacles = new List<Vector3Int>();
        //             
        //     for (var i = 0; i < numObstacles; i++)
        //     {
        //         var collider = _obstacleColliders[i];
        //
        //         // The collider in question is actually on the child, not this object..
        //         if (collider.transform.IsChildOf(transform) || collider.transform == transform) continue;
        //
        //         // Find all positions that collide with this collider, iterate by 1f outward from the collider position
        //         var bounds = collider.bounds;
        //         var min = bounds.min;
        //         var max = bounds.max;
        //         for (var x = min.x; x <= max.x; x++)
        //         {
        //             for (var z = min.z; z <= max.z; z++)
        //             {
        //                 var roundedX = Mathf.RoundToInt(x);
        //                 var roundedZ = Mathf.RoundToInt(z);
        //                 var boundsPosition = new Vector3Int(roundedX, 0, roundedZ);
        //                 obstacles.Add(boundsPosition);
        //             }
        //         }
        //     }
        //
        //     return obstacles;
        // }

        public List<Vector3Int> SetDestination(Vector3 destination)
        {
            _destination = destination;
            
            var transformPosition = new Vector3Int(
                Mathf.RoundToInt(transform.position.x),
                0,
                Mathf.RoundToInt(transform.position.z)
            );
            
            var destinationPosition = new Vector3Int(
                Mathf.RoundToInt(destination.x),
                0,
                Mathf.RoundToInt(destination.z)
            );

            var path = PathfindingModule.AStar(
                transformPosition,
                destinationPosition,
                (a, b) => Mathf.Pow(b.x - a.x, 2f) + Mathf.Pow(b.z - a.z, 2f),
                GetBlockedPositionsNearPositionSpherecast
            );

            if (path.Contains(transformPosition))
            {
                path.Remove(transformPosition);
            }
            
            return path;
        }

        public IEnumerator WalkPath(List<Vector3Int> path)
        {
            while (path.Count > 0)
            {
                var node = path[0];
                _node = node;
                path.RemoveAt(0);
                
                while (Vector3.Distance(transform.position, node) > 0.1f)
                {
                    // If the node becomes blocked, recalculate the path
                    var blockedPositions = GetBlockedPositionsNearPositionSpherecast(node);
                    if (blockedPositions.Contains(node))
                    {
                        path = SetDestination(_destination);
                        break;
                    }
                    
                    // Otherwise, move to the next node
                    var transformPosition = transform.position;
                    var target = new Vector3(node.x, transformPosition.y, node.z);
                    _target = target;
                    
                    // Should rotation be handled here?
                    transform.LookAt(target, Vector3Int.up);
                    
                    // Should movement be handled here?
                    var nextPosition = Vector3.MoveTowards(transformPosition, target, speed * Time.deltaTime);
                    transform.position = nextPosition;
                    
                    // Should velocity calculation be handled here?
                    velocity = (nextPosition - transformPosition) / Time.deltaTime;
                    yield return null;
                }
            }
            
            velocity = Vector3.zero;
        }
    }
}