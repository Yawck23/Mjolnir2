using UnityEngine;

public class ExplosionIntoParts : MonoBehaviour
{
    [SerializeField] private float dissapearTimeMin, dissapearTimeMax;
    [SerializeField] private float explosionForce = 10000f;
    [SerializeField] private float explosionRadius = 450f;
    [SerializeField] private Transform explosionPoint;
    private float dissapearTime;
    private PlayerController player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    //Este script requiere un obejto padre con rigidbody y collider y varios hijos con rigidbody.
    //Los rigidbody de los hijos deben estar en isKinematic true.

    public void Explosion()
    {        
        BoxCollider bc = GetComponent<BoxCollider>();
        bc.enabled = false;
        AudioManager.AM.Play("IceBreak");

        foreach (Transform child in transform) //Aplicamos la explosion a cada rigidbody hijo
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb == null) continue;

            dissapearTime = Random.Range(dissapearTimeMin, dissapearTimeMax);

            rb.isKinematic = false;
            rb.AddExplosionForce(explosionForce, explosionPoint.position, explosionRadius);
            Destroy(child.gameObject, dissapearTime);
        }

        Destroy(this.gameObject, dissapearTimeMax + 1f); //Destruimos el objeto padre despues de que todos los hijos hayan desaparecido
    }

    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.collider.CompareTag("PlayerHitBox") || collision.collider.CompareTag("Player"))
        {
            if (player.IsDashing())
            {
                Explosion();
            }
        }
    }
}
