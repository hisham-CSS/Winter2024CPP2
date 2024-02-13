using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float gravity = 9.81f;
    public float jumpSpeed = 10.0f;
    public LayerMask enemyCheck;

    private CharacterController cc;
    private Animator anim;
    private Player playerInput;
    private Health playerHealth;

    private Vector2 moveInput;

    public float YVelocity;

    private void Awake()
    {
        playerInput = new Player();

        playerInput.Console.Move.performed += MovePerformed;
        playerInput.Console.Jump.performed += ctx => JumpPressed();
        playerInput.Console.Attack.performed += ctx => AttackPressed();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();        
    }

    void JumpPressed()
    {
        if (cc.isGrounded)
            YVelocity = jumpSpeed;
    }

    void AttackPressed()
    {
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);

        if (clipInfo[0].clip.name != "CrossPunch")
            anim.SetTrigger("Attack");
        
    }

    void MovePerformed(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();

        //set our animation and move
        anim.SetFloat("Speed", moveInput.magnitude);
    }

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            cc = GetComponent<CharacterController>();
            anim = GetComponentInChildren<Animator>();
            playerHealth = GetComponent<Health>();

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
    }

    // Update is called once per frame
    void Update()
    {
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
        Vector3 desiredMoveDirection = cameraForward * moveInput.y + cameraRight * moveInput.x;

        //when we are not moving - we do not rotate
        if (desiredMoveDirection.magnitude > 0)
        {
            //Spherical rotation towards our desired move direction
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), 0.1f);
        }

        //add a speed to our direction so we do not just use normalized values - because we are in update we need to multiply by Time.deltaTime
        desiredMoveDirection *= speed * Time.deltaTime;

        //Y Velocity is set to zero if we are grounded - otherwise add gravity
        YVelocity = (!cc.isGrounded) ? YVelocity -= gravity * Time.deltaTime : 0;

        //set our gravity
        desiredMoveDirection.y = YVelocity;

        ////set our an move
        cc.Move(desiredMoveDirection);


        //Ray ray = new Ray(transform.position, transform.forward);
        //RaycastHit hitInfo;

        //Debug.DrawLine(transform.position, (transform.position) + (transform.forward) * 10.0f, Color.red);
        //if (Physics.Raycast(ray, out hitInfo, 10.0f, enemyCheck))
        //{
        //    Debug.Log(hitInfo);
        //}
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Healing"))
        {
            playerHealth.IncreaseHealth(1);
        }
    }

    private void OnDestroy()
    {
        //GameOver logic goes here
    }
}
