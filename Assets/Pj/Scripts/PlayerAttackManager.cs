using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class PlayerAttackManager : MonoBehaviour
{
    #region Variables: Components
    private PlayerController controller;
    private EnemyHealthSystem enemyHealth;
    private Animator animator;

    [SerializeField] GameObject iceProjectile;
    #endregion

    [SerializeField] float dashDamage = 10f;
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
            DashAttack();
        }
    }

    //AirDrops son colliders
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
        if (hit.collider.CompareTag("AirDrop"))
        {
            if (!controller.IsDashing()) return;
            AirDropAttack();
            Destroy(hit.gameObject);
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

        //Instanciamos el proyectil
        Instantiate(iceProjectile, transform.position, transform.rotation);

        _nextAirDropAttack = Time.time + airDropAttackCooldown;
    }
}
