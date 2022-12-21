using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrollAI : MonoBehaviour
{

    public Transform player;
    private NavMeshAgent agent;
    public Animator animation_controller;
    public float walk_threshold = 0.001f;
    public bool attacking;

    private int attackIndex;
    // private bool 

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animation_controller = GetComponent<Animator>();
        // agent.destination = player.position; 
        attacking = false;
        attackIndex = 1;
        // agent.Stop
    }

    // Update is called once per frame
    void Update()
    {
        // print(agent.desiredVelocity);
        var curState = animation_controller.GetCurrentAnimatorStateInfo(0);
        attacking = (curState.IsName("attack1") || curState.IsName("attack2")) && curState.normalizedTime % 1 < 0.975f;
        bool player_has_key = player.GetComponent<Claire>().inventory.GetComponent<InventoryScript>().held_object != null;
        bool active = player_has_key || Vector3.Distance(gameObject.transform.position, player.position) < 8f;
        if (active) {
            agent.destination = player.position;
        }
        if (agent.remainingDistance < 4f && active) {
        // if (agent.remainingDistance < 4f && !curState.IsName("attack1")) {
            if (!attacking || curState.normalizedTime >= 1f) {
                if (!animation_controller.GetBool("attack1") && !animation_controller.GetBool("attack2")) {
                
            // if (!curState.IsName("attack1") || curState.normalizedTime >= 1f) {
                // print(agent.remainingDistance);
                    // print(attackIndex);
                    agent.velocity = Vector3.zero;
                    if (attackIndex == 1) {
                        animation_controller.ResetTrigger("attack2");
                        animation_controller.SetTrigger("attack1");
                        attackIndex = 2;
                    } else {
                        animation_controller.ResetTrigger("attack1");
                        animation_controller.SetTrigger("attack2");
                        attackIndex = 1;
                    }
                    attacking = true;
                        // print(animation_controller.GetBool("attack1"));
                }
                // animation_controller.ResetTrigger("idle");
                // animation_controller.ResetTrigger("walk");
            } else {
                // print("aa");
                animation_controller.ResetTrigger("attack1");
                animation_controller.ResetTrigger("attack2");
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
            if (!attacking) {
                if (!curState.IsName("idle_break") && !curState.IsName("idle")) {
                    animation_controller.SetTrigger("idle_break");
                } else if (curState.IsName("idle_break") && curState.normalizedTime >= 1f) {
                    animation_controller.SetTrigger("idle");
                }
                // print(curState.normalizedTime);
                // print(curState.loop);
            }
            // animation_controller.SetTrigger("idle");
        }
        // print(agent.isStopped);
        // attacking = (curState.IsName("attack1") || curState.IsName("attack2")) && curState.normalizedTime % 1 < 0.975f;
        if (attacking) {
        // if (attacking && curState.normalizedTime % 1 < 0.975f) {
            agent.isStopped = true;
            // print(agent.desiredVelocity);
            // attacking = true;
        } else {
            agent.isStopped = false;
            // attacking = false;
        }
    }
}
