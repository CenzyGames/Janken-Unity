using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RemoteThirdPersonSmooth : MonoBehaviour
{
	
    private Vector3 position;

    //private Vector3 velocity;
    private Quaternion rotation;
    Quaternion eulerY;
    private bool started;
    public float speed = 6.0f;
    Vector3 smoothMoveVelocity;
    Vector3 startpos;


    float lerpTime = 1f;
    float currentLerpTime;

    public void SetTransform(Vector3 pos, Vector3 rot, float eulerY)
    {
        started = true;
        position = pos;
        //velocity = vel;
        rotation.eulerAngles = rot;
        this.eulerY = Quaternion.Euler(new Vector3(0, eulerY, 0));
        startpos = transform.position;
    }

    public void SetTransform(Vector3 pos, float eulerY)
    {
        started = true;
        position = pos;
        this.eulerY = Quaternion.Euler(new Vector3(0, eulerY, 0));
        startpos = transform.position;

     
    }
	
    // Use this for initialization
    void Start()
    {
        started = false;
     
    }
	
    // Update is called once per frame
    void Update()
    {
        if (started == true)
        {


            // Move();
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            float t = currentLerpTime / lerpTime;
            t = t * t * t * (t * (6f * t - 15f) + 10f);
            //transform.position = position;
            //  transform.rotation = rotation;
           
            transform.position = Vector3.Lerp(startpos, position, t); 
           
            //transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime);
            // transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime);
            transform.FindChild("Capsule").transform.localRotation = eulerY;
        }
    }


    public void Move()
    {
        iTween.MoveTo(gameObject, iTween.Hash("position", position,
            "easeType", iTween.EaseType.linear,
            "islocal", true,
            "speed", speed,
            "oncomplete", "AnimationFinished"));  
    }

    void AnimationFinished()
    {
        Debug.Log("Finished");
        started = false;
       
    }
}
