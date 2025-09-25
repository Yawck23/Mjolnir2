using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public enum ZoneType { Lejos, Cerca}
    public ZoneType zoneType;
    private AttacksManager enemy;

    private void Start()
    {
        enemy = GetComponentInParent<AttacksManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.OnPlayerEnterZone(zoneType);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.OnPlayerExitZone(zoneType);
        }
    }
}
