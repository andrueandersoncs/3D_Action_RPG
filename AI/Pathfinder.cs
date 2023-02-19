using System;
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
        private const float SpherecastDistance = 1f;
        
        private void OnEnable()
        {
            _obstacleLayer = LayerMask.GetMask("PathfindingObstacle");

            // var prim = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            // prim.transform.position = new Vector3(0, 200f, 0);
            
            // var prim = GameObject.CreatePrimitive(PrimitiveType.Cube);
            // prim.transform.position = new Vector3(0, 200f, 0);
        }

        private void FixedUpdate()
        {
            // Sphere, Yes
            // var results = Physics.SphereCastAll(new Vector3(0, 200f, 0), 0.01f, Vector3.up, 0.02f);
            
            // Sphere, Yes
            // var results = Physics.SphereCastAll(new Vector3(0, 200f, 0), 10f, Vector3.up, 0f);
            
            // Sphere, No
            // var results = Physics.RaycastAll(new Vector3(0, 200f, 0), Vector3.up, 10f);
            
            // Sphere, Yes
            // var results = Physics.RaycastAll(new Vector3(0, 198f, 0), Vector3.up, 10f);
            
            // Cube, Yes
            // var results = Physics.SphereCastAll(new Vector3(0, 200f, 0), 0.01f, Vector3.up, 0.02f);
            
            // Cube, No
            // var results = Physics.RaycastAll(new Vector3(0, 200f, 0), Vector3.up, 10f);
            
            // Cube, Yes
            // var results = Physics.RaycastAll(new Vector3(0, 198f, 0), Vector3.up, 10f);
            
            // Sphere, Yes
            // var results = Physics.OverlapSphere(new Vector3(0, 200f, 0), 0.01f);
            
            // Sphere, Yes
            // var results = Physics.OverlapSphere(new Vector3(0, 200f, 0), 10f);
            
            // Cube, Yes
            // var results = Physics.RaycastAll(new Vector3(0, 198f, 0), Vector3.up, 10f);
            
            // Debug.Log("Results:" + results.Length);
        }

        // private void OnDrawGizmos()
        // {
        //     Gizmos.DrawSphere(new Vector3(0, 200f, 0), 10f);
        // }

        private List<Vector3Int> GetBlockedNeighborsFromPosition(Vector3Int position)
        {
            var obstacles = new List<Vector3Int>();
            
            // Make sure to check the current position otherwise
            // we end up looking for a path to a location that can't be reached
            var numCurrentPositionObstacles = Physics.OverlapSphereNonAlloc(
                position,
                SpherecastRadius,
                _obstacleColliders,
                _obstacleLayer
            );
            if (numCurrentPositionObstacles > 0)
            {
                var collider = _obstacleColliders[0];
                if (!ColliderIsChild(collider, transform))
                {
                    obstacles.Add(position);
                }
            }
            
            // This is the actual check for the neighboring positions
            foreach (var neighborDirection in PathfindingModule.StandardNeighboringPositions)
            {
                var numObstacles = Physics.OverlapSphereNonAlloc(
                    position + neighborDirection,
                    SpherecastRadius,
                    _obstacleColliders,
                    _obstacleLayer
                );
                
                for (var o = 0; o < numObstacles; o++)
                {
                    var collider = _obstacleColliders[o];
                    if (ColliderIsChild(collider, transform)) continue;
                    obstacles.Add(position + neighborDirection);
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

            if ((bool)path?.Contains(transformPosition))
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

                    // Check the node for obstacles
                    var numObstacles = Physics.OverlapSphereNonAlloc(
                        node,
                        SpherecastRadius * 2f,
                        _obstacleColliders,
                        _obstacleLayer
                    );
                    for (var o = 0; o < numObstacles; o++)
                    {
                        var collider = _obstacleColliders[o];
                        if (ColliderIsChild(collider, transform)) continue;
                        path = SetDestination(_destination);
                        break;
                    }
                    
                    // Check our current position to make sure we're not stepping on an obstacle
                    numObstacles = Physics.OverlapSphereNonAlloc(
                        transformPosition,
                        SpherecastRadius * 2f,
                        _obstacleColliders,
                        _obstacleLayer
                    );
                    for (var o = 0; o < numObstacles; o++)
                    {
                        var collider = _obstacleColliders[o];
                        if (ColliderIsChild(collider, transform)) continue;
                        path = SetDestination(_destination);
                        break;
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
        
        private static bool ColliderIsChild(Component collider, Transform transform)
        {
            return collider.transform == transform || collider.transform.IsChildOf(transform);
        }
    }
}