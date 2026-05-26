using NUnit.Framework;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f;

    public float jumpHeight = 3f;

    // Coyote Time
    public float coyoteTime = 0.15f;
    float coyoteTimer;

    // Jump Buffer
    public float jumpBufferTime = 0.15f;
    float jumpBufferTimer;


    // Dash
    public float dashSpeed = 25f;
    public float dashTime = 0.2f;

    float dashTimer;
    bool isDashing;

    public int maxDashes = 1;
    int currentDashes;

    float launchTimer;

    Vector3 velocity;
    bool isGrounded;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame

    void Start()
    {
        
    }
    void Update()
    {
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isGrounded = controller.isGrounded;

        if (isGrounded)
        {
            coyoteTimer = coyoteTime;
            currentDashes = maxDashes;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }

        if (!isGrounded && Input.GetKeyDown(KeyCode.LeftShift) && currentDashes > 0)
        {
            isDashing = true;
            dashTimer = dashTime;
            currentDashes--;
        }

        // Jump buffer input
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferTimer = jumpBufferTime;
        }
        else
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -1f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Jump condition
        if (coyoteTimer > 0f && jumpBufferTimer > 0f)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            // Reset timers so it doesn't double trigger
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
        }

        if (Input.GetButtonUp("Jump") && velocity.y > 0)
        {
            velocity.y *= 0.7f;
        }

        
        

        Vector3 move = transform.right * x + transform.forward * z;
        Vector3 finalMovementVector;

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;

            Vector3 dashDirection = move.normalized;
            dashDirection.y *= 0.3f; // reduce vertical influence
            dashDirection = dashDirection.normalized;
            
            if(dashDirection == Vector3.zero)
            {
                dashDirection = transform.forward;
            }

            finalMovementVector = dashDirection * dashSpeed;
            finalMovementVector.y = velocity.y +2f;

            if(dashTimer <= 0f)
            {
                isDashing = false;
            }
        }
        else
        {
            if (launchTimer > 0)
            {
                launchTimer -= Time.deltaTime;
            }
            else
            {
                velocity.y += gravity * Time.deltaTime;
            }
            finalMovementVector = move * speed + velocity;
        }
        controller.Move(finalMovementVector * Time.deltaTime);
    }

    public void Launch(Vector3 force)
    {
        velocity.y = force.y;
        Vector3 horizontal = new Vector3(force.x, 0, force.z);
        controller.Move(horizontal * Time.deltaTime);

        launchTimer = 0.2f; // duration of launch
        Debug.Log("LAUNCH CALLED: " + force);
    }
}
