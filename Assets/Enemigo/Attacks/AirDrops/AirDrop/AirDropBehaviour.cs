using NUnit.Framework;
using UnityEngine;

public class AirDropBehaviour : MonoBehaviour
{

    #region Variables: Modifiers
    [SerializeField] float gravityMultiplier = 12f;
    public bool playerDetected = false;
    //[SerializeField] float destroyAfter = 20f;
    #endregion

    public bool hasLanded = false;

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

        //Destroy(this.gameObject, destroyAfter); Solo se destruye cuando el jefe lo toca
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
                playerDetected = true;
                playerHealth.TakeDamage();
                
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
        // Calcular el punto central de la cara inferior del BoxCollider
        Bounds b = ObjCollider.bounds;
        Vector3 origin = new Vector3(b.center.x, b.min.y, b.center.z);
        // Pequeña compensación hacia abajo para evitar detectar el propio collider
        origin += Vector3.down * 0.01f;
        //Debug.DrawRay(origin, Vector3.down * 4f, Color.red, 0.1f);

        Ray ray = new Ray(origin, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 4f, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                hasLanded = true;
            }
        }
    }
}
