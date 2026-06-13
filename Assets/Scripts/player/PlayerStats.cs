using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Oxygen")]
    public float maxOxygen = 100f;
    public float oxygenDrainRate = 5f;        // per second, normal
    public float oxygenDashDrainRate = 20f;   // per second, while dashing
    float currentOxygen;

    [Header("Suit Integrity")]
    public float maxSuitIntegrity = 100f;
    public float suitDrainWhenNoOxygen = 15f; // per second when oxygen is 0
    float currentSuitIntegrity;

    PlayerMovement playerMovement;

    void Start()
    {
        currentOxygen = maxOxygen;
        currentSuitIntegrity = maxSuitIntegrity;
        playerMovement = GetComponent<PlayerMovement>();
    }

    
    void Update()
    {
        DrainOxygen();
        CheckSuitIntegrity();

        Debug.Log($"Oxygen: {currentOxygen:F1} | Suit: {currentSuitIntegrity:F1}");
    }

    void DrainOxygen()
    {
        float drainRate = playerMovement.IsDashing ? oxygenDashDrainRate : oxygenDrainRate;
        currentOxygen -= drainRate * Time.deltaTime;
        currentOxygen = Mathf.Clamp(currentOxygen, 0f, maxOxygen);
    }

    void CheckSuitIntegrity()
    {
        if (currentOxygen <= 0f)
        {
            currentSuitIntegrity -= suitDrainWhenNoOxygen * Time.deltaTime;
            currentSuitIntegrity = Mathf.Clamp(currentSuitIntegrity, 0f, maxSuitIntegrity);

            if (currentSuitIntegrity <= 0f)
            {
                Die();
            }
        }
    }


    public float GetSuitIntegrity() => currentSuitIntegrity;
    public float GetOxygen() => currentOxygen;
    public void RepairSuit()
    {
        currentSuitIntegrity = maxSuitIntegrity;
    }

    public void RefillOxygen(float amount)
    {
        currentOxygen = Mathf.Clamp(currentOxygen + amount, 0f, maxOxygen);
    }

    public void RefillAll()
    {
        currentOxygen = maxOxygen;
        currentSuitIntegrity = maxSuitIntegrity;
    }

    public void DamageSuit(float amount)
    {
        currentSuitIntegrity = Mathf.Clamp(currentSuitIntegrity - amount, 0f, maxSuitIntegrity);
    }

    void Die()
    {
        Debug.Log("Player died - respawn at last safe zone");
        // respawn logic goes here later
        RefillAll();
    }
}
