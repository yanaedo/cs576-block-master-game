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
    }

    // Update is called once per frame
    void Update()
    {
        
        if (PauseScript.isPaused) {
            // Do nothing if paused

        // Rotate camera left
        } else if (Input.GetKey(KeyCode.Q)) {
            transform.RotateAround(character.transform.position, up, -turn_speed * Time.deltaTime);

        // Rotate camera right
        } else if (Input.GetKey(KeyCode.E)) {
            transform.RotateAround(character.transform.position, up, turn_speed * Time.deltaTime);

        // Reset the camera to over Claire's shoulder
        } else if (Input.GetKey(KeyCode.X)) {
            if (!cam_was_reset) {
                Vector3 new_position = character.transform.position;
                new_position += rel_offset * character.transform.right;   // 'x'
                new_position += rel_height * character.transform.up;      // 'y'
                new_position += rel_back   * character.transform.forward; // 'z'

                // update the position + angle
                transform.position = new_position;
                transform.eulerAngles = character.transform.eulerAngles + relativeAngle;

                cam_was_reset = true;
            }   
        } else {
            cam_was_reset = false;
        }
    }
}
