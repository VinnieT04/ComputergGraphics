using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("Oxygen Bar")]
    public Image oxygenBarFill;
    public Color oxygenNormal = new Color(0.11f, 0.62f, 0.46f);   // teal
    public Color oxygenLow    = new Color(0.94f, 0.62f, 0.15f);   // amber
    public Color oxygenEmpty  = new Color(0.89f, 0.30f, 0.29f);   // red

    [Header("Suit Integrity Bar")]
    public Image suitBarFill;
    public Color suitNormal   = new Color(0.22f, 0.54f, 0.87f);   // blue
    public Color suitDamaged  = new Color(0.94f, 0.62f, 0.15f);   // amber
    public Color suitCritical = new Color(0.89f, 0.30f, 0.29f);   // red

    PlayerStats playerStats;

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();

        if (playerStats == null)
            Debug.LogError("PlayerHUD: PlayerStats not found in scene!");
    }

    void Update()
    {
        if (playerStats == null) return;

        UpdateOxygenBar();
        UpdateSuitBar();
    }

    void UpdateOxygenBar()
    {
        float oxyPct = playerStats.GetOxygen() / playerStats.maxOxygen;
        oxygenBarFill.fillAmount = oxyPct;

        if (oxyPct <= 0.2f)
            oxygenBarFill.color = oxygenEmpty;
        else if (oxyPct <= 0.5f)
            oxygenBarFill.color = oxygenLow;
        else
            oxygenBarFill.color = oxygenNormal;
    }

    void UpdateSuitBar()
    {
        float suitPct = playerStats.GetSuitIntegrity() / playerStats.maxSuitIntegrity;
        suitBarFill.fillAmount = suitPct;

        if (suitPct <= 0.2f)
            suitBarFill.color = suitCritical;
        else if (suitPct <= 0.5f)
            suitBarFill.color = suitDamaged;
        else
            suitBarFill.color = suitNormal;
    }
}