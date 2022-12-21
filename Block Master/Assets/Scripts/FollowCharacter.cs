using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCharacter : MonoBehaviour
{

    public GameObject character;    // The character (or gameObject) to follow
    public float turn_speed = 40f;  // The speed of the camera rotation
    private Vector3 up;             // points up, re-used enough to be a global

    // Used to establish the relative orientation of the character and the camera
    private Vector3 relativeAngle; 
    private float rel_offset; // delta x
    private float rel_height; // delta y
    private float rel_back;   // delta z
    private bool cam_was_reset;

    // when resetting the camera, moves the inventory location but not the held item
    public GameObject inventory;
    private InventoryScript inventory_script;

    // Start is called before the first frame update
    void Start()
    {
        up = new Vector3(0, 1, 0);
        relativeAngle = transform.eulerAngles - character.transform.eulerAngles;

        Vector3 relativePosition = transform.position - character.transform.position;
        rel_offset = relativePosition.x;
        rel_height = relativePosition.y;
        rel_back   = relativePosition.z;

        cam_was_reset = false;

        inventory_script = inventory.GetComponent<InventoryScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseScript.isPaused) {
            // Do nothing if paused

        // Rotate camera left
        } else if (Input.GetKey(KeyCode.Q)) {
            rotate_camera(-turn_speed);

        // Rotate camera right
        } else if (Input.GetKey(KeyCode.E)) {
            rotate_camera(turn_speed);

        // Reset the camera to over Claire's shoulder
        } else if (Input.GetKey(KeyCode.X)) {
            if (!cam_was_reset) {
                reset_camera();
            }   
        } else {
            cam_was_reset = false;
        }
    }

    void rotate_camera(float base_speed) {
        // If the player is running, the camera will rotate faster
        if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift)) {
            base_speed *= 1.2f;
        }
        transform.RotateAround(character.transform.position, up, base_speed * Time.deltaTime);
    }

    void reset_camera() {
        // Get the new position of the camera
        Vector3 new_position = character.transform.position;
        new_position += rel_offset * character.transform.right;   // 'x'
        new_position += rel_height * character.transform.up;      // 'y'
        new_position += rel_back   * character.transform.forward; // 'z'


        // Store info about the held_object (if we have one)
        // The way we do reset the camera, the inventory can clip out of bounds
        // We mess with the held_object so that the keys don't do the same 
        Vector3 child_angles = new Vector3();
        Transform last_parent = null;
        if (inventory_script.held_object != null) {
            child_angles = inventory_script.held_object.transform.localEulerAngles;
            last_parent = inventory_script.held_object.transform.parent;

            inventory_script.held_object.transform.parent = null;
        }

        // update the position + angle of the camera
        transform.position = new_position;
        transform.eulerAngles = character.transform.eulerAngles + relativeAngle;

        // Now that the camera (+ inventory location) have moved
        // Restore the relationship with the held_object (key).
        if (inventory_script.held_object != null) {
            inventory_script.held_object.transform.parent = last_parent;
            inventory_script.held_object.transform.localEulerAngles = child_angles;
        }

        cam_was_reset = true;
    }
}
