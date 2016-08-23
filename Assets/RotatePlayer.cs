using UnityEngine;
using System.Collections;

public class RotatePlayer : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
	
    }
	
    // Update is called once per frame
    void Update()
    {
        //Pull the rotation direction from Controller script
        Vector3 rotDir = transform.parent.GetComponent<Controller>().rotationDir;
        if (rotDir != Vector3.zero)
        {
            //Calculate amount of rotation in degrees
            Quaternion newRot = Quaternion.LookRotation(rotDir);

            //Apply Rotation and lock rotation along X and Z axis
            transform.localRotation = Quaternion.Euler(0, newRot.eulerAngles.y, 0);
        }
    }
}
