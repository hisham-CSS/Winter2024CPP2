using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float gravity = 9.81f;
    public float jumpSpeed = 10.0f;
    public LayerMask enemyCheck;

    CharacterController cc;
    Animator anim;

    public float YVelocity;

    //Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            cc = GetComponent<CharacterController>();
            anim = GetComponentInChildren<Animator>();

            if (speed < 0)
            {
                speed = 10;
                throw new ArgumentException("Default value has been set for speed");
            }
        }
        catch(NullReferenceException e)
        {
            Debug.Log(e.ToString());
        }
        catch (ArgumentException e)
        {
            Debug.Log(e.ToString());
        }
        
        //rb = GetComponent<Rigidbody>();
        //GameManager.Instance.TestGameManager();
    }

    // Update is called once per frame
    void Update()
    {
        //YVelocity = cc.velocity.y;

        //Grabbing our inputs
        float hInput = Input.GetAxis("Horizontal");
        float fInput = Input.GetAxis("Vertical");

        //making a dir value to be used for animation
        Vector3 dir = new Vector3(hInput, 0, fInput);

        //grabing camera foward and right vectors for camera relative movement
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        //removing yaw rotation
        cameraForward.y = 0;
        cameraRight.y = 0;

        //normalize our vectors because they may affect our speed when at camera extents
        cameraForward.Normalize();
        cameraRight.Normalize();

        //Vector projection formula 
        Vector3 desiredMoveDirection = cameraForward * fInput + cameraRight * hInput;

        //when we are not moving - we do not rotate
        if (desiredMoveDirection.magnitude > 0)
        {
            //Spherical rotation towards our desired move direction
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), 0.1f);
        }

        //add a speed to our direction so we do not just use normalized values - because we are in update we need to multiply by Time.deltaTime
        desiredMoveDirection *= speed * Time.deltaTime;
        //desiredMoveDirection.y -= gravity;

        //Y Velocity is set to zero if we are grounded - otherwise add gravity
        YVelocity = (!cc.isGrounded) ? YVelocity -= gravity * Time.deltaTime : 0;

        //if we jump affect y velocity again
        if (cc.isGrounded && Input.GetButtonDown("Jump"))
        {
            //moveInput.y = jumpSpeed;
            YVelocity = jumpSpeed;
        }

        //
        desiredMoveDirection.y = YVelocity;

        anim.SetFloat("Speed", dir.magnitude);
        cc.Move(desiredMoveDirection);


        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        Debug.DrawLine(transform.position, (transform.position) + (transform.forward) * 10.0f, Color.red);
        if (Physics.Raycast(ray, out hitInfo, 10.0f, enemyCheck))
        {
            Debug.Log(hitInfo);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
    }
}
