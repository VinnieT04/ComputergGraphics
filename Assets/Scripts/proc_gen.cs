using UnityEngine;
[ExecuteInEditMode]
public class TerrainObjectSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] prefabs; // drag rocks, geysers, etc here

    [Header("Spawn Settings")]
    public int count = 50;
    public float areaWidth = 100f;
    public float areaLength = 100f;

    [Header("Scale Randomization")]
    public float minScale = 0.8f;
    public float maxScale = 1.5f;

    [Header("Slope Filter")]
    public float maxSlopeAngle = 30f; // won't spawn on steep walls

    [Header("Spawn Zones (optional)")]
    public bool useZones = false;
    public Collider[] allowedZones; // only spawn inside these colliders

    private Terrain terrain;
    private TerrainData terrainData;
    private Vector3 terrainPos;

    void Start()
    {
        terrain = Terrain.activeTerrain;
        terrainData = terrain.terrainData;
        terrainPos = terrain.transform.position;

        Spawn();
    }

    void Spawn()
    {
        int spawned = 0;
        int attempts = 0;
        int maxAttempts = count * 10; // avoid infinite loop

        while (spawned < count && attempts < maxAttempts)
        {
            attempts++;

            // Random position within area (centered on this GameObject)
            Vector3 randomPos = new Vector3(
                transform.position.x + Random.Range(-areaWidth / 2f, areaWidth / 2f),
                0,
                transform.position.z + Random.Range(-areaLength / 2f, areaLength / 2f)
            );

            // Snap to terrain height
            randomPos.y = terrain.SampleHeight(randomPos) + terrainPos.y;

            // Check slope — skip if too steep
            float normX = (randomPos.x - terrainPos.x) / terrainData.size.x;
            float normZ = (randomPos.z - terrainPos.z) / terrainData.size.z;
            Vector3 normal = terrainData.GetInterpolatedNormal(normX, normZ);
            float slopeAngle = Vector3.Angle(normal, Vector3.up);

            if (slopeAngle > maxSlopeAngle) continue;

            // Zone check (optional)
            if (useZones && !IsInsideAnyZone(randomPos)) continue;

            // Pick random prefab
            GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];

            // Spawn with random rotation and scale
            float scale = Random.Range(minScale, maxScale);
            GameObject obj = Instantiate(prefab, randomPos, Quaternion.Euler(0, Random.Range(0, 360), 0));
            obj.transform.localScale = Vector3.one * scale;
            obj.transform.parent = this.transform; // keep hierarchy clean

            spawned++;
        }

        Debug.Log($"Spawned {spawned} objects after {attempts} attempts.");
    }

    bool IsInsideAnyZone(Vector3 pos)
    {
        foreach (Collider zone in allowedZones)
        {
            if (zone.bounds.Contains(pos)) return true;
        }
        return false;
    }

    // Draw the spawn area in the editor so you can see it
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawCube(transform.position, new Vector3(areaWidth, 5f, areaLength));
    }
}