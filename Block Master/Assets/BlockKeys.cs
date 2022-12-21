using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockKeys : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other) { 
        // Transform par = other.transform.parent;
        // if (par != null) {
        Claire player = other.GetComponent<Claire>();
        if (player != null) {
            // print(other);
            player.inventory.GetComponent<InventoryScript>().DropKey();
        } else {
            InventoryScript inv = other.GetComponentInParent<InventoryScript>();
            if (inv != null) {
                inv.DropKey();
            }
        }
        // }
    }
}
