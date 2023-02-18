using System;
using System.Collections.Generic;
using Stats.Vitals;
using UniRx;
using UnityEngine;

namespace Player
{
    public class CommandInterpreter : MonoBehaviour
    {
        public static Dictionary<string, Action<Stack<object>, Dictionary<string, object>>> commands = new()
        {
            {"print()", Print},
            {"get-component()", GetComponent},
            {"gc()", GetComponent},
            {"take-damage()", TakeDamage},
            {"set", SetVariable},
            {"get", GetVariable},
            {"receive-heal()", ReceiveHeal}
        };

        [Multiline(10)]
        public string onEnableCommand;
        
        private Stack<object> stack = new();
        private Dictionary<string, object> variables = new();

        private void OnEnable()
        {
            variables["gameObject"] = gameObject;
            Interpret(onEnableCommand);
        }

        public void Push(object commandOrData)
        {
            if (commandOrData is string commandName && commands.TryGetValue(commandName, out var func))
            {
                func(stack, variables);
            }
            else
            {
                stack.Push(commandOrData);
            }
        }

        public void Interpret(string source)
        {
            var commands = source.Split("\n");
            foreach (var command in commands)
            {
                Push(command);
            }
        }
        
        private static void Print(Stack<object> stack, Dictionary<string, object> variables)
        {
            var arg = stack.Pop();
            Debug.Log(arg);
        }
        
        private static void GetComponent(Stack<object> stack, Dictionary<string, object> variables)
        {
            var componentType = stack.Pop();
            var gameObject = stack.Pop();
            var component = ((GameObject) gameObject).GetComponent(componentType as string);
            stack.Push(component);
        }
        
        private static void TakeDamage(Stack<object> stack, Dictionary<string, object> variables)
        {
            var damage = int.Parse(stack.Pop() as string);
            var gameObject = stack.Pop() as GameObject;
            var health = gameObject.GetComponent<VitalStats>();
            health.Health -= damage;
        }
        
        private static void SetVariable(Stack<object> stack, Dictionary<string, object> variables)
        {
            var value = stack.Pop();
            var key = stack.Pop() as string;
            variables[key] = value;
        }
        
        private static void GetVariable(Stack<object> stack, Dictionary<string, object> variables)
        {
            var key = stack.Pop() as string;
            stack.Push(variables[key]);
        }
        
        private static void ReceiveHeal(Stack<object> stack, Dictionary<string, object> variables)
        {
            var heal = int.Parse(stack.Pop() as string);
            var gameObject = stack.Pop() as GameObject;
            var health = gameObject.GetComponent<VitalStats>();
            health.Health += heal;
        }
    }
}