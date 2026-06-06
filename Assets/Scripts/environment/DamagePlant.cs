using UnityEngine;

public class DamagePlant : MonoBehaviour
{
    public float damagePerSecond = 10f;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStats>().DamageSuit(damagePerSecond * Time.deltaTime);
        }
    }
}