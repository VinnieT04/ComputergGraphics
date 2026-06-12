using UnityEngine;
using UnityEditor;

public class SnapToTerrain : EditorWindow
{
    [MenuItem("Tools/Snap Selected To Terrain")]
    static void SnapSelected()
    {
        Terrain terrain = Terrain.activeTerrain;
        if (terrain == null)
        {
            Debug.LogError("No active terrain found.");
            return;
        }

        int snapped = 0;
        foreach (GameObject obj in Selection.gameObjects)
        {
            Vector3 pos = obj.transform.position;
            float height = terrain.SampleHeight(pos) + terrain.transform.position.y;
            obj.transform.position = new Vector3(pos.x, height, pos.z);
            snapped++;
        }

        Debug.Log($"Snapped {snapped} objects to terrain.");
    }
}