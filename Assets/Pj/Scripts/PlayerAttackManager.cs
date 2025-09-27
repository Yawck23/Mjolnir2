using UnityEngine;
using UnityEngine.InputSystem.HID;

public class PlayerAttackManager : MonoBehaviour
{
    private PlayerController controller;
    private EnemyHealthSystem enemyHealth;
    private Animator animator;

    [SerializeField] float dashDamage = 10f;
    [SerializeField] float airDropDamage = 10f;
    private void Start()
    {
        controller = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        enemyHealth = GameObject.Find("Ymir").GetComponent<EnemyHealthSystem>();

    }

    //Hitbox del enemigo son Triggers
    private void OnTriggerEnter(Collider other)
    {
        if (!controller.IsDashing()) return;
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("EnemeyHit");
            DashAttack();
        }
    }

    //AirDrops son colliders
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!controller.IsDashing()) return;
        if (hit.collider.CompareTag("AirDrop"))
        {
            Debug.Log("AirDropHit");
            AirDropAttack();
        }
    }

    private void DashAttack()
    {
        animator.SetTrigger("Choque");
        enemyHealth.TakeDamage(dashDamage);
    }

    private void AirDropAttack()
    {
        animator.SetTrigger("Choque");
        //Lanzar el objeto y, si golpea en el enemigo, hacerle daño
    }
}
