using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Claire : MonoBehaviour {

    public float walking_velocity = 25f;  
    public float jump_height = 15f;
    // TODO: Get this programatically - knowing they're in Claire's hierarchy
    public Camera _camera;
    public Transform inventory; // Where the key will be placed
    
    private Animator animation_controller;
    private CharacterController character_controller;
    private Dictionary<string, int> states; // animation states
    private Rigidbody claire_RB;

    // Defines control keys and whether they're pressed
    private KeyCode jumping_key;
    private KeyCode[] forwards_keys, backwards_keys, left_keys, right_keys, running_keys;
    private bool forwards, backwards, left, right, running, jumping, moving;

    // Used so player can pick up an object through the enemy barrier
    [SerializeField] private LayerMask exclude_enemy_layermask;
    
    private AudioSource footsteps;

	// Use this for initialization
	void Start ()
    {
        walking_velocity *= 10;
        jump_height *= 100;
        
        footsteps = GetComponent<AudioSource>();
        animation_controller = GetComponent<Animator>();
        character_controller = GetComponent<CharacterController>();

        claire_RB = gameObject.GetComponent(typeof(Rigidbody)) as Rigidbody;
        if (claire_RB == null) {
            Debug.Log("You may have deleted Claire's Rigid Body");
        }

        // Set the center of mass height to 0
        Vector3 CoM = claire_RB.centerOfMass;
        CoM.y = 0;
        claire_RB.centerOfMass = CoM;

        // Animation states, used in the animation controller
        states = new Dictionary<string, int>() {
                {"idle", 0},
                {"walkingForward", 1},
                {"walkingBackward", 2},
                {"runningForwards", 3},
                {"jumping", 4},
            };

        // Controls
        jumping_key = KeyCode.Space;
        forwards_keys  = new KeyCode[] {KeyCode.UpArrow,    KeyCode.W};
        backwards_keys = new KeyCode[] {KeyCode.DownArrow,  KeyCode.S};
        left_keys      = new KeyCode[] {KeyCode.LeftArrow,  KeyCode.A};
        right_keys     = new KeyCode[] {KeyCode.RightArrow, KeyCode.D};
        running_keys   = new KeyCode[] {KeyCode.RightShift, KeyCode.LeftShift};

        forwards  = false;
        backwards = false;
        left      = false;
        right     = false;
        running   = false;
        jumping   = false;
        moving    = false;
    }

    void FixedUpdate() {
        // Moves the character forwards, and also handles jumping
        if(!PauseScript.isPaused) {
            HandleMovementRB();
        }
    }

    // Implements a more RPG style movement using RigidBody
    void HandleMovementRB() {
        Vector3 movement;

        if (moving) {
            // determine the movement direction + speed
            if (animation_controller.GetCurrentAnimatorStateInfo(0).IsName("Jump")) {
                // Player can't rotate but should still be able to control the movement direction
                Vector3 moveAngle = getRotation(_camera.transform.rotation.eulerAngles);
                float xdirection = Mathf.Sin(Mathf.Deg2Rad * moveAngle.y);
                float zdirection = Mathf.Cos(Mathf.Deg2Rad * moveAngle.y);

                movement = walking_velocity * new Vector3(xdirection, 0, zdirection);

                // make them slower if they're not moving forward
                if (Vector3.Dot( transform.forward.normalized, movement.normalized ) < 0.1) {
                    movement *= 0.6f;
                }
            } else {
                // Player can rotate, so just move forward
                movement = walking_velocity * transform.forward;
            }

            // Make them faster if they're running
            if (running) {
                movement *= 2;
            }

            // Make movement proportional to the elapsed time
            movement *= Time.deltaTime;
            

            // If claire isn't moving in a direction, 'accelerate' in that direction
            float vx = Mathf.Pow(claire_RB.velocity.x, 2);
            float vz = Mathf.Pow(claire_RB.velocity.z, 2);
            float vel = Mathf.Sqrt(vx + vz);
            
            if (vel < walking_velocity * Time.deltaTime / 4) {
                // Add velocity - simulating acceleration
                // Debug.Log("Going Slow");
                claire_RB.velocity += movement / 10;

            } else {
                // set velocity
                // Preserve the y-component (for jumping)
                movement.y = claire_RB.velocity.y;
                // Update the velocity
                claire_RB.velocity = movement;
            }

            
            
        }

        // Jump height as a vector
        Vector3 height = transform.up * jump_height * Time.deltaTime;

        // If in the air, accelerate to the ground
        if (! isGrounded()) {
            // Already jumping => deccelerate
            claire_RB.AddForce(-height / 3, ForceMode.Acceleration);

        // If on the ground and jumping, accelerate to the sky
        } else if (jumping) {
            // VelocityChange makes the jumps more satisfying
            claire_RB.AddForce(height, ForceMode.VelocityChange);
        }
        // Can add a different types of force (ForceMode): 
        // Force, Acceleration, Impulse, or VelocityChange

    }

    // Update is called once per frame
    void Update()
    {
        if(!PauseScript.isPaused) {
            // Get the relevant keys pressed to determine type of movement
            jumping   = Input.GetKey(jumping_key);
            running   = CheckKeys(running_keys);
            forwards  = CheckKeys(forwards_keys);
            backwards = CheckKeys(backwards_keys);
            left      = CheckKeys(left_keys);
            right     = CheckKeys(right_keys);
            moving    = forwards || backwards || left || right;

            // Play the sound if we're moving on the ground
            if (moving && isGrounded() && !footsteps.isPlaying) {
                footsteps.volume = Random.Range(0.8f, 1);
                footsteps.pitch  = Random.Range(1.3f, 1.7f);
                footsteps.Play();
            }

            // Rotates and animates the character based on which buttons are pressed
            HandleAnimationAndRotation();
        }
    }

    // Implements a more RPG style movement based on keyboard input
    void HandleAnimationAndRotation() {
        Vector3 rotation = transform.rotation.eulerAngles;
        // Align the rotation with the camera, so we rotate/move relative to the camera
        rotation.y = _camera.transform.rotation.eulerAngles.y;

        if (moving) {
            // HACKY: set the rotation (w/o rotating the camera or inventory)
            if (!animation_controller.GetCurrentAnimatorStateInfo(0).IsName("Jump")) {
                // inventory.parent = null; // Prevents inventory from rotating with the camera
                _camera.transform.parent = null;

                transform.eulerAngles = getRotation(rotation);

                _camera.transform.parent = transform;
                // inventory.parent = transform;
            }
        }

        // set the animation state
        if (jumping) {
            animation_controller.SetInteger("state", states["jumping"]);
        } else if (running && moving) {
            animation_controller.SetInteger("state", states["runningForwards"]);
        } else if (moving) {
            animation_controller.SetInteger("state", states["walkingForward"]);
        } else {
            animation_controller.SetInteger("state", states["idle"]);
        }

    }

    Vector3 getRotation(Vector3 startRotation) {
        Vector3 endRotation = startRotation;

        // Determines what angle the character will be facing
        if (forwards && right) {
            endRotation.y += 45;
        } else if (forwards && left) {
            endRotation.y -= 45;
        } else if (backwards && right) {
            endRotation.y += 135;
        } else if (backwards && left) {
            endRotation.y -= 135;
        } else if (forwards) {
            // no change to the angle
        } else if (backwards) {
            endRotation.y += 180;
        } else if (right) {
            endRotation.y += 90;
        } else if (left) {
            endRotation.y -= 90;
        }

        return endRotation;
    }

    // Checks if at least one key in the list is pressed
    bool CheckKeys(KeyCode[] keys) {
        foreach (KeyCode key in keys) {
            if (Input.GetKey(key)) {
                return true;
            }
        }
        return false;
    }

    // Cast a ray downwards from the player (height adjusted slightly)
    // If it hits, we're on top of something and therefore grounded
    bool isGrounded() {
        return Physics.Raycast(transform.position + Vector3.up * 0.02f, -Vector3.up, 0.05f);
    }

}
