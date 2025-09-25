using UnityEngine;

public class AttacksManager : MonoBehaviour
{
    private Animator animator;
    private HealthSystem playerHealth;
    private Transform targetPlayer;

    private bool canApplyDamage;
    private bool canTakeDamage;
    private bool canLookAtPlayer;
    private int randomAttackSelect;

    [SerializeField] GameObject airDrop;

    private float timer;
    [SerializeField] float attackCooldown = 8f;
    [SerializeField] float turnSpeed = 360f;
    [SerializeField] float modelOffset = -105f;

    private bool playerInLejos = false;
    private bool playerInCerca = false;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        targetPlayer = player.GetComponent<Transform>();
        playerHealth = player.GetComponent<HealthSystem>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        LookAtPlayer();
        AttackSelect();
    }

    #region Event Methods
    public void ApplyDamage()
    {
        canApplyDamage = true;
        canTakeDamage = false;
    }

    public void TakeDamage()
    {
        canTakeDamage = true;
        canApplyDamage = false;
    }

    public void NoDamage()
    {
        canTakeDamage = false;
        canApplyDamage = false;
    }

    public void StartLookAtPlayer()
    {
        canLookAtPlayer = true;
    }
    public void StopLookAtPlayer()
    {
        canLookAtPlayer = false;
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canApplyDamage)
        {
            playerHealth.TakeDamage(30);
        }

    }

    public void OnPlayerEnterZone (DetectionZone.ZoneType zone)
    {
        if (zone == DetectionZone.ZoneType.Cerca)
        {
            playerInCerca = true;
        }else if (zone == DetectionZone.ZoneType.Lejos)
        {
            playerInLejos = true;
        }
    }

    public void OnPlayerExitZone(DetectionZone.ZoneType zone)
    {
        if (zone == DetectionZone.ZoneType.Cerca)
        {
            playerInCerca = false;
        }
        else if (zone == DetectionZone.ZoneType.Lejos)
        {
            playerInLejos = false;
        }
    }

    public void AirDropStart()
    {
        Instantiate(airDrop);
    }

    private void AttackSelect()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            int randomAttackSelect = Random.Range(1, 4); // 1 o 2
            
            switch (randomAttackSelect)
            {
                case 1:
                    if (playerInCerca) animator.SetTrigger("AplastarCerca");
                    if (playerInLejos) animator.SetTrigger("AplastarLejos");
                    break;

                case 2:
                    animator.SetTrigger("ArrastrarBajo");
                    break;

                case 3:
                    animator.SetBool("LluviaHielo", true);
                    break;
            }

            timer = attackCooldown;
        }
    }

    private void LookAtPlayer()
    {
        if (canLookAtPlayer)
        {
            Vector3 to = targetPlayer.position - transform.position;
            to.y = 0f;
            if (to.sqrMagnitude < 0.0001f) return;

            // LookRotation ya mira en +Z; le sumamos el offset del modelo
            Quaternion desired = Quaternion.LookRotation(to, Vector3.up) * Quaternion.Euler(0f, modelOffset, 0f);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, desired, turnSpeed * Time.deltaTime);
        }

    }
}
