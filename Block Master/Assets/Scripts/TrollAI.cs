using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrollAI : MonoBehaviour
{

    public Transform goal;
    private NavMeshAgent agent;
    public Animator animation_controller;
    public float walk_threshold = 0.2f;

    // private bool 

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animation_controller = GetComponent<Animator>();
        agent.destination = goal.position; 
        // agent.Stop
    }

    // Update is called once per frame
    void Update()
    {
        // print(agent.desiredVelocity);
        agent.destination = goal.position;
        if (agent.desiredVelocity.magnitude > walk_threshold) {
            if (!animation_controller.GetCurrentAnimatorStateInfo(0).IsName("walk")) {
                animation_controller.ResetTrigger("idle");
                animation_controller.SetTrigger("walk");
            }
        } else {
            animation_controller.ResetTrigger("walk");
            animation_controller.SetTrigger("idle");
        }
        // if (agent.remainingDistance < 1f) //TODO can do something like this to trigger attack animation
    }
}
