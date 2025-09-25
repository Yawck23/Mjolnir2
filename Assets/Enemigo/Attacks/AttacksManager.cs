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
    private float timer;
    [SerializeField] float attackCooldown = 8f;
    [SerializeField] float turnSpeed = 360f;
    [SerializeField] float modelOffset = -105f;

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

    private void AttackSelect()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            int randomAttackSelect = Random.Range(1, 4); // 1, 2 o 3
            
            switch (randomAttackSelect)
            {
                case 1:
                    animator.SetTrigger("AplastarLejos");
                    break;

                case 2:
                    animator.SetTrigger("AplastarCerca");
                    break;

                case 3:
                    animator.SetTrigger("ArrastrarBajo");
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
