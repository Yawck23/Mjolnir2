using System.Timers;
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
    //[SerializeField] float detectDistance;
    //[SerializeField] float centerOffset;
    public bool playerDetected = false;
    [SerializeField] float detectDistance = 15f; // Distancia extra hacia abajo
    #endregion

    #region Variables: applyDamage
    private GameObject playerObject;
    private HealthSystem playerHealth;
    [SerializeField] private float damage = 100f;
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
        /*//Devuelve true si detecta una colisión con un jugador
        Vector3 bottomCenter = transform.position - transform.up * detectDistance;

        Ray[] rays = {
            new Ray(transform.position - transform.right * halfObjectExtents.x * centerOffset - transform.forward * halfObjectExtents.z * centerOffset - transform.up * halfObjectExtents.y, (bottomCenter - (transform.position - transform.right * halfObjectExtents.x * centerOffset - transform.forward * halfObjectExtents.z * centerOffset - transform.up * halfObjectExtents.y))),
            new Ray(transform.position + transform.right * halfObjectExtents.x * centerOffset - transform.forward * halfObjectExtents.z * centerOffset - transform.up * halfObjectExtents.y, (bottomCenter - (transform.position + transform.right * halfObjectExtents.x * centerOffset - transform.forward * halfObjectExtents.z * centerOffset - transform.up * halfObjectExtents.y))),
            new Ray(transform.position - transform.right * halfObjectExtents.x * centerOffset + transform.forward * halfObjectExtents.z * centerOffset - transform.up * halfObjectExtents.y, (bottomCenter - (transform.position - transform.right * halfObjectExtents.x * centerOffset + transform.forward * halfObjectExtents.z * centerOffset - transform.up * halfObjectExtents.y))),
            new Ray(transform.position + transform.right * halfObjectExtents.x * centerOffset + transform.forward * halfObjectExtents.z * centerOffset - transform.up * halfObjectExtents.y, (bottomCenter - (transform.position + transform.right * halfObjectExtents.x * centerOffset + transform.forward * halfObjectExtents.z * centerOffset - transform.up * halfObjectExtents.y)))
        };  //Obtenemos un ray para cada esquina inferior del objeto y lo apuntamos detectDistance por debajo del centro del objeto
                //El centerOffset nos sirve para arrimar el raycast desde la esquina hacia el centro del objeto

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

        return false;*/


        //Nueva detección del player
        Bounds bounds = objectCollider.bounds;
        Vector3 centerBottom = new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);

        Vector3[] corners = new Vector3[4];
        corners[0] = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);
        corners[1] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
        corners[2] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
        corners[3] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);

        foreach (Vector3 corner in corners)
        {
            Vector3 target = centerBottom + Vector3.down * detectDistance;
            Vector3 direction = (target - corner).normalized;
            float distance = Vector3.Distance(corner, target);

            if (Physics.Raycast(corner, direction, out RaycastHit hit, distance, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }

            // Dibujar el rayo para debug
            Debug.DrawRay(corner, direction * distance, Color.blue);
        }

        return false;
    }
}
