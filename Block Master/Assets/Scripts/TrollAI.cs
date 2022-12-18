using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrollAI : MonoBehaviour
{

    public Transform goal;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position; 
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = goal.position;
    }
}
