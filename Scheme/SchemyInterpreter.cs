using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Schemy;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Player
{
    public class SchemyInterpreter : MonoBehaviour
    {
        public static SchemyInterpreter Instance
        {
            get
            {
                if (instance != null) return instance;

                var existingObject = GameObject.Find("SchemyInterpreter");
                if (existingObject != null)
                {
                    if (existingObject.TryGetComponent<SchemyInterpreter>(out var existingInstance))
                    {
                        instance = existingInstance;
                        return instance;
                    }

                    instance = existingObject.AddComponent<SchemyInterpreter>();
                    return instance;
                }

                var g = new GameObject("SchemyInterpreter");
                instance = g.AddComponent<SchemyInterpreter>();
                return instance;
            }
        }
        
        private static SchemyInterpreter instance;

        private Interpreter interpreter = new(
            new[] {
                (Interpreter.CreateSymbolTableDelegate)((Interpreter _) =>
                    new Dictionary<Symbol, object> {
                        {
                            Symbol.FromString("print"),
                            new NativeProcedure(strings =>
                            {
                                Debug.Log(strings[0]);
                                return default;
                            })
                        }, 
                        {
                            Symbol.FromString("find"),
                            new NativeProcedure(strings => GameObject.Find(strings[0] as string))
                        },
                        {
                            Symbol.FromString("destroy"),
                            new NativeProcedure(objects =>
                            {
                                Destroy(objects[0] as Object);
                                return default;
                            })
                        },
                        {
                            Symbol.FromString("instantiate"),
                            new NativeProcedure(objects =>
                            {
                                var prefab = objects[0] as GameObject;
                                return Instantiate(prefab);
                            })
                        },
                        {
                            Symbol.FromString("instantiate/3"),
                            new NativeProcedure(objects =>
                            {
                                var prefab = objects[0] as GameObject;
                                var position = objects[1] as Vector3? ?? default;
                                var rotation = objects[2] as Quaternion? ?? default;
                                return Instantiate(prefab, position, rotation);
                            })
                        },
                        {
                            Symbol.FromString("load-addressable"),
                            new NativeProcedure(strings =>
                            {
                                var address = strings[0] as string;
                                return Addressables.LoadAssetAsync<GameObject>(address);
                            })
                        }
                    })
            }, new ReadOnlyFileSystemAccessor());

        public T InterpretCommand<T>(string source, string commandName)
        {
            var evaluationResult = interpreter.Evaluate(new StringReader(source));

            if (evaluationResult.Error != null)
            {
                Debug.Log("Evaluation Error: " + evaluationResult.Error);
                return default;
            }
            
            if (interpreter.Environment.TryGetValue(Symbol.FromString(commandName), out var result))
            {
                Debug.Log(result.GetType());
                return (T)result;
            }
            
            Debug.Log("Could not find symbol in environment");
            return default;
        }

        public Interpreter.EvaluationResult InterpretFile(string fullPath)
        {
            if (!File.Exists(fullPath)) return default;
            using var reader = new StreamReader(fullPath);
            return interpreter.Evaluate(reader);
        }
    }
}