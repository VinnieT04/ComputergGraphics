using UnityEngine;

public class GeyserLaunch : MonoBehaviour
{
    public float launchForce = 20f;
    public float cooldown = 3f;
    float cooldownTimer = 0f;

    void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (cooldownTimer > 0f) return;

        PlayerMovement pm = other.GetComponent<PlayerMovement>();
        pm.Launch(Vector3.up * launchForce);
        cooldownTimer = cooldown;
        Debug.Log("Geyser launched player!");
    }
}