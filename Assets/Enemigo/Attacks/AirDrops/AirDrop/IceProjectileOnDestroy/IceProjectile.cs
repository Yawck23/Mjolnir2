using System;
using UnityEngine;

public class IceProjectile : MonoBehaviour
{
    private Transform pjTransform;
    [SerializeField] float iceProjectileSpeed = 50f;
    [SerializeField] float iceProjectileAngle = 20f;
    [SerializeField] float destroyDelay = 10f;
    [SerializeField] float iceProjectileDamage = 10f;

    private EnemyHealthSystem ymirHealth;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ymirHealth = GameObject.Find("Ymir").GetComponent<EnemyHealthSystem>();
        pjTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Rigidbody rb = GetComponent<Rigidbody>();

        //Calculamos el ángulo
        Vector3 dir = Quaternion.AngleAxis(-iceProjectileAngle, pjTransform.right) * pjTransform.forward;
        dir.Normalize();
        Vector3 projectileForce = dir * iceProjectileSpeed;
        //Aplicamos la fuerza
        rb.AddForce(projectileForce, ForceMode.Impulse);
        Destroy(this.gameObject, destroyDelay);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Enemy"))
        {
            ymirHealth.TakeDamage(iceProjectileDamage, "Pecho");
            Destroy(this.gameObject);
        }
    }
}
