using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

    //public vars
    public float cameraDistOffset;
    public GameObject Target;

    // Use this for initialization
    void Start()
    {
	
    }
	
    // Update is called once per frame
    void Update()
    {

        Vector3 playerInfo = Target.transform.position;

        //Align camera with player with offset along Y axis
        transform.position = playerInfo + (Target.transform.up * cameraDistOffset);
        transform.LookAt(Target.transform);

    }
}
