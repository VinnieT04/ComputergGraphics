using UnityEngine;

public class FogZoneTrigger : MonoBehaviour
{
    private FogZone fogZone;

    void Start()
    {
        fogZone = GetComponentInParent<FogZone>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            fogZone.PlayerEntered(other.GetComponent<PlayerStats>());
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            fogZone.PlayerExited();
    }
}