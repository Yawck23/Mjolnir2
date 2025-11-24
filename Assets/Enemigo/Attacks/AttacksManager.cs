using UnityEngine;

public class AttacksManager : MonoBehaviour
{
    #region Variables: Components
    private Animator animator;
    private HealthSystem playerHealth;
    private Transform targetPlayer;

    private Transform pivotYmir;
    #endregion

    #region Variables: Boolean States
    private bool canApplyDamage;
    private bool canTakeDamage;
    private bool canLookAtPlayer;

    private bool playerInLejos = false;
    private bool playerInCerca = false;
    #endregion

    #region Variables: Attacks
    private float timer;
    private int randomAttackSelect;
    [SerializeField] float attackCooldown = 8f;
    [SerializeField] float turnSpeed = 360f;
    [SerializeField] float modelOffset = -105f;
    [SerializeField] GameObject airDrop;
    #endregion


    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        targetPlayer = player.GetComponent<Transform>();
        playerHealth = player.GetComponent<HealthSystem>();
        animator = GetComponent<Animator>();
        pivotYmir = transform.parent;

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
    public void AirDropStart()
    {
        Instantiate(airDrop);
    }
    #endregion

    #region Triggers Zones
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
    #endregion
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canApplyDamage)
        {
            playerHealth.TakeDamage(30);
        }
    }

    private void LookAtPlayer()
    {
        if (canLookAtPlayer)
        {
            Vector3 to = targetPlayer.position - pivotYmir.position;
            to.y = 0f;
            if (to.sqrMagnitude < 0.0001f) return;

            // LookRotation ya mira en +Z; le sumamos el offset del modelo
            Quaternion desired = Quaternion.LookRotation(to, Vector3.up) * Quaternion.Euler(0f, modelOffset, 0f);

            pivotYmir.rotation = Quaternion.RotateTowards(pivotYmir.rotation, desired, turnSpeed * Time.deltaTime);
        }

    }

    public bool CanTakeDamage() => canTakeDamage;
}
