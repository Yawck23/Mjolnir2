using Unity.VisualScripting;
using UnityEngine;

public class Chocar : MonoBehaviour
{


    private Principal player;
    private float fuerzaRetroceso = 20f;
    private float duracionRetroceso = 0.5f;

    private Vector3 knockbackVector = Vector3.zero;
    private float knockbackTimer = 0f;

    
    void Start()
    {
        player = GetComponent<Principal>();
    }

    // Update is called once per frame
    void Update()
    {
        if (knockbackTimer > 0)
        {
            player.controller.Move(knockbackVector * Time.deltaTime);
            knockbackTimer -= Time.deltaTime;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Enemigo"))
        {
            Vector3 dir = transform.position - hit.point;
            dir.y = 0f;
            dir.Normalize();

            knockbackVector = dir * fuerzaRetroceso;
            knockbackTimer = duracionRetroceso;
        }
    }
}
