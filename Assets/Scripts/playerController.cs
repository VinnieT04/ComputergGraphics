using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("Mouse Look")]
    [Range(50f, 500f)]
    public float mouseSensitivity = 300f;       
    public float verticalLookLimit = 89f;        
    public Transform cameraTransform;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;

    // Track if we are touching a safe platform or the normal ground
    private bool isOnSafeGround = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleJump();
        ApplyGravity();

        // Reset our safe ground flag every frame so it has to be continuously proven true
        if (!controller.isGrounded)
        {
            isOnSafeGround = false;
        }
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalLookLimit, verticalLookLimit);
        
        if (cameraTransform != null)
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal"); 
        float v = Input.GetAxis("Vertical");   

        Vector3 move = transform.right * h + transform.forward * v;
        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    void HandleJump()
    {
        // The player can jump if Unity says they are grounded on flat terrain,
        // OR if they are specifically standing on a "Platform" labeled object.
        if ((controller.isGrounded || isOnSafeGround) && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isOnSafeGround = false; // Reset immediately upon jumping
        }

        if (controller.isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f; 
        }
    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // This built-in Unity function detects exactly what the Character Controller is stepping on
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Check if the collision is happening beneath our feet (normal vector pointing up)
        if (hit.normal.y > 0.6f) 
        {
            // If the object has our "Platform" label, authorize jumping!
            if (hit.gameObject.CompareTag("jumpable"))
            {
                isOnSafeGround = true;
            }
            // If they step back onto the regular flat ground outside the crater, that's safe too
            else if (controller.isGrounded && !hit.gameObject.CompareTag("jumpable"))
            {
                // If it's the steep mountain wall, 'controller.isGrounded' will automatically 
                // be false because of your Slope Limit, keeping the walls unsafe!
                isOnSafeGround = false;
            }
        }
    }
}