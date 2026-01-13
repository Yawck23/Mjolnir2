using NUnit.Framework;
using UnityEngine;

public class AirDropBehaviour : MonoBehaviour
{

    #region Variables: Modifiers
    [SerializeField] float gravityMultiplier = 12f;
    public bool playerDetected = false;
    [SerializeField] float destroyAfter = 20f;
    #endregion

    private bool hasLanded = false;

    #region Variables: Components
    private GameObject playerObject;
    private HealthSystem playerHealth;
    private Rigidbody rb;
    private BoxCollider ObjCollider;
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ObjCollider = GetComponent<BoxCollider>();
        rb.useGravity = false;
        playerObject = GameObject.FindWithTag("Player");
        playerHealth = playerObject.GetComponent<HealthSystem>();

        Destroy(this.gameObject, destroyAfter);
    }

    void Update()
    {
        if (!hasLanded){
            DetectFloor();
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && !playerDetected)
        {
            if (!hasLanded)
            {
                playerHealth.TakeDamage();
                playerDetected = true;
            }
        }

        if (collision.collider.CompareTag("AirDrop"))
        {
            if (transform.position.y >= collision.transform.position.y)
            {
                Destroy(collision.gameObject);
            }
        }
    }

    private void DetectFloor()
    {
        hasLanded = Physics.Raycast(transform.position, Vector3.down, ObjCollider.bounds.extents.y + 0.2f, 0, QueryTriggerInteraction.Ignore);
    }
}
