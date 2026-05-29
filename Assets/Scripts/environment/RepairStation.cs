using UnityEngine;

public class RepairStation : MonoBehaviour
{
    public float holdTime = 3f;
    float holdTimer = 0f;
    bool playerInRange = false;

    PlayerStats playerStats;

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
                holdTimer = 0f;
                Debug.Log("Suit fully repaired. Abilities restored.");
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
            Debug.Log("Left repair station range.");
        }
    }
}
