using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollMace : MonoBehaviour
{

    private TrollAI troll;
    // Start is called before the first frame update
    void Start()
    {
        troll = gameObject.GetComponentInParent<TrollAI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider other) { 
    // void OnTriggerEnter(Collider other) { //not sure if stay or enter works better; I think stay maybe since it can force the player out of the way better?
        if (troll.attacking) {
            Rigidbody body = other.GetComponent<Rigidbody>();
            if (body != null) {
                Vector3 troll_pos = gameObject.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.position; //this line looks absurd but I'm not sure of a better way to access that position from here
                troll_pos.y -= 0.35f;
                Vector3 force = (other.transform.position - troll_pos).normalized * 30f;
                // body.AddForce(force);
                body.AddForce(force, ForceMode.Impulse); //I think just default is maybe more consistent but impulse seems to deal with this better
            }
            InventoryScript inv = other.GetComponentInParent<InventoryScript>();
            Claire player = other.GetComponent<Claire>();
            if (inv == null) {
                if (player != null) {
                    inv = player.inventory.GetComponent<InventoryScript>();
                }
            }
            if (inv != null) {
                GameObject held_object = inv.held_object;
                inv.DropKey();
                if (player != null && held_object != null) {
                    // OnTriggerEnter(held_object.GetComponent<Collider>());
                    OnTriggerStay(held_object.GetComponent<Collider>());
                }
            }
        }
    }

}
