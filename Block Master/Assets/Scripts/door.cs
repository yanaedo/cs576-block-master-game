using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    // Start is called before the first frame update
    public keyslot keySlot;
    public bool open;
    public Animator animation_controller;
    void Start()
    {
        open = false;
        animation_controller.SetBool("Open", false);
    }

    // Update is called once per frame
    void Update()
    {
        // open = false;
        if (keySlot.activated) {
            // print("AAAA");
            open = true;
        } else {
            open = false;
        }
        animation_controller.SetBool("Open", open);
    }
}
