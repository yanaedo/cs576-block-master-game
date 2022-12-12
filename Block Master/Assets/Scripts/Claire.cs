using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Claire : MonoBehaviour {

    private Animator animation_controller;
    private CharacterController character_controller;
    private Dictionary<string, int> states;

    public Vector3 movement_direction;
    public float walking_velocity;  
    public float velocity;
    public float turn_speed;

    public float pickup_range;
    private float pickup_force;
    public float object_rotation = 15f;
    public Transform holdArea;
    private GameObject held_object;
    private Rigidbody  held_object_RB;

    // TODO: try to get this programatically
    // Knowing the script is attached to claire, and she has a camera object in her hierarchy
    public Camera _camera;

	// Use this for initialization
	void Start ()
    {
        animation_controller = GetComponent<Animator>();
        character_controller = GetComponent<CharacterController>();
        movement_direction = new Vector3(0.0f, 0.0f, 0.0f);
        walking_velocity = 3.5f;
        velocity = 0.0f;
        turn_speed = 1.0f;

        states = new Dictionary<string, int>() {
                {"idle", 0},
                {"walkingForward", 1},
                {"walkingBackward", 2},
                {"runningForwards", 3},
                {"jumping", 4},
            };

        pickup_range = 15f;
        // Note from Griffin: Probably want to limit the max range on this more eventually, since seems pretty far currently (like, enough that you can grab a cube and have it be stuck behind a wall on its way to the player). Maybe should be range from player character rather than from camera which I think is what it currently is
        pickup_force = 75f;
        // object_rotation = 15f; //45 degrees = 8 positions per axis //Griffin: commented out so I can move this to be set as a public variable and be more flexible
    }

    // Update is called once per frame
    void Update()
    {
        // Handle movement related key presses
        // Move:    UpArrow, DownArrow
        // Rotate:  LeftArrow, RightArrow
        // Actions: Control (run), Space (jump)
        handleMovement();

        // Handle picking up, and dropping a key
        // Pickup:   LeftClick
        // Drop Key: RightClick
        if (Input.GetMouseButtonDown(0) && held_object == null) {
            TryPickupKey();
        } else if (Input.GetMouseButtonDown(1) && held_object != null) {
            DropKey();
        }

        // If we have a held object, move it to the desired position
        if (held_object != null && Vector3.Distance(held_object.transform.position, holdArea.position) > 0.1f) {
            Vector3 moveDirection = (holdArea.position - held_object.transform.position);
            held_object_RB.AddForce(moveDirection * pickup_force);
        }

        // Handle Rotating Keys
        // TODO: find a better mapping
        // axis:        (x) (y) (z)
        // Rotate Left:  Y,  H,  N
        // Rotate Right: U,  J,  M
        if (held_object != null) {
            RotateKey();
        }
        

    }

    void TryPickupKey() {
        // Following this tutorial: https://www.youtube.com/watch?v=6bFCQqabfzo

        // Construct a ray from the current mouse coordinates
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // if we hit an object in range (of camera) check if its a key
        if (Physics.Raycast(ray, out hit, pickup_range)) {
            GameObject pickup_obj = hit.transform.gameObject;

            // If we've hit a key, pick it up
            if (pickup_obj.tag.ToLower().Contains("key") && pickup_obj.GetComponent<Rigidbody>()) {
                held_object_RB = pickup_obj.GetComponent<Rigidbody>();
                held_object_RB.useGravity = false;
                held_object_RB.drag = 10;
                held_object_RB.constraints = RigidbodyConstraints.FreezeRotation; // Prevents unwanted rotations
                held_object_RB.transform.parent = holdArea;
                held_object = pickup_obj;

                // Shrink the object when we pick it up
                //Griffin: disabling this because I want key to be same size as hole it's being lined up with
                // Vector3 scale = held_object.transform.localScale;
                // held_object.transform.localScale = new Vector3(scale.x * 2 / 3, scale.y * 2 / 3, scale.z * 2 / 3);
            }
        }
    }

    void DropKey() {
        held_object_RB.useGravity = true;
        held_object_RB.drag = 1;
        held_object_RB.constraints = RigidbodyConstraints.None;

        // Grow the object when we drop it
        // Vector3 scale = held_object.transform.localScale;
        // held_object.transform.localScale = new Vector3(scale.x * 3 / 2, scale.y * 3 / 2, scale.z * 3 / 2);

        held_object_RB.transform.parent = null;
        held_object = null;
    }

    void RotateKey() {
        // Could do this with
            // pushing button (1 push = 45 degrees)
            // holding button (1 push = 1 degree)
            // continuous steps (physics - AddTorque)
                // https://docs.unity3d.com/ScriptReference/Rigidbody.AddTorque.html
        //Note from Griffin: Currently triggers on every frame but has object_rotation = 15f which means it seems to turn very fast; just going to lower that value for now, so I can test the inserting into slots, not sure which setup you prefer/intend. I think also if possible, clicking and dragging could be a good control method since that'd give better precision
        //                   Personally I think snapping to angles maybe only when the player releases the rotation controls could work? So it'd rotate slower/smoother but then when the player stops pressing the button it'd go into position
        //                   Also maybe shouldn't rotate with the player's movement if it's going to snap to angles, as otherwise need player to line up their character as well as the key, which is kind of difficult when controlling player turning with arrow keys (could also do mouse control for player rotation and that'd resolve the issue as it'd be more precise regardless)
        Vector3 rotata = new Vector3();
        bool rotate_x_left   = Input.GetKey(KeyCode.Y);
        bool rotate_x_right  = Input.GetKey(KeyCode.U);
        bool rotate_y_left   = Input.GetKey(KeyCode.H);
        bool rotate_y_right  = Input.GetKey(KeyCode.J);
        bool rotate_z_left   = Input.GetKey(KeyCode.N);
        bool rotate_z_right  = Input.GetKey(KeyCode.M);

        // Handle X rotation
        if (rotate_x_left) {
            rotata.x = object_rotation;
        } else if (rotate_x_right) {
            rotata.x = -1 * object_rotation;
        }

        // Handle Y rotation
        if (rotate_y_left) {
            rotata.y = object_rotation;
        } else if (rotate_y_right) {
            rotata.y = -1 * object_rotation;
        }

        // Handle Z rotation
        if (rotate_z_left) {
            rotata.z = object_rotation;
        } else if (rotate_z_right) {
            rotata.z = -1 * object_rotation;
        }

        // Rotate in world/global space
        // because its a lot easier to visualize than local space (local to the key)
        held_object.transform.Rotate( rotata , Space.World);
    }

    void handleMovement() {
        // Get the relevant keys pressed to determine type of movement
        bool forwards_key  = Input.GetKey(KeyCode.UpArrow);
        bool backwards_key = Input.GetKey(KeyCode.DownArrow);
        bool running_key   = Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift);
        bool jumping_key   = Input.GetKey(KeyCode.Space);

        // type of movement is determined hierarachaly from highest state (jumping) to lowest state (idle)

        // The repeated check for IsName("Jump") (within each if statement) enforces the jump behavior - though its hacky
        //      Specifically the state must be capable of changing, but the speed shouldn't change until after the animation finishes

        if (jumping_key) {
            // jumping
            if (!animation_controller.GetCurrentAnimatorStateInfo(0).IsName("Jump")) {
                animation_controller.SetInteger("state", states["jumping"]);

            }
            update_velocity(velocity, 2.5f, '+');
            
        } else if (forwards_key && running_key) {
            // running forwards
            animation_controller.SetInteger("state", states["runningForwards"]);

            if (!animation_controller.GetCurrentAnimatorStateInfo(0).IsName("Jump")) {
                update_velocity(velocity, 2.0f, '+');
            }

        } else if (forwards_key) {
            // walking forwards
            animation_controller.SetInteger("state", states["walkingForward"]);

            if (!animation_controller.GetCurrentAnimatorStateInfo(0).IsName("Jump")) {
                update_velocity(velocity, 1.0f, '+');
            }
            
        } else if (backwards_key) {
            // walking backwards
            animation_controller.SetInteger("state", states["walkingBackward"]);

            if (!animation_controller.GetCurrentAnimatorStateInfo(0).IsName("Jump")) {
                update_velocity(velocity, 2f/3f, '-');
            }

        } else {
            // idle
            animation_controller.SetInteger("state", states["idle"]);

            if (!animation_controller.GetCurrentAnimatorStateInfo(0).IsName("Jump")) {
                velocity = 0.0f;
            }
        }


        // Handle rotations (slower while jumping)
        float cur_turn_speed = turn_speed;
        if (animation_controller.GetCurrentAnimatorStateInfo(0).IsName("Jump")) {
            cur_turn_speed /= 2;
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            transform.Rotate(new Vector3(0.0f, -cur_turn_speed, 0.0f));
        } else if (Input.GetKey(KeyCode.RightArrow)) {
            transform.Rotate(new Vector3(0.0f, cur_turn_speed, 0.0f));
        }

        // End animation, speed, and rotation control

        // Use that info to actually control the character
        float xdirection = Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.y);
        float zdirection = Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.y);
        movement_direction = new Vector3(xdirection, 0.0f, zdirection);

        // character controller's move function is useful to prevent the character passing through the terrain
        // (changing transform's position does not make these checks)
        if (transform.position.y > 0.0f) { // if the character starts "climbing" the terrain, drop her down
            Vector3 lower_character = movement_direction * velocity * Time.deltaTime;
            lower_character.y = -100f; // hack to force her down, doesn't work with jumping to a height
            character_controller.Move(lower_character);
        } else {
            character_controller.Move(movement_direction * velocity * Time.deltaTime);
        }
    }

    void update_velocity(float vi, float scale, char direction) {
        // Define the target velocity as some multiple of the default walking speed
        float target_velocity = walking_velocity * scale;
        if (direction == '-') {
            target_velocity *= -1;
        }

        // Find a step that acts like constant acceleration
        float difference = target_velocity - vi;
        float step = difference / 20;
        float min_step = 0.05f;

        // Ensure step is at least min_step (in the proper direction)
        if (direction == '+') {
            step = Mathf.Max(step, min_step);
        } else {
            step = Mathf.Min(step, -1 * min_step);
        }

        // Find the final velocity and make sure it doesn't exceed the target velocity
        float vf = vi + step;
        if (direction == '+') {
            vf = Mathf.Min(vf, target_velocity);
        } else {
            vf = Mathf.Max(vf, target_velocity);
        }

        // update the velocity
        velocity = vf;
    }

}
