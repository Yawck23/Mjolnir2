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

    private void DashAttack()
    {
        animator.SetTrigger("Choque");
        enemyHealth.TakeDamage(dashDamage, "Mano");
    }

    public void AirDropAttack()
    {
        if (Time.time < _nextAirDropAttack) return; //Ataque en Cooldown

        animator.SetTrigger("Choque");

        //Instanciamos el proyectil. Angle es para que apunte hacia arriba
        Vector3 angle = transform.eulerAngles;
        angle.x = -20f;
        Quaternion rotation = Quaternion.Euler(angle);
        Instantiate(iceProjectile, transform.position, rotation);

        _nextAirDropAttack = Time.time + airDropAttackCooldown;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!controller.IsDashing()) return;
        ExplosionIntoParts explosionController = hit.collider.GetComponent<ExplosionIntoParts>();
        //TutorialExit tutorialExit = hit.collider.GetComponent<TutorialExit>();

        if (hit.collider.CompareTag("AirDrop"))
        {
            AirDropAttack();
        }

        if (explosionController != null)
        {
            explosionController.Explosion();
        }

        /*if (tutorialExit != null)
        {
            tutorialExit.TutorialExitTrigger();
        }*/

    }
}
