using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    // Start is called before the first frame update
    public keyslot keySlot;
    public bool open;
    public Animator animation_controller;

    private AudioSource open_sound;
    private bool was_closed;

    void Start()
    {
        open = false;
        was_closed = true;
        animation_controller.SetBool("Open", false);
        open_sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // open = false;
        if (keySlot.activated) {
            // print("AAAA");
            open = true;
            if (was_closed) {
                open_sound.Play();
                was_closed = false;
            }
        } else {
            open = false;
            was_closed = true;
        }
        animation_controller.SetBool("Open", open);
    }
}
