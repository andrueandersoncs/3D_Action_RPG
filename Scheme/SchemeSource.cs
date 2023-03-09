using IronScheme;
using IronScheme.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scheme
{
    [CreateAssetMenu(fileName = "NewSchemeSource", menuName = "Scheme/Source", order = 0)]
    public class SchemeSource : ScriptableObject
    {
        [HideLabel]
        [MultiLineProperty(25)]
        public string source;

        private static string prelude = @"
(import (rnrs) (ironscheme clr))
    
(clr-using UnityEngine)

(define-syntax get-component
    (lambda (e)
      (syntax-case e ()
        [(_ instance type)
          #'(clr-call GameObject (GetComponent #(type)) instance)])))

(define (debug-log message) (clr-static-call Debug Log message))

(define (find name) (clr-static-call GameObject Find name))

(define (name game-object) (clr-prop-get UnityEngine.GameObject name game-object))
";
        
        [Button]
        public void Interpret()
        {
            // (clr-call GameObject (GetComponent #(Transform)) (find "Player))
            var proc2 = "(lambda (x) (+ 2 x))".Eval<Callable>();
            var result2 = proc2.Call(3);
            Debug.Log("Result: " + result2);

            var src = $"(begin {prelude}\n{source})";
            Debug.Log("Src:" + src);
            var proc = src.Eval<Callable>();
            var result = proc.Call();
            Debug.Log("Result: " + result);
        }

        [Button]
        public void Execute(GameObject target)
        {
            var proc = $"(begin {prelude}\n{source})".Eval<Callable>();
            proc.Call(target);
        }
    }
}