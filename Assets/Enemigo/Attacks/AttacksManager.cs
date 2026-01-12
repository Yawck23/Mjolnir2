using UnityEngine;

public class AttacksManager : MonoBehaviour
{
    #region Variables: Components
    private Animator animator;
    private HealthSystem playerHealth;
    private Transform targetPlayer;

    private Transform pivotYmir;
    private EnemyHealthSystem ymirHealth;
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
    [SerializeField] GameObject pisoHielo;
    #endregion

    #region DebugMode
    [SerializeField] bool debugMode = false;
    [SerializeField] int debugAttack;
    [SerializeField] int debugStage;
    #endregion

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        targetPlayer = player.GetComponent<Transform>();
        playerHealth = player.GetComponent<HealthSystem>();
        ymirHealth = GetComponent<EnemyHealthSystem>();
        animator = GetComponent<Animator>();
        pivotYmir = transform.parent;

    }

    void Update()
    {
        LookAtPlayer();
        AttackSelect();
        
        if (debugMode)
        {
            animator.SetInteger("Stage", debugStage);
        }
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

    public void PisoHieloStart()
    {
        Instantiate(pisoHielo);
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
        if (timer > 0f) return; //Esperamos al cooldown
        if (ymirHealth.IsDead()) return; //Si el boss está muerto, no elige ataques
        if (playerHealth.getIsDead()) return; //Si el player está muerto, no elige ataques

        if (debugMode)
        {
            randomAttackSelect = debugAttack;
        }
        else
        {
            int actualStage = ymirHealth.getActualStage();
            int maxRandomRange = 3; //Por defecto hace ataques 1 y 2
            if (actualStage > 1) maxRandomRange = 5; //Si se pasa la stage 1, hace ataques 1, 2, 3, 4.
            randomAttackSelect = Random.Range(1, maxRandomRange);
        }


        switch (randomAttackSelect)
        {
            case 1: //Aplastar cerca o lejos
                if (playerInCerca) animator.SetTrigger("AplastarCerca");
                if (playerInLejos) animator.SetTrigger("AplastarLejos");
                timer = attackCooldown;
                break;

            case 2: //Arrastrar
                animator.SetTrigger("ArrastrarBajo");
                timer = attackCooldown;
                break;

            case 3: //Lluvia de hielo
                animator.SetBool("LluviaHielo", true);
                timer = attackCooldown;
                break;

            case 4: //Piso de hielo
                animator.SetTrigger("PisoHielo");
                timer = attackCooldown / 4; //Espera menos para realizar otro ataque
                break;
        }

        int bisAttack = Random.Range(0, 2);
        if (bisAttack == 0) animator.SetBool("BisAttack", false);
        if (bisAttack == 1) animator.SetBool("BisAttack", true);        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canApplyDamage)
        {
            playerHealth.TakeDamage();
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
