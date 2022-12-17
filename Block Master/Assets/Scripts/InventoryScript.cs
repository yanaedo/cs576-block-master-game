using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour {

    public float pickup_range   =  6f;
    public float pickup_force   = 50f;
    public float rotation_snap  = 45f; // 45 degrees = 8 positions per axis
    public float rotation_speed =  7f;
    // TODO: Get this programatically - knowing Claire's hierarchy
    public Camera _camera;      // The main camera, behind Claire
    public Camera _key_camera;  // The camera pointed at the inventory/key

    // Used to prevent collisions between the player and the inventory
    public  GameObject player;
    private Collider   player_collider;

    private Transform  holdArea;                     // Where the inventory is
    private GameObject held_object;                  // What is in the inventory
    private Rigidbody  held_object_RB;               // The inventory's rigid body
    private Transform  held_object_prev_parent;      // Keeps track of the key's position in the scene hierarchy
    private KeyCode rotation_key, switch_camera_key; // Control keys
    private bool cam_switched;  // Allows frame-buffering between button presses

    // Used so player can pick up an object through the enemy barrier
    [SerializeField] private LayerMask exclude_enemy_layermask;

    

	// Use this for initialization
	void Start ()
    {
        _camera.enabled = true;
        _key_camera.enabled = false;

        player_collider = player.GetComponent<Collider>();

        // This script is attached to the (empty) inventory
        holdArea = transform;
        held_object = null;

        // Controls
        rotation_key = KeyCode.R;
        switch_camera_key   = KeyCode.C;
        cam_switched = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Handle picking up (LeftClick), and dropping a key (RightClick)
        if (Input.GetMouseButtonDown(0) && held_object == null) {
            TryPickupKey();
        } else if (Input.GetMouseButtonDown(1) && held_object != null) {
            DropKey();
        }

        // Interact with the key - if we have one
        if (held_object != null) {

            // Make sure its in the desired position
            if (held_object != null && Vector3.Distance(held_object.transform.position, holdArea.position) > 0.05f) {
                Vector3 moveDirection = (holdArea.position - held_object.transform.position);
                held_object_RB.AddForce(moveDirection * pickup_force);
                // Debug.Log("Force Applied");
            }

            // Let the player rotate it, or snap it into position
            if (Input.GetKey(rotation_key)) {
                HandleRotateKey();
            } else {
                SnapRotation();
            }

            // Switch the camera (between character and key views) only if we have a key
            // If the held_object is ever null, we force 3rd person view in DropKey()
            if (!cam_switched && Input.GetKey(switch_camera_key)) {
                _camera.enabled = !_camera.enabled;
                _key_camera.enabled = !_key_camera.enabled;
                cam_switched = true;
            } else {
                cam_switched = false;
            }

        }
    }

    // IF the user clicked on a key (w/in range) pick it up
    void TryPickupKey() {
        // Following this tutorial: https://www.youtube.com/watch?v=6bFCQqabfzo

        // Construct a ray from the current mouse coordinates
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // If we hit an object check if its a key
        // The ray passes through layers NOT included in "exclude_enemy_layermask"
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, exclude_enemy_layermask)) {
            Transform  location   = hit.transform;
            GameObject pickup_obj = location.gameObject;
            Rigidbody  pickup_rb  = pickup_obj.GetComponent<Rigidbody>();

            bool in_range = Vector3.Distance(transform.position, location.position) < pickup_range;
            bool is_key   = pickup_obj.tag.ToLower().Contains("key");
            bool has_rb   = pickup_rb != null;  // Makes sure the key has an RB component so we don't error

            // If we've hit a key, pick it up
            if (in_range && is_key && has_rb) {
                held_object_RB = pickup_obj.GetComponent<Rigidbody>();
                held_object_RB.useGravity = false;
                held_object_RB.drag = 10;
                held_object_RB.constraints = RigidbodyConstraints.FreezeRotation; // Prevents unwanted rotations
                held_object_prev_parent = held_object_RB.transform.parent;
                held_object_RB.transform.parent = holdArea;
                held_object = pickup_obj;

                // Ignore collision with the key once we've picked it up.
                // **This prevents a bug where the key will push Claire**
                // It occurs because it is trying to move to its designated position by going through her, rather than around 
                Physics.IgnoreCollision(player_collider, pickup_obj.GetComponent<Collider>(), true);

                // Griffin: disabling this because I want key to be same size as hole it's being lined up with
                // Shrink the object when we pick it up - opposite of "grow" in DropKey()
                // Vector3 scale = held_object.transform.localScale;
                // held_object.transform.localScale = new Vector3(scale.x * 2 / 3, scale.y * 2 / 3, scale.z * 2 / 3);
            }
        }
    }

    // Removes the key from inventory and resets its default values
    void DropKey() {
        held_object_RB.useGravity = true;
        held_object_RB.drag = 1;
        held_object_RB.constraints = RigidbodyConstraints.None;
        held_object_RB.transform.parent = held_object_prev_parent;

        // Now that we've dropped the key, resume collisions
        Physics.IgnoreCollision(player_collider, held_object.GetComponent<Collider>(), false);

        // Grow the object when we drop it - opposite of "shrink" in TryPickupKey()
        // Vector3 scale = held_object.transform.localScale;
        // held_object.transform.localScale = new Vector3(scale.x * 3 / 2, scale.y * 3 / 2, scale.z * 3 / 2);
        
        held_object = null;

        // Enforce 3rd person view
        _camera.enabled = true;
        _key_camera.enabled = false;
    }

    // Rotates the key based on mouse movement
    void HandleRotateKey() {
        float forward = Input.GetAxis("Mouse Y");
        float right   = Input.GetAxis("Mouse X");
        execute_key_rotation( new Vector3(forward, 0, -right) * rotation_speed);

        // Alt approach (didn't work for some reason):
        // https://docs.unity3d.com/ScriptReference/Rigidbody.AddTorque.html
        // held_object_RB.AddTorque(rotation, ForceMode.Impulse);
        // ForceMode: Force, Acceleration, Impulse, VelocityChange
    }

    // Rotates the key around the holdArea
    void execute_key_rotation(Vector3 rotation) {
        Vector3 pos = held_object.transform.position;
        Vector3 x_axis = holdArea.right;
        Vector3 y_axis = holdArea.up;
        Vector3 z_axis = holdArea.forward;

        held_object.transform.RotateAround(pos, x_axis, rotation.x);
        held_object.transform.RotateAround(pos, y_axis, rotation.y);
        held_object.transform.RotateAround(pos, z_axis, rotation.z);

        // Alt. approach
        // This line functioned, but would only rotate [-90, 90], not continously
        // held_object.transform.localEulerAngles += rotation;
    }

    // Snap the held object to certain angles (based off of rotation_snap)
    void SnapRotation() {
        Vector3 curAngle = held_object.transform.localEulerAngles;
        Vector3 newAngle = new Vector3( Snap(curAngle.x), Snap(curAngle.y), Snap(curAngle.z));
        held_object.transform.localEulerAngles = newAngle;
    }

    // Uses interpolation to snap curAngle to a multiple of "rotation_snap" degrees
    private float Snap(float curAngle) {
        int nearest_multiple = Mathf.RoundToInt(curAngle / rotation_snap);
        float newAngle = nearest_multiple * rotation_snap;
        float delta = newAngle - curAngle;

        float finalAngle = Mathf.MoveTowardsAngle(curAngle, newAngle, 8 * rotation_speed * Time.deltaTime);

        return (delta < 0.1) ? curAngle : finalAngle;
    }

}
