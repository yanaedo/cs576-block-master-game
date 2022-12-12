using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyslot : MonoBehaviour
{
    //TODO making parameters that store the desired angle and the tolorance around them, and a parameter that stores the corresponding door to open when there's a key of the right shape and angle in the region 
    // Start is called before the first frame update
    public string keyTag = "Cube Key";
    // float target_rotate_x = 0f;
    // float target_rotate_y = 0f;
    // float target_rotate_z = 0f;
    public Quaternion target_rotation;
    public float angle_tolerance = 5f;

    public bool activated;
    private Vector3 target_eulerAngles;

    void Start()
    {
        target_eulerAngles = target_rotation.eulerAngles;
        target_eulerAngles = new Vector3(Mathf.Repeat(target_eulerAngles.x,90f),Mathf.Repeat(target_eulerAngles.y,90f),Mathf.Repeat(target_eulerAngles.z,90f));
        activated = false;
    }

    // Update is called once per frame
    void Update()
    {
        // activated = false;
    }

    void OnTriggerStay(Collider other)
    {
        // if (other.gameObject.getComponent<>)
        // print(other.tag);
        // print(keyTag);
        activated = false;
        if (other.CompareTag(keyTag)) {
            // print(other.transform.rotation);
            // print(other.parent.transform.rotation);
            Vector3 other_eulerAngles = other.transform.rotation.eulerAngles;
            // print(other_eulerAngles);
            // print(other_eulerAngles.x);

            //TODO make work for other types of rotational symmetry, just checking along all possible 90 degree rotations since a cube turned 90 degrees in any direction is identical
            float xAngle = Mathf.Repeat(other_eulerAngles.x,90f);
            float yAngle = Mathf.Repeat(other_eulerAngles.y,90f);
            float zAngle = Mathf.Repeat(other_eulerAngles.z,90f);
            

            // print(xAngle);
            // print(yAngle);
            // print(zAngle);
            float dx = Mathf.Abs(xAngle - target_eulerAngles.x);
            dx = Mathf.Min(dx, 90f - dx);
            float dy = Mathf.Abs(yAngle - target_eulerAngles.y);
            dy = Mathf.Min(dy, 90f - dy);
            float dz = Mathf.Abs(zAngle - target_eulerAngles.z);
            dz = Mathf.Min(dz, 90f - dz);

            if (    dx < angle_tolerance && 
                    dy < angle_tolerance && 
                    dz < angle_tolerance) {
            // if (    Mathf.Abs(Mathf.Repeat(other_eulerAngles.x - target_eulerAngles.x - 45f,90f) + 45f) < angle_tolerance && 
            //         Mathf.Abs(Mathf.Repeat(other_eulerAngles.y - target_eulerAngles.y - 45f,90f) + 45f) < angle_tolerance && 
            //         Mathf.Abs(Mathf.Repeat(other_eulerAngles.z - target_eulerAngles.z - 45f,90f) + 45f) < angle_tolerance) {
                // print(other_eulerAngles);
                activated = true;
            }
            
            // for (float dx = -360f; dx <= 360f; dx += 90f) {
            //     if (activated) {
            //         break;
            //     }
            //     for (float dy = -360f; dy <= 360f; dy += 90f) {
            //         if (activated) {
            //             break;
            //         }
            //         for (float dz = -360f; dz <= 360f; dz += 90f) {
            //             print(other_eulerAngles.x + dx - target_eulerAngles.x);
            //             if (    Mathf.Abs(other_eulerAngles.x + dx - target_eulerAngles.x) < angle_tolerance &&
            //                     Mathf.Abs(other_eulerAngles.y + dy - target_eulerAngles.y) < angle_tolerance &&
            //                     Mathf.Abs(other_eulerAngles.z + dz - target_eulerAngles.z) < angle_tolerance) {
            //                 activated = true;
            //                 break;
            //             }
            //         }
            //     }
            // }
            // print(other_eulerAngles);
            // print(target_eulerAngles);
            // float angle = Quaternion.Angle(other.transform.rotation, target_rotation);
            // if (angle < angle_tolerance) {
            // print(target_rotation);
            // print(angle);
            // }
        }
        // print(activated);
    }

    void OnTriggerExit( Collider other ) {
       activated = false;
    }

}
