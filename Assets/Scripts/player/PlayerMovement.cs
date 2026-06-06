using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float doubleJumpMultiplier = 0.65f;
    bool hasDoubleJump;

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
    public bool IsDashing => isDashing;
    public int maxDashes = 1;
    int currentDashes;
    float launchTimer;

    // Inertia
    public float groundFriction = 8f;   // higher = snappier, lower = icier
    public float airControl = 0.3f;     // 0 = no air control, 1 = full control
    Vector3 currentMove;                // the smoothed movement vector

    // Damaged state
    public bool isDamaged = true;       // starts true, repair station sets to false
    public float damagedSpeedMultiplier = 0.5f;

    Vector3 velocity;
    bool isGrounded;

    void Start() { }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))    //testing Damaged state. Press T to change Hurt/Unhurt movement of player
        isDamaged = !isDamaged;

        isGrounded = controller.isGrounded;

        if (isGrounded)
        {
            coyoteTimer = coyoteTime;
            currentDashes = maxDashes;
            hasDoubleJump = true;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }

        if (!isDamaged && !isGrounded && Input.GetKeyDown(KeyCode.LeftShift) && currentDashes > 0)
        {
            isDashing = true;
            dashTimer = dashTime;
            currentDashes--;
        }

        // Jump buffer
        if (Input.GetButtonDown("Jump"))
            jumpBufferTimer = jumpBufferTime;
        else
            jumpBufferTimer -= Time.deltaTime;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Raw input direction — used for dash (where you intend to go)
        Vector3 targetMove = transform.right * x + transform.forward * z;

        // In air: lerp slower (harder to redirect) but toward full speed target
        float lerpSpeed = isGrounded ? groundFriction : groundFriction * airControl;
        currentMove = Vector3.Lerp(currentMove, targetMove, Time.deltaTime * lerpSpeed);

        // Jump
        if (coyoteTimer > 0f && jumpBufferTimer > 0f)
        {
            float currentJumpHeight = isDamaged ? jumpHeight * 0.4f : jumpHeight;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
        }
        else if (!isDamaged && hasDoubleJump && jumpBufferTimer > 0f && !isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * doubleJumpMultiplier * -2f * gravity);
            jumpBufferTimer = 0f;
            hasDoubleJump = false;
        }

        if (Input.GetButtonUp("Jump") && velocity.y > 0)
            velocity.y *= 0.7f;

        Vector3 finalMovementVector;

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;

            // Dash uses targetMove (raw input) not currentMove, so it feels responsive
            Vector3 dashDirection = targetMove.normalized;
            dashDirection.y *= 0.3f;
            dashDirection = dashDirection.normalized;

            if (dashDirection == Vector3.zero)
                dashDirection = transform.forward;

            finalMovementVector = dashDirection * dashSpeed;
            finalMovementVector.y = velocity.y + 2f;

            if (dashTimer <= 0f)
                isDashing = false;
        }
        else
        {
            if (launchTimer > 0)
                launchTimer -= Time.deltaTime;
            else
                velocity.y += gravity * Time.deltaTime;

            // Use currentMove (smoothed) for normal movement
            finalMovementVector = currentMove * speed + velocity;
        }

        controller.Move(finalMovementVector * Time.deltaTime);
    }

    public void Launch(Vector3 force)
    {
        velocity.y = force.y;
        Vector3 horizontal = new Vector3(force.x, 0, force.z);
        controller.Move(horizontal * Time.deltaTime);
        launchTimer = 0.2f;
        Debug.Log("LAUNCH CALLED: " + force);
    }
}