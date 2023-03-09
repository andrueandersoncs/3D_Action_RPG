using IronScheme;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scheme
{
    public class PlayerIronSchemeInterpreter : MonoBehaviour
    {
        public string filename;
        
        private void Start()
        {
            Execute();
        }
        
        [Button]
        public void Execute() => $"".Eval();
    }
}