using UnityEngine;

namespace AI
{
    public class StateMachine : MonoBehaviour
    {
        // track current state
        
        // Condition: Func<GameObject, bool>
        // Condition Combinators for composing conditions
        // Condition Combinator: Func<Condition, Condition, Condition>
        // - And (A, B)
        // - Or (A, B)
        
        // Command: Func<GameObject, IEnumerator>
        // Command Combinators for composing commands
        // Command Combinator: Func<Command, Command, Command>
        // - Sequence (A, B)
        // - Parallel (A, B)
        
    }
}