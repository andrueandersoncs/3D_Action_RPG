using Sirenix.OdinInspector;
using UnityEngine;

namespace Terrain
{
    public class SetTerrainDetailPrototypeSizes: MonoBehaviour
    {
        public float minWidth;
        public float maxWidth;
        public float minHeight;
        public float maxHeight;
        public float grassModifier;
        public float bushModifier;
        public float bigModifier;
        public float otherModifier;
        
        [Button]
        public void SetDetailPrototypeSizes()
        {
            var terrain = GetComponent<UnityEngine.Terrain>();
            var detailPrototypes = terrain.terrainData.detailPrototypes;
            foreach (var detailPrototype in detailPrototypes)
            {
                if (detailPrototype.prototype.name.Contains("Grass"))
                {
                    detailPrototype.minWidth = minWidth * grassModifier;
                    detailPrototype.maxWidth = maxWidth * grassModifier;
                    detailPrototype.minHeight = minHeight * grassModifier;
                    detailPrototype.maxHeight = maxHeight * grassModifier;
                } else if (detailPrototype.prototype.name.Contains("Bush"))
                {
                    detailPrototype.minWidth = minWidth * bushModifier;
                    detailPrototype.maxWidth = maxWidth * bushModifier;
                    detailPrototype.minHeight = minHeight * bushModifier;
                    detailPrototype.maxHeight = maxHeight * bushModifier;
                } else if (detailPrototype.prototype.name.Contains("Big"))
                {
                    detailPrototype.minWidth = minWidth * bigModifier;
                    detailPrototype.maxWidth = maxWidth * bigModifier;
                    detailPrototype.minHeight = minHeight * bigModifier;
                    detailPrototype.maxHeight = maxHeight * bigModifier;
                }
                else
                {
                    detailPrototype.minWidth = minWidth * otherModifier;
                    detailPrototype.maxWidth = maxWidth * otherModifier;
                    detailPrototype.minHeight = minHeight * otherModifier;
                    detailPrototype.maxHeight = maxHeight * otherModifier;
                }
            }
            terrain.terrainData.detailPrototypes = detailPrototypes;
        }
    }
}