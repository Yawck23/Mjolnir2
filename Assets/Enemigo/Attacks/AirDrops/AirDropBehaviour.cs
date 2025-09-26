using UnityEngine;

public class AirDropBehaviour : MonoBehaviour
{

    #region Variables: gravity
    //private Vector3 _velocity;
    [SerializeField] float gravityMultiplier = 2.0f;
    //private float _gravity = -9.81f;
    //[SerializeField] float groundSeparation = 1f;
    private Rigidbody rb;
    #endregion

    #region Variables: detectPlayer
    private Vector3 halfObjectExtents;
    private BoxCollider objectCollider;
    [SerializeField] private float detectDistance;
    public bool playerDetected = false;
    #endregion

    #region Variables: applyDamage
    private GameObject playerObject;
    private HealthSystem playerHealth;
    [SerializeField] private float damage = 5f;
    #endregion

    [SerializeField] float destroyAfter = 10f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        objectCollider = GetComponent<BoxCollider>();
        halfObjectExtents = Vector3.Scale(objectCollider.size * 0.5f, transform.localScale);
        playerObject = GameObject.FindWithTag("Player");
        playerHealth = playerObject.GetComponent<HealthSystem>();

        Destroy(this.gameObject, destroyAfter);
    }

    private void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
    }

    void Update()
    {
        //ApplyGravity();
        if (detectPlayer())
        {
            playerDetected = true;
            playerHealth.TakeDamage(damage);
        }

    }

    /*private void ApplyGravity()
    {
        //Raycast desde la parte centro/abajo del objeto, un poco más arriba
        Vector3 origin = transform.position + Vector3.down * ((transform.localScale.y - 1f) / 2);
        Ray ray = new Ray(origin, Vector3.down);

        //Si el raycast detecta un hit y ese hit está a poca distancia, frena el objeto
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
        {
            if (hit.distance < groundSeparation)
            {
                _velocity.y = 0.0f;
                return;
            }
        }

        //Aplicar gravedad
        _velocity.y += _gravity * gravityMultiplier * Time.deltaTime;
        transform.position += _velocity * Time.deltaTime;
    }*/

    private bool detectPlayer()
    {
        //Devuelve true si detecta una colisión con un jugador
        Vector3 bottomCenter = transform.position - transform.up * detectDistance;

        Ray[] rays = {
            new Ray(transform.position - transform.right * halfObjectExtents.x - transform.forward * halfObjectExtents.z - transform.up * halfObjectExtents.y, (bottomCenter - (transform.position - transform.right * halfObjectExtents.x - transform.forward * halfObjectExtents.z - transform.up * halfObjectExtents.y))),
            new Ray(transform.position + transform.right * halfObjectExtents.x - transform.forward * halfObjectExtents.z - transform.up * halfObjectExtents.y, (bottomCenter - (transform.position + transform.right * halfObjectExtents.x - transform.forward * halfObjectExtents.z - transform.up * halfObjectExtents.y))),
            new Ray(transform.position - transform.right * halfObjectExtents.x + transform.forward * halfObjectExtents.z - transform.up * halfObjectExtents.y, (bottomCenter - (transform.position - transform.right * halfObjectExtents.x + transform.forward * halfObjectExtents.z - transform.up * halfObjectExtents.y))),
            new Ray(transform.position + transform.right * halfObjectExtents.x + transform.forward * halfObjectExtents.z - transform.up * halfObjectExtents.y, (bottomCenter - (transform.position + transform.right * halfObjectExtents.x + transform.forward * halfObjectExtents.z - transform.up * halfObjectExtents.y)))
        }; //Obtenemos un ray para cada esquina inferior del objeto y lo apuntamos detectDistance por debajo del centro del objeto

        foreach (Ray ray in rays)
        {
            float distanceToPoint = Vector3.Distance(transform.position - transform.right * halfObjectExtents.x - transform.forward * halfObjectExtents.z - transform.up * halfObjectExtents.y, bottomCenter);
            Debug.DrawRay(ray.origin, ray.direction * distanceToPoint, Color.red);
            if (Physics.Raycast(ray, out RaycastHit hit, distanceToPoint, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
