using UnityEngine;

public class IceProjectile : MonoBehaviour
{
    private Transform pjTransform;
    [SerializeField] float iceProjectileSpeed = 50f;
    [SerializeField] float iceProjectileAngle = 20f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pjTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Rigidbody rb = GetComponent<Rigidbody>();
        //Calculamos el ángulo
        Vector3 dir = Quaternion.AngleAxis(-iceProjectileAngle, pjTransform.right) * pjTransform.forward;
        dir.Normalize();
        Vector3 projectileForce = dir * iceProjectileSpeed;
        //Aplicamos la fuerza
        rb.AddForce(projectileForce, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
