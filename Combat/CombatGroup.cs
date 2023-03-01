using UnityEngine;

namespace Combat
{
    public class CombatGroup : MonoBehaviour
    {
        public string group;

        public bool CanDamage(CombatGroup other) => other.group != group;
    }
}