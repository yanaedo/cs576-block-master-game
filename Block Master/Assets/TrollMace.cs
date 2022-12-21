using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollMace : MonoBehaviour
{

    private TrollAI troll;
    // Start is called before the first frame update
    void Start()
    {
        troll =  gameObject.GetComponentInParent<TrollAI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other) {
        if (troll.attacking) {
            Rigidbody body = other.GetComponent<Rigidbody>();
            if (body != null) {
                // ()
                Vector3 troll_pos = gameObject.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.position;
                Vector3 force = (other.transform.position - troll_pos).normalized * 10000;
                //TODO make player drop cube, then apply force to cube regardless of if player hit. Also, should lower the position this is calculated from a bit
                body.AddForce(force);
                // print(gameObject.transform.position - other.transform.position);
            }
        }
    }

}
