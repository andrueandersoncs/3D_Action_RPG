using Schemy;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "NewSchemeSource", menuName = "Scheme/Source", order = 0)]
    public class SchemeSource : ScriptableObject
    {
        [HideLabel]
        [MultiLineProperty(25)]
        public string source;

        public Procedure command;

        [Button]
        public void Interpret()
        {
            Debug.Log("Interpreted!");
            var interpreter = SchemyInterpreter.Instance; 
            command = interpreter.InterpretCommand<Procedure>(source, "command");
        }

        private void OnEnable()
        {
            this.ObserveEveryValueChanged(t => t.source).Subscribe(s => Interpret());
        }
    }
}