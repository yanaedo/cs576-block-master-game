using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrollAI : MonoBehaviour
{

    public Transform goal;
    private NavMeshAgent agent;
    public Animator animation_controller;
    public float walk_threshold = 0.001f;
    public bool attacking;

    // private bool 

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animation_controller = GetComponent<Animator>();
        agent.destination = goal.position; 
        attacking = false;
        // agent.Stop
    }

    // Update is called once per frame
    void Update()
    {
        // print(agent.desiredVelocity);
        var curState = animation_controller.GetCurrentAnimatorStateInfo(0);
        agent.destination = goal.position;
        if (agent.remainingDistance < 4f) {
        // if (agent.remainingDistance < 4f && !curState.IsName("attack1")) {
            if (!curState.IsName("attack1") || curState.normalizedTime >= 1f) {
                // print(agent.remainingDistance);
                animation_controller.SetTrigger("attack1");
                // animation_controller.ResetTrigger("idle");
                // animation_controller.ResetTrigger("walk");
            } else {
                animation_controller.ResetTrigger("attack1");
            }
        } else if (agent.desiredVelocity.magnitude > walk_threshold) {
            if (!curState.IsName("walk")) {
                // animation_controller.ResetTrigger("idle");
                animation_controller.SetTrigger("walk");
            } else {
                animation_controller.ResetTrigger("walk");
            }
        } else {
            // animation_controller.ResetTrigger("walk");
            // print(agent.remainingDistance);
            if (!curState.IsName("attack1")) {
                animation_controller.SetTrigger("idle_break");
            }
            // animation_controller.SetTrigger("idle");
        }
        // print(agent.isStopped);
        if (curState.IsName("attack1") && curState.normalizedTime < 0.95f) {
            agent.isStopped = true;
            attacking = true;
            // print("AAA");
        } else {
            agent.isStopped = false;
            attacking = false;
        }
        // if (agent.remainingDistance < 1f) //TODO can do something like this to trigger attack animation
    }
}
