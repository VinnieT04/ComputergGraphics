using UnityEngine;

public class RepairStation : MonoBehaviour
{
    public static Vector3 respawnPoint; // shared across all repair stations

    public float holdTime = 3f;
    float holdTimer = 0f;
    bool playerInRange = false;

    PlayerStats playerStats;
    PlayerMovement playerMovement;

    [Header("Audio")]
    public AudioClip repairSound;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKey(KeyCode.E))
        {
            holdTimer += Time.deltaTime;
            Debug.Log($"Repairing... {holdTimer:F1} / {holdTime:F1}");

            if (holdTimer >= holdTime)
            {
                playerStats.RepairSuit();
                playerMovement.isDamaged = false;
                respawnPoint = transform.position;  // save this as respawn point
                holdTimer = 0f;
                Debug.Log("Suit repaired. Respawn point saved.");

                if (audioSource != null && repairSound != null)
                    audioSource.PlayOneShot(repairSound);
            }
        }
        else
        {
            holdTimer = 0f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerStats = other.GetComponent<PlayerStats>();
            playerMovement = other.GetComponent<PlayerMovement>();
            playerInRange = true;
            Debug.Log("Near repair station. Hold E to repair.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            holdTimer = 0f;
        }
    }
}