using UnityEngine;
using UnityEngine.Events;

namespace Callbacks
{
    public class OnTriggerEnterPathfindingObstacle : MonoBehaviour
    {
        public UnityEvent<Collider> onTriggerEnterPathfindingObstacleCollider;
        public UnityEvent onTriggerEnterPathfindingObstacle;
        
        private int _pathfindingObstacleLayer;

        private void OnEnable()
        {
            _pathfindingObstacleLayer = LayerMask.GetMask("PathfindingObstacle");
        }

        private void OnTriggerEnter(Collider other)
        {
            var binaryMaskOfBothLayers = other.gameObject.layer & ~_pathfindingObstacleLayer;
            var objectIsNotInLayer = binaryMaskOfBothLayers == 0;
            if (objectIsNotInLayer) return;
            onTriggerEnterPathfindingObstacle.Invoke();
            onTriggerEnterPathfindingObstacleCollider.Invoke(other);
        }
    }
}