using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class Knockback : MonoBehaviour
{
    private PlayerController player;
    private CharacterController controller;
    private float knockbackSpeed = 20f;
    private float knockbackTime = 0.5f;

    private Vector3 knockbackVector = Vector3.zero;
    private bool isKnockback; //Determina si te están aplicando knockback o no. Se obtiene con el método isKnockbacking.
    private string[] appliesKnockback = {"Enemy"}; //Determina con qué tags aplica knockback


    void Start()
    {
        player = GetComponent<PlayerController>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator KnockbackCorutine()
    {
        isKnockback = true;
        float startTime = Time.time;
        while (Time.time < startTime + knockbackTime) //La corutina dura el knockbackTime
        {
            controller.Move(knockbackVector * Time.deltaTime);
            yield return null;

        }
        isKnockback = false;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (appliesKnockback.Contains(hit.collider.tag)) //Si el objeto colisiona con un tag valido
        {
            Vector3 dir = transform.position - hit.point;
            dir.y = 0f;
            dir.Normalize();

            knockbackVector = dir * knockbackSpeed;

            if (!isKnockback)
            {
                StartCoroutine(KnockbackCorutine());
            }
        }
    }

    public bool isKnockbacking()
    {
        return isKnockback;
    }
}
