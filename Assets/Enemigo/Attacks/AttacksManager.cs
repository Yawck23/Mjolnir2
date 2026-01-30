using System.Collections;
using UnityEngine;

public class AttacksManager : MonoBehaviour
{
    #region Variables: Components
    private Animator animator;
    private HealthSystem playerHealth;
    private Transform targetPlayer;

    private Transform pivotYmir;
    private EnemyHealthSystem ymirHealth;
    private TutorialExit tutorialExit;
    #endregion

    #region Variables: Boolean States
    private bool canApplyDamage;
    private bool canTakeDamage;
    private bool canLookAtPlayer;
    private bool canDestroyPisoHielo;

    //private bool playerInLejos = false; No es necesario dado que si no está cerca, es porque está lejos...
    private bool playerInCerca = false;
    #endregion

    #region Variables: Attacks
    private float nextAttackTimer;
    private int randomAttackSelect;
    [SerializeField] float attackCooldown = 8f;
    [SerializeField] float turnSpeed = 360f;
    [SerializeField] float modelOffset = -105f;
    [SerializeField] GameObject airDrop;
    [SerializeField] GameObject pisoHielo;
    #endregion

    #region Variables: Attack Modifiers

    [SerializeField] float maxVelocityMultiplier = 1.5f;
    [SerializeField] float minLookPlayerTime = 0.0f;
    [SerializeField] float maxLookPlayerTime = 1.5f;
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

        tutorialExit = GameObject.FindGameObjectWithTag("TutorialExit").GetComponent<TutorialExit>();

    }

    void Update()
    {
        LookAtPlayer();
        AttackTimerUpdate();
        
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
        StartCoroutine(stopLookAtPlayerRandomWait());
    }

    private IEnumerator stopLookAtPlayerRandomWait()
    {
        float waitTime = Random.Range(minLookPlayerTime, maxLookPlayerTime);
        yield return new WaitForSeconds(waitTime);
        canLookAtPlayer = false;
    }

    public void AirDropStart()
    {
        Vector3 spawnPointAirDropSpawn = pivotYmir.position;
        Instantiate(airDrop, spawnPointAirDropSpawn, Quaternion.identity);
    }

    public void PisoHieloStart()
    {
        Instantiate(pisoHielo);
    }

    public void AttackSelect()
    {        
        if (nextAttackTimer > 0f) return; //Esperamos al cooldown
        if (ymirHealth.IsDead()) return; //Si el boss está muerto, no elige ataques
        if (playerHealth.getIsDead()) return; //Si el player está muerto, no elige ataques
        if (!tutorialExit.IsTutorialFinished()) return; //Si no se terminó el tutorial, no elige ataques

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

        canDestroyPisoHielo = false; //Por defecto no puede destruir el piso de hielo

        float velocityMultiplier = Random.Range(1.0f, maxVelocityMultiplier);
        animator.SetFloat("VelocityMultiplier", velocityMultiplier);

        switch (randomAttackSelect)
        {
            case 1: //Aplastar cerca o lejos
                if (playerInCerca)
                {
                    animator.SetTrigger("AplastarCerca");
                }
                else
                {
                    animator.SetTrigger("AplastarLejos");
                }
                canDestroyPisoHielo = true; //Se habilita el romper piso de hielo
                nextAttackTimer = attackCooldown;
                break;

            case 2: //Arrastrar
                animator.SetTrigger("ArrastrarBajo");
                nextAttackTimer = attackCooldown;
                break;

            case 3: //Lluvia de hielo
                animator.SetTrigger("LluviaHielo");
                nextAttackTimer = attackCooldown;
                break;

            case 4: //Piso de hielo
                animator.SetTrigger("PisoHielo");
                nextAttackTimer = attackCooldown / 4; //Espera menos para realizar otro ataque
                break;
        }

        int bisAttack = Random.Range(0, 2);
        if (bisAttack == 0) animator.SetBool("BisAttack", false);
        if (bisAttack == 1) animator.SetBool("BisAttack", true);        
    }

    #endregion

    #region Triggers Zones
    public void OnPlayerEnterZone (DetectionZone.ZoneType zone)
    {
        if (zone == DetectionZone.ZoneType.Cerca)
        {
            playerInCerca = true;
        }
        /*else if (zone == DetectionZone.ZoneType.Lejos)
        {
            playerInLejos = true;
        }*/
    }
    public void OnPlayerExitZone(DetectionZone.ZoneType zone)
    {
        if (zone == DetectionZone.ZoneType.Cerca)
        {
            playerInCerca = false;
        }
        /*else if (zone == DetectionZone.ZoneType.Lejos)
        {
            playerInLejos = false;
        }*/
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canApplyDamage)
        {
            playerHealth.TakeDamage();
        }
    }

    private void AttackTimerUpdate()
    {
        nextAttackTimer -= Time.deltaTime;
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

    public bool CanDestroyPisoHielo() => canDestroyPisoHielo;
}
