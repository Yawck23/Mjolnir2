using System.Collections;
using DG.Tweening;
using NUnit.Framework;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    #region Variables: Health
    private float maxHealth;
    [SerializeField] float inmuneTime = 1f;

    private bool isDead;

    private bool isInmune = false;
    public float currentHealth;
    #endregion

    #region Variables: Stages
    [SerializeField] float stage1Health = 30f;
    [SerializeField] float stage2Health = 30f;
    [SerializeField] float stage3Health = 30f;
    [SerializeField] float stage4Health = 30f;
    private int actualStage;

    #endregion

    [SerializeField] GameObject stageGameObject;
    private StageChange stageChangeScript;

    #region Variables: Components
    private Animator animatorYmir;
    private AttacksManager attackManager;
    [SerializeField] private Renderer ymirShader;
    [SerializeField] private float ymirCambioTransitionTime = 3f;
    #endregion

    void Start()
    {
        isDead = false;
        actualStage = 1;
        maxHealth = stage1Health + stage2Health + stage3Health + stage4Health;
        currentHealth = maxHealth;
        animatorYmir = GetComponent<Animator>();
        attackManager = GetComponent<AttacksManager>();
        animatorYmir.SetInteger("Stage", actualStage);
        stageChangeScript = stageGameObject.GetComponent<StageChange>();
    }

    public void TakeDamage(float amount, string hitLocation)
    {
        if (amount <= 0f) return;
        if (!attackManager.CanTakeDamage()) return;
        if (isInmune) return;

        //Seleccionamos un hurtAnim al azar, salvo que el hit sea en el pecho, entonces se selecciona el hurtPecho
        int randomHurt = Random.Range(0,3);
        
        if (hitLocation.Equals("Pecho")) randomHurt = 3;
        animatorYmir.SetFloat("HurtSelect", randomHurt);
        
        animatorYmir.SetTrigger("Hurt");
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); //Mantener la vida entre 0 y max

        StageManager();
            
        if (currentHealth <= 0)
        {
            Die();
        }

        StartCoroutine(InmuneCoroutine()); //Inmunidad temporal
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); //Mantener la vida entre 0 y max
    }

    private void Die()
    {
        isDead = true;
        StartCoroutine(dieCorutine());
    }

    private void StageManager (){

        switch (actualStage){
            case 1:
                if ((maxHealth - stage1Health) >= currentHealth){
                    actualStage = 2;
                    StartCoroutine(Stage2Transition());
                    animatorYmir.SetInteger("Stage", actualStage);
                    //La transition to Ice está en un evento de animación
                }
                break;
            
            case 2:
                if (maxHealth - stage1Health - stage2Health >= currentHealth){
                    actualStage = 3;
                    animatorYmir.SetInteger("Stage", actualStage);
                    animatorYmir.SetFloat("IdleSelect", 1);
                    stageChangeScript.TransitionToNormal();
                    ymirShader.material.DOFloat(1f, "_Cambio", ymirCambioTransitionTime);
                }
                break;
            
            case 3:
                if (maxHealth - stage1Health - stage2Health - stage3Health >= currentHealth){
                    actualStage = 4;
                    animatorYmir.SetInteger("Stage", actualStage);
                }
                break;

            case 4:
                break;
        }

    }

    IEnumerator dieCorutine()
    {
        GameManager.GM.Win();
        yield return null;
    }

    private IEnumerator InmuneCoroutine()
    {
        isInmune = true;

        yield return new WaitForSeconds(inmuneTime);

        isInmune = false;
    }

    private IEnumerator Stage2Transition()
    {
        animatorYmir.SetBool("Instance2", true);

        yield return new WaitForSeconds(3f);

        animatorYmir.SetBool("Instance2", false);
    }

    public void TransitionToIceAnimEvent()
    {
        stageChangeScript.TransitionToIce();
    }

    public void TransitionYmirShaderCinematicaFinal()
    {
        ymirShader.material.DOFloat(1f, "_Cambio_2", ymirCambioTransitionTime);
    }

    public int getActualStage() => actualStage;
    public bool IsDead() => isDead;
}
