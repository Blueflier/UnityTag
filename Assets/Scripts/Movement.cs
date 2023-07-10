using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    private Rigidbody rb;
    public GameObject camHolder;
    private Transform target;
    private Vector2 move, look;
    
    private float lookRotation;
    public PauseMenu PauseMenu;
    
    public enum MovementState
    {
        still,
        walking,
        running,
        wallrunning,
        climbing,
        sliding,
        inair
    }

    public MovementState state;

    private bool grounded, inAir, wallrunning;
    
    [SerializeField] private LayerMask ground, wall;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float sens = 0.1f;
    [SerializeField] private float maxForce = 5.0f;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float groundDistance = 0.1f;
    

    [Header("States")]
    /*private bool walking, running, wallrunning, */

    [Header("Wallrunning")]
    //how far from the wall you have to be to wallrun
    [SerializeField] private float wallCheckDistance = 0.5f;
    //minimum height you can wallrun at (uses raycast)
    [SerializeField] private float minJumpHeight = 0.5f;
    private RaycastHit rightHit;
    private RaycastHit leftHit;
    private bool wallR, wallL;
    [SerializeField] private float wallrunForce;


    public void OnMove(InputAction.CallbackContext ctx)
    {
        move = ctx.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        look = ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        Jump();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        target = GetComponent<Transform>();
    }
    private void FixedUpdate()
    {

        checkForWall();
        AboveGround();
        stateMachine();
        stateHandler();
        isGrounded();

        //DO RAYCAST FOR GROUNDED AND MOVE IT INTO A FUNCTION

        /*grounded = Physics.CheckSphere(groundCheck.position, groundDistance, ground);*/

        inAir = !grounded && !wallrunning;

        movePlayer();
        

    }

    public void movePlayer()
    {
        if (wallrunning)
        {
            wallRunMovement();
        }


        if (grounded)
        {
            Vector3 currentVelocity = rb.velocity;
            Vector3 targetVelocity = new Vector3(move.x, 0, move.y);
            targetVelocity *= speed;
            targetVelocity = target.TransformDirection(targetVelocity);
            Vector3 velocityChange = (targetVelocity - currentVelocity);
            velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z);
            Vector3.ClampMagnitude(velocityChange, maxForce);
            rb.AddForce(velocityChange, ForceMode.VelocityChange);
        }

        //movement while in air - applies force not velocitychange
        if (inAir)
        {
            Vector3 currentVelocity = rb.velocity;
            Vector3 targetVelocity = new Vector3(move.x, 0, move.y);
            targetVelocity *= speed;
            targetVelocity = target.TransformDirection(targetVelocity);
            Vector3 velocityChange = (targetVelocity - currentVelocity);
            velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z);
            Vector3.ClampMagnitude(velocityChange, maxForce * 2);
            rb.AddForce(velocityChange, ForceMode.Force);
        }


        //when on wall you can jump but you jump away from the wall and up
        /*if (onWall)
        {
            Vector3 currentVelocity = rb.velocity;
            Vector3 targetVelocity = new Vector3(0, 0, move.y);
            targetVelocity *= speed * 2;
            targetVelocity = target.TransformDirection(targetVelocity);
            Vector3 velocityChange = (targetVelocity - currentVelocity);
            velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z);
            Vector3.ClampMagnitude(velocityChange, maxForce * 2);
            rb.AddForce(velocityChange, ForceMode.Force);
        }*/
    }

    void LateUpdate()
    {
        //Look
        playerLook();
    }

    private void playerLook()
    {
        if (!PauseMenu.isGamePaused)
        {
            target.Rotate(Vector3.up * look.x * sens);
            lookRotation += (-look.y * sens);
            lookRotation = Mathf.Clamp(lookRotation, -90, 90);
            camHolder.transform.eulerAngles = new Vector3(lookRotation, camHolder.transform.eulerAngles.y, camHolder.transform.eulerAngles.z);
        }
    }
        
    private void Jump()
    {
        if (grounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (wallrunning)
        {

            if (wallL) // checkLeft
            {

                Vector3 right = new Vector3(2,1,0);
                right = target.transform.TransformDirection(right);
                rb.AddForce(right * jumpForce, ForceMode.VelocityChange);
            }
            else if (wallR) //checkRight
            {
                Vector3 left = new Vector3(-2, 1, 0);
                left = target.transform.TransformDirection(left);
                rb.AddForce(left * jumpForce, ForceMode.VelocityChange);
            }
            /*rb.AddForce*/
        }
    }

    private void isGrounded()
    {
        grounded =  Physics.Raycast(transform.position, Vector3.down, groundDistance, ground);
    }

    private void checkForWall()
    {
        wallR = Physics.Raycast(transform.position, target.right, out rightHit, wallCheckDistance, wall);
        wallL = Physics.Raycast(transform.position, -target.right, out leftHit, wallCheckDistance, wall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, ground);
    }

    private void stateMachine()
    {
        if ((wallL || wallR) && Input.GetAxisRaw("Vertical") > 0 && AboveGround())
        {
            if(!wallrunning)
            {
                startWallRun();
            }
        }

        else
        {
            stopWallRun();
        }
    }

    private void startWallRun()
    {
        wallrunning = true;
    }

    private void wallRunMovement()
    {
        rb.useGravity= false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        Vector3 wallNormal = !wallR ? leftHit.normal : rightHit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((target.forward - wallForward).magnitude > (target.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        rb.AddForce(wallForward * wallrunForce, ForceMode.Force);
    }

    private void stopWallRun()
    {
        wallrunning = false;
        rb.useGravity= true;
    }


    private void stateHandler()
    {
        if (wallrunning)
        {
            state = MovementState.wallrunning;
        }
    }

}
