using UnityEngine;

namespace Skills.Effects
{
    public abstract class SkillEffect : MonoBehaviour
    {
        public abstract void Apply(GameObject target);
        public abstract void Unapply(GameObject target);
        public abstract string GetName();
    }
}