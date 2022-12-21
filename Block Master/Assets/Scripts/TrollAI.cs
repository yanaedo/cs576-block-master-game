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
    public float key_detect_range = 40f;
    public float no_key_detect_range = 8f;

    private int attackIndex;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animation_controller = GetComponent<Animator>();
        attacking = false;
        attackIndex = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // print(agent.desiredVelocity);
        var curState = animation_controller.GetCurrentAnimatorStateInfo(0);
        attacking = (curState.IsName("attack1") || curState.IsName("attack2")) && curState.normalizedTime % 1 < 0.975f;
        bool player_has_key = player.GetComponent<Claire>().inventory.GetComponent<InventoryScript>().held_object != null;
        // bool active = player_has_key || player_distance < 8f;
        float player_distance = Vector3.Distance(gameObject.transform.position, player.position);
        bool active = player_distance < no_key_detect_range;
        if (player_has_key && !active) {
            //when holding key, has longer detection range but based on path length rather than direct distance
            NavMeshPath potential_path = new NavMeshPath();
            agent.CalculatePath(player.position, potential_path);
            float potential_distance = 0;
            for (int i = 0; i < potential_path.corners.Length - 1; ++i) {
                potential_distance += Vector3.Distance(potential_path.corners[i], potential_path.corners[i + 1]);
            }
            // print(potential_distance);
            if (potential_distance < key_detect_range && potential_distance != 0) {
                active = true;
            }
        }
        if (active) {
            agent.destination = player.position;
        }
        if (agent.remainingDistance < 4f && active) {
            if (!attacking || curState.normalizedTime >= 1f) {
                if (!animation_controller.GetBool("attack1") && !animation_controller.GetBool("attack2")) {
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
                }
            } else {
                animation_controller.ResetTrigger("attack1");
                animation_controller.ResetTrigger("attack2");
            }
        } else if (agent.desiredVelocity.magnitude > walk_threshold) {
            if (!curState.IsName("walk")) {
                animation_controller.SetTrigger("walk");
            } else {
                animation_controller.ResetTrigger("walk");
            }
        } else {
            if (!attacking) {
                if (!curState.IsName("idle_break") && !curState.IsName("idle")) {
                    animation_controller.SetTrigger("idle_break");
                } else if (curState.IsName("idle_break") && curState.normalizedTime >= 1f) {
                    animation_controller.SetTrigger("idle");
                }
            }
        }
        if (attacking) {
            agent.isStopped = true;
        } else {
            agent.isStopped = false;
        }
    }
}
