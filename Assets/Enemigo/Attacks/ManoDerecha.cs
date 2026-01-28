using UnityEngine;

public class ManoDerecha : MonoBehaviour
{
    private AttacksManager attacksManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attacksManager = GetComponentInParent<AttacksManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AirDrop"))
        {
            other.GetComponent<ExplosionIntoParts>().Explosion();
        }

        if (other.CompareTag("PisoHielo") && attacksManager.CanDestroyPisoHielo())
        {
            Destroy(other.gameObject);
        }
    }
}
