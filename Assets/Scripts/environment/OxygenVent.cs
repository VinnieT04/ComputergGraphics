using UnityEngine;

public class OxygenVent : MonoBehaviour
{
    public float holdTime = 3f;
    float holdTimer = 0f;
    bool playerInRange = false;
    //bool isRefilling = false; //for UI later

    PlayerStats playerStats;

    [Header("Audio")]
    public AudioClip healingSound;
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
            //isRefilling = true;   //UI later
            Debug.Log($"Refilling... {holdTimer:F1} / {holdTime:F1}");

            if (holdTimer >= holdTime)
            {
                playerStats.RefillOxygen(playerStats.maxOxygen);
                holdTimer = 0f;
                //isRefilling = false;  //UI later
                Debug.Log("Oxygen fully refilled.");

                if (audioSource != null && healingSound != null)
                    audioSource.PlayOneShot(healingSound);
            }
        }
        else
        {
            holdTimer = 0f;
            //isRefilling = false;  //UI later
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerStats = other.GetComponent<PlayerStats>();
            playerInRange = true;
            Debug.Log("Near oxygen vent. Hold E to refill.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            holdTimer = 0f;
            //isRefilling = false;  //UI later
            Debug.Log("Left oxygen vent range.");
        }
    }
}