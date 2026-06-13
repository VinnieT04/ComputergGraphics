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
        if (MusicManager.Instance != null)
            MusicManager.Instance.PlayEndSequence(nextSceneName);
        else
            SceneManager.LoadScene(nextSceneName); // fallback si no hay MusicManager
    }
}