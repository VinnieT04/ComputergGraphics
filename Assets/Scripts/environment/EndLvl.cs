using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    public string nextSceneName;
    private bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;
        OnLevelEnd();
    }

    void OnLevelEnd()
    {
        // CINEMATIC HOOK — teammate plugs their system in here
        // For now just loads next scene directly
        SceneManager.LoadScene(nextSceneName);
    }
}