using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class Pathfinder : MonoBehaviour
    {
        private static LayerMask _obstacleLayer;

        public float speed;
        public Vector3 velocity;

        private readonly Collider[] _obstacleColliders = new Collider[25];
        private readonly RaycastHit[] _spherecastResults = new RaycastHit[25];
        [SerializeField] private Vector3 _target;
        [SerializeField] private Vector3Int _node;
        [SerializeField] private Vector3 _destination;
        
        private const float SpherecastRadius = 0.25f;
        private const float SpherecastDistance = 0.5f;
        
        private void OnEnable()
        {
            _obstacleLayer = LayerMask.GetMask("PathfindingObstacle");
        }

        private List<Vector3Int> GetBlockedNeighborsFromPosition(Vector3Int position)
        {
            var obstacles = new List<Vector3Int>();
            var colliders = new List<Collider>();

            // Sphere-casting from position to neighbors to check if they're blocked
            foreach (var neighborDirection in PathfindingModule.StandardNeighboringPositions)
            {
                var numObstacles = Physics.SphereCastNonAlloc(
                    position,
                    SpherecastRadius,
                    Vector3.Normalize(neighborDirection),
                    _spherecastResults,
                    SpherecastDistance,
                    _obstacleLayer
                );
                for (var o = 0; o < numObstacles; o++)
                {
                    var result = _spherecastResults[o];
                    
                    if (
                        result.collider.transform.IsChildOf(transform)
                        || result.collider.transform == transform
                    ) continue;
                    
                    obstacles.Add(position + neighborDirection);
                    colliders.Add(result.collider);
                    
                    // If the neighbor is not blocked, check if the position itself is blocked
                    var numSelfObstacles = Physics.SphereCastNonAlloc(
                        position + neighborDirection,
                        SpherecastRadius,
                        -Vector3.Normalize(neighborDirection),
                        _spherecastResults,
                        SpherecastDistance,
                        _obstacleLayer
                    );
                    for (var selfCheckIterator = 0; selfCheckIterator < numSelfObstacles; selfCheckIterator++)
                    {
                        var selfCheckResult = _spherecastResults[selfCheckIterator];
                        
                        if (
                            selfCheckResult.collider.transform.IsChildOf(transform)
                            || selfCheckResult.collider.transform == transform
                        ) continue;
                        
                        obstacles.Add(position);
                        colliders.Add(selfCheckResult.collider);
                    }
                }
            }

            return obstacles;
        }

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
                GetBlockedNeighborsFromPosition
            );

            if (path != null && path.Contains(transformPosition))
            {
                path.Remove(transformPosition);
            }
            
            return path;
        }

        public IEnumerator WalkPath(List<Vector3Int> path)
        {
            if (path == null) yield break;
            
            while (path.Count > 0)
            {
                var node = path[0];
                _node = node;
                path.RemoveAt(0);
                
                while (Vector3.Distance(transform.position, node) > 0.1f)
                {
                    var transformPosition = transform.position;
                    
                    // If the node becomes blocked, recalculate the path
                    var numObstacles = Physics.SphereCastNonAlloc(
                        transformPosition,
                        SpherecastRadius,
                        Vector3.Normalize(node - transformPosition),
                        _spherecastResults,
                        SpherecastDistance,
                        _obstacleLayer
                    );
                    for (var o = 0; o < numObstacles; o++)
                    {
                        var result = _spherecastResults[o];
                        if (result.collider.transform.IsChildOf(transform) || result.collider.transform == transform) continue;
                        path = SetDestination(_destination);
                    }
                    
                    // Otherwise, move toward the node
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