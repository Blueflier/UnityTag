using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject camHolder;
    private Transform target;
    private Vector2 move, look;
    private bool grounded, onWall, inAir;
    private float lookRotation;
/*    [SerializeField] GameObject groundedCube;*/
    [SerializeField] private LayerMask ground, wall;
    [SerializeField] private float speed, sens, maxForce, jumpForce, groundDistance;
    public Transform groundCheck, wallCheckL, wallCheckR;


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
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, ground);
        onWall = (Physics.CheckSphere(wallCheckL.position, groundDistance, wall) || Physics.CheckSphere(wallCheckR.position, groundDistance, wall)) && !grounded;
        if (onWall)
        {
            Debug.Log("On Wall");
            Physics.gravity = new Vector3(0, -3.0F, 0);
        }
        if(!onWall)
        {
            Physics.gravity = new Vector3(0, -15.81F, 0);
        }

        inAir = !grounded && !onWall;

        //MOVEMENT WASD
        if (grounded)
        {
            Vector3 currentVelocity = rb.velocity;
            Vector3 targetVelocity = new Vector3(move.x, 0, move.y);
            targetVelocity *= speed;
            targetVelocity = target.TransformDirection(targetVelocity);
            Vector3 velocityChange = (targetVelocity - currentVelocity);
            velocityChange = new Vector3 (velocityChange.x, 0, velocityChange.z);
            Vector3.ClampMagnitude(velocityChange, maxForce);
            rb.AddForce(velocityChange, ForceMode.VelocityChange);
        }
        
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
        if(onWall)
        {
            Vector3 currentVelocity = rb.velocity;
            Vector3 targetVelocity = new Vector3(0, 0, move.y);
            targetVelocity *= speed * 2;
            targetVelocity = target.TransformDirection(targetVelocity);
            Vector3 velocityChange = (targetVelocity - currentVelocity);
            velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z);
            Vector3.ClampMagnitude(velocityChange, maxForce * 2);
            rb.AddForce(velocityChange, ForceMode.Force);
        }

    }

    void LateUpdate()
    {
        //Turn
        target.Rotate(Vector3.up * look.x * sens);
        //Look
        lookRotation += (-look.y * sens);
        lookRotation = Mathf.Clamp(lookRotation, -90, 90);
        camHolder.transform.eulerAngles = new Vector3(lookRotation, camHolder.transform.eulerAngles.y, camHolder.transform.eulerAngles.z);
    }
        private void Jump()
    {
        if (grounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        if (onWall)
        {

            if (Physics.CheckSphere(wallCheckL.position, groundDistance, wall)) // checkLeft
            {

                Vector3 right = new Vector3(2,1,0);
                right = target.transform.TransformDirection(right);
                rb.AddForce(right * jumpForce, ForceMode.Impulse);
            }
            else if (Physics.CheckSphere(wallCheckR.position, groundDistance, wall)) //checkRight
            {
                Vector3 left = new Vector3(-2, 1, 0);
                left = target.transform.TransformDirection(left);

                rb.AddForce(left * jumpForce, ForceMode.Impulse);
            }
            /*rb.AddForce*/
        }
    }

}
