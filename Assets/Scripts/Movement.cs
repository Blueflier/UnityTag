using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody rb;
    public GameObject camHolder;
    private Transform target;
    private Vector2 move, look;
    private bool grounded = true;
    private float lookRotation;
    [SerializeField] private float speed, sens, maxForce;

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
        //Find target velocity
        Vector3 currentVelocity = rb.velocity;
        Vector3 targetVelocity = new Vector3(move.x, 0, move.y);
        targetVelocity *= speed;
        //Align direction
        targetVelocity = target.TransformDirection(targetVelocity);
        //Calculate forces
        Vector3 velocityChange = (targetVelocity - currentVelocity);
        velocityChange = new Vector3 (velocityChange.x, 0, velocityChange.z);
        //Limit force
        Vector3.ClampMagnitude(velocityChange, maxForce);
        rb.AddForce(velocityChange, ForceMode.VelocityChange);

    }

    void LateUpdate()
    {
        //Turn
        target.Rotate(Vector3.up * look.x * sens);
        //Look
        lookRotation += (-look.y * sens);
        camHolder.transform.eulerAngles = new Vector3(lookRotation, camHolder.transform.eulerAngles.y, camHolder.transform.eulerAngles.z);
    }
        private void Jump()
    {
        if (grounded)
        {
            rb.AddForce(Vector3.up, ForceMode.Impulse);
        }
    }

}
