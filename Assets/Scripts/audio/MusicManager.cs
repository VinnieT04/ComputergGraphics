using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource riserSource;

    [Header("Riser Settings")]
    public AudioClip riserClip;
    public float fadeDuration = 2f;

    void Awake()
    {
        // Singleton para que EndLvl pueda encontrarlo facilmente
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayEndSequence(string nextSceneName)
    {
        StartCoroutine(EndSequenceRoutine(nextSceneName));
    }

    IEnumerator EndSequenceRoutine(string nextSceneName)
    {
        // Reproduce el riser
        if (riserSource != null && riserClip != null)
        {
            riserSource.PlayOneShot(riserClip);
        }

        // Fade out del soundtrack mientras suena el riser
        float startVolume = musicSource.volume;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            yield return null;
        }

        musicSource.Stop();

        // Espera a que termine el riser antes de cambiar de escena
        if (riserClip != null)
            yield return new WaitForSeconds(riserClip.length - fadeDuration);

        SceneManager.LoadScene(nextSceneName);
    }
}