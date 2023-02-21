using UnityEngine;

namespace Movement
{
    public static class PositioningExtensions
    {
        public static readonly Vector3Int[] StandardNeighboringPositions = {
            new(1, 0, 0),
            new(-1, 0, 0),
            new(0, 0, 1),
            new(0, 0, -1),
            new(1, 0, 1),
            new(1, 0, -1),
            new(-1, 0, 1),
            new(-1, 0, -1)
        };

        public static Vector3Int RoundToInt(this Vector3 v) =>
            new(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));
        
        public static Vector3Int RoundToPosition(this Vector3 v) =>
            new(Mathf.RoundToInt(v.x), 0, Mathf.RoundToInt(v.z));
    }
}