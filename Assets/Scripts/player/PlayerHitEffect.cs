using UnityEngine;
using UnityEngine.UI;

public class PlayerHitEffect : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip hitSound;
    private AudioSource audioSource;

    [Header("Particle Effect")]
    public ParticleSystem hitParticles;

    [Header("Screen Flash")]
    public Image screenFlashImage;
    public float flashDuration = 0.2f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.4f);
    private float flashTimer;
    private bool isFlashing;

    //detects damage automatically
    private PlayerStats playerStats;
    private float lastSuitIntegrity;

    private float hitCooldown = 0.5f;
    private float hitCooldownTimer;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerStats = GetComponent<PlayerStats>();
        lastSuitIntegrity = playerStats.GetSuitIntegrity();

        if (screenFlashImage != null)
            screenFlashImage.color = Color.clear;
    }

    void Update()
    {
        hitCooldownTimer -= Time.deltaTime;

        float currentIntegrity = playerStats.GetSuitIntegrity();
        if (currentIntegrity < lastSuitIntegrity && hitCooldownTimer <= 0f)
        {
            TriggerHitEffect();
            hitCooldownTimer = hitCooldown;
        }
        lastSuitIntegrity = currentIntegrity;

        // Fade out del flash
        if (isFlashing)
        {
            flashTimer -= Time.deltaTime;
            float alpha = Mathf.Clamp01(flashTimer / flashDuration);
            screenFlashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha * flashColor.a);

            if (flashTimer <= 0f)
            {
                isFlashing = false;
                screenFlashImage.color = Color.clear;
            }
        }

        // SOLO PARA TESTING — borrar después
        if (Input.GetKeyDown(KeyCode.H))
            playerStats.DamageSuit(10f);
    }

    public void TriggerHitEffect()
    {
        //sound
        if (hitSound != null)
            audioSource.PlayOneShot(hitSound);

        //particles
        if (hitParticles != null)
            hitParticles.Play();

        //image ui
        if (screenFlashImage != null)
        {
            flashTimer = flashDuration;
            isFlashing = true;
            screenFlashImage.color = flashColor;
        }
    }
}