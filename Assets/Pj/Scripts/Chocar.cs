using Unity.VisualScripting;
using UnityEngine;

public class Chocar : MonoBehaviour
{

    private CharacterController character;
    [SerializeField] float fuerzaRetroceso;
    [SerializeField] float duracionRetroceso;

    private Vector3 knockbackVector = Vector3.zero;
    private float knockbackTimer = 0f;

    
    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (knockbackTimer > 0)
        {
            character.Move(knockbackVector * Time.deltaTime);
            knockbackTimer -= Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Enemigo")
        {
            Vector3 dir = transform.position - c.contacts[0].point;
            dir = -dir.normalized;

            knockbackVector = dir * fuerzaRetroceso;
            knockbackTimer = duracionRetroceso;
        }
    }
}
