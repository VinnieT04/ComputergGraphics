using UnityEngine;
using UnityEngine.Rendering;

public class FogZone : MonoBehaviour
{
    [Header("Fog Settings")]
    public float maxFogDensity = 0.08f;
    public float fogTransitionSpeed = 2f;
    public Color safeFogColor = new Color(0.6f, 0.4f, 0.2f);
    public Color dangerFogColor = new Color(0.3f, 0.15f, 0.05f);

    [Header("Damage")]
    public float damagePerSecond = 10f;

    private int safeZoneCount = 0; // tracks how many boxes player is inside
    private PlayerStats playerStats;

    void Start()
    {
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogDensity = 0.02f;
        RenderSettings.fogColor = new Color(151/255f, 95/255f, 40/255f, 200/255f);
    }

    void Update()
    {
        Debug.Log($"SafeZoneCount: {safeZoneCount}");
        bool inSafeZone = safeZoneCount > 0;

        float targetDensity = inSafeZone ? 0.02f : maxFogDensity;
        Color targetColor = inSafeZone ? safeFogColor : dangerFogColor;

        RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, targetDensity, Time.deltaTime * fogTransitionSpeed);
        RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, targetColor, Time.deltaTime * fogTransitionSpeed);

        if (!inSafeZone && playerStats != null)
            playerStats.DamageSuit(damagePerSecond * Time.deltaTime);
    }

    public void PlayerEntered(PlayerStats stats)
    {
        safeZoneCount++;
        playerStats = stats;
    }

    public void PlayerExited()
    {
        safeZoneCount--;
        if (safeZoneCount < 0) safeZoneCount = 0;
    }
}