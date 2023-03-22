using UnityEngine;
using UnityEngine.AI;

public class SimpleClickToMove : MonoBehaviour
{
    NavMeshAgent agent;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                agent.SetDestination(hit.point);
            }
        }
    }
}
