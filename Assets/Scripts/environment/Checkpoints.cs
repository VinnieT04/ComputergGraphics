using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    private bool activated = false;

    void OnTriggerEnter(Collider other)
    {
        if (activated) return;
        
        if (other.CompareTag("Player"))
        {
            activated = true;
            other.GetComponent<PlayerStats>().RefillAll();
            Debug.Log("Checkpoint reached!");
        }
    }
}