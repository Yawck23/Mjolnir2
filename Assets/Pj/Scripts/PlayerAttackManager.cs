using UnityEngine;
using UnityEngine.InputSystem.HID;

public class PlayerAttackManager : MonoBehaviour
{
    private PlayerController controller;
    private EnemyHealthSystem enemyHealth;
    [SerializeField] float dashDamage = 10f;
    [SerializeField] float airDropDamage = 10f;
    private void Start()
    {
        controller = GetComponent<PlayerController>();
        enemyHealth = GameObject.Find("Ymir").GetComponent<EnemyHealthSystem>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!controller.IsDashing()) return;
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("EnemeyHit");
            DashAttack();
        }
    }

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
        enemyHealth.TakeDamage(dashDamage);
    }

    private void AirDropAttack()
    {
        //Lanzar el objeto y, si golpea en el enemigo, hacerle daño
    }
}
