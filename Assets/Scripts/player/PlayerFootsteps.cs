using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip[] rockRunClips;
    public AudioClip[] rockWalkClips;
    public AudioClip[] rockJumpStartClips;
    public AudioClip[] rockJumpLandClips;

    [Header("Settings")]
    public float stepInterval = 0.4f;
    public float walkStepInterval = 0.6f;

    private AudioSource audioSource;
    private PlayerMovement playerMovement;
    private CharacterController controller;
    private float stepTimer;
    private bool wasGrounded;
    private bool wasDashing;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        bool isGrounded = controller.isGrounded;
        bool isMoving = new Vector3(controller.velocity.x, 0, controller.velocity.z).magnitude > 0.1f;
        bool isDashing = playerMovement.IsDashing;

        // Jump Start — detecta al presionar salto
        if (Input.GetButtonDown("Jump") && isGrounded)
            PlayJumpStart();

        // Double Jump — usa el mismo sonido de salto
        if (Input.GetButtonDown("Jump") && !isGrounded && !isDashing)
            PlayJumpStart();

        // Jump Land — detecta cuando toca el suelo después de estar en el aire
        if (!wasGrounded && isGrounded)
            PlayJumpLand();

        // Dash — usa sonido de run al iniciar
        if (!wasDashing && isDashing)
            PlayDash();

        wasGrounded = isGrounded;
        wasDashing = isDashing;

        // Footsteps — solo si está en el suelo, moviéndose y sin dash
        if (!isGrounded || !isMoving || isDashing)
        {
            stepTimer = 0f;
            return;
        }

        stepTimer -= Time.deltaTime;
        if (stepTimer <= 0f)
        {
            PlayFootstep();
            stepTimer = stepInterval;
        }
    }

    void PlayFootstep()
    {
        if (rockRunClips.Length == 0) return;
        audioSource.PlayOneShot(rockWalkClips[Random.Range(0, rockRunClips.Length)]);
    }

    public void PlayJumpStart()
    {
        if (rockJumpStartClips.Length == 0) return;
        audioSource.PlayOneShot(rockJumpStartClips[Random.Range(0, rockJumpStartClips.Length)]);
    }

    public void PlayJumpLand()
    {
        if (rockJumpLandClips.Length == 0) return;
        audioSource.PlayOneShot(rockJumpLandClips[Random.Range(0, rockJumpLandClips.Length)]);
    }

    void PlayDash()
    {
        if (rockRunClips.Length == 0) return;
        audioSource.PlayOneShot(rockRunClips[Random.Range(0, rockRunClips.Length)]);
    }
}