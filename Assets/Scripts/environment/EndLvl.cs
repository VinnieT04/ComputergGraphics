using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    private bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        
        if (other.CompareTag("Player"))
        {
            triggered = true;
            Debug.Log("Level end reached, play cinematic!");
            // cinematic trigger goes here
        }
    }
}