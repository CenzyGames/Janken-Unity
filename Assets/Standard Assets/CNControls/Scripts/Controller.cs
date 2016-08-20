using UnityEngine;
using System.Collections;
using CnControls;


public class Controller : MonoBehaviour
{
	
    // public vars
    public float walkSpeed = 6;
    public LayerMask groundedMask;
    public  Transform planet;
    public float gravity = -9.8f;
	
    // System vars
    bool grounded;
    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;
    Transform cameraTransform;
    Rigidbody rigidbody;
    Vector3 gravityUp;



    void Awake()
    {
        //Initialize vars
        cameraTransform = Camera.main.transform;
        planet = GameObject.FindGameObjectWithTag("Planet").GetComponent<Transform>();
        rigidbody = GetComponent<Rigidbody>();

        // Disable rigidbody gravity and rotation as this is simulated manually
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }


    void Update()
    {
        //Joystick Input
        float inputX = CnInputManager.GetAxisRaw("Horizontal");
        float inputY = CnInputManager.GetAxisRaw("Vertical");

        //Calculate gravity direction
        gravityUp = (transform.position - planet.position).normalized;
        Vector3 localUp = transform.up;
        
       
       
        // Align player's up axis with the centre of planet
        transform.rotation = Quaternion.FromToRotation(localUp, gravityUp) * transform.rotation;
        //transform.rotation = Quaternion.FromToRotation(transform.forward, cameraTransform.TransformDirection(new Vector3(inputX, 0, inputY).normalized)) * transform.rotation;
      


        // Calculate movement
        Vector3 moveDir = new Vector3(inputX, 0, inputY).normalized;
        Vector3 targetMoveAmount = Vector3.zero;
        //   if (moveDir.sqrMagnitude > 0.001f)
        targetMoveAmount = moveDir * walkSpeed;
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);
		
		
        // Grounded check
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;
		
        if (Physics.Raycast(ray, out hit, 1 + .1f, groundedMask))
        {
            grounded = true;
        } else
        {
            grounded = false;
        }
		
    }

    void FixedUpdate()
    {
        // Apply downwards gravity to body
        rigidbody.AddForce(gravityUp * gravity);

        // Apply movement to rigidbody
        Vector3 localMove = transform.TransformDirection(moveAmount) * Time.fixedDeltaTime;
        rigidbody.MovePosition(rigidbody.position + localMove);
    }
}
