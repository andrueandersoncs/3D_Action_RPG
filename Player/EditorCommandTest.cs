using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player
{
    public class EditorCommandTest : MonoBehaviour
    {
        [Multiline(10)]
        public string command;

        private CommandInterpreter commandInterpreter;
        
        private void OnEnable()
        {
            commandInterpreter = GetComponent<CommandInterpreter>();
        }

        [Button("Interpret")]
        public void Interpret()
        {
            commandInterpreter.Interpret(command);
        }
    }
}