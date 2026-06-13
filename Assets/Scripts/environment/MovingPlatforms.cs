using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public enum PlatformType { Horizontal, Vertical, Circular, ZigZag }
    public PlatformType platformType = PlatformType.Horizontal;

    [Header("Settings")]
    public float speed = 2f;
    public float range = 5f;
    public float zigZagAngle = 45f;
    public Vector3 horizontalDirection = Vector3.right; // change this to any direction you want

    Vector3 startPos;
    float timer = 0f;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        timer += Time.deltaTime * speed;

        switch (platformType)
        {
            case PlatformType.Horizontal:
                transform.position = startPos + horizontalDirection.normalized * Mathf.Sin(timer) * range;
                break;

            case PlatformType.Vertical:
                transform.position = startPos + Vector3.up * Mathf.Sin(timer) * range;
                break;

            case PlatformType.Circular:
                transform.position = startPos + new Vector3(
                    Mathf.Cos(timer) * range,
                    0,
                    Mathf.Sin(timer) * range
                );
                break;

            case PlatformType.ZigZag:
                float angle = Mathf.Sin(timer) * zigZagAngle * Mathf.Deg2Rad;
                transform.position = startPos + new Vector3(
                    Mathf.Sin(angle) * range,
                    0,
                    Mathf.Cos(timer) * range
                );
                break;
        }
    }

    // Makes player move with the platform
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}
