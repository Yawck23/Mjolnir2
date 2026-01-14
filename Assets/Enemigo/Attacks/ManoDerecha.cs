using UnityEngine;

public class ManoDerecha : MonoBehaviour
{
    private AttacksManager attacksManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attacksManager = GetComponentInParent<AttacksManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AirDrop"))
        {
            Destroy(other.gameObject);
        }

        if (other.CompareTag("PisoHielo") && attacksManager.CanDestroyPisoHielo())
        {
            Destroy(other.gameObject);
        }
    }
}
