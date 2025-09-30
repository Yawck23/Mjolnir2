using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class PlayerAttackManager : MonoBehaviour
{
    private PlayerController controller;
    private EnemyHealthSystem enemyHealth;
    private Animator animator;

    [SerializeField] float dashDamage = 10f;
    [SerializeField] float airDropDamage = 10f;
    [SerializeField] float airDropAttackCooldown = 2f;
    private float _nextAirDropAttack = 0f;
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
        if (Time.time < _nextAirDropAttack) return; //Ataque en Cooldown

        animator.SetTrigger("Choque");
        Debug.Log("AirDropHit");
        //Lanzar el objeto y, si golpea en el enemigo, hacerle da�o

        _nextAirDropAttack = Time.time + airDropAttackCooldown;
    }
}
