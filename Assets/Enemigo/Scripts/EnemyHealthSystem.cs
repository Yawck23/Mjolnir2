using System.Collections;
using NUnit.Framework;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    #region Variables: Health
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float inmuneTime = 1f;

    private bool isDead;

    private bool isInmune = false;
    public float currentHealth;
    #endregion

    #region Variables: Stages
    [SerializeField] float damageToStage2 = 20f;
    [SerializeField] float damageToStage3 = 30f;
    [SerializeField] float damageToStage4 = 30f;
    private int actualStage;

    [SerializeField] GameObject iceDome;

    #endregion

    #region Variables: Components
    private Animator animatorYmir;
    private AttacksManager attackManager;
    #endregion

    void Start()
    {
        isDead = false;
        actualStage = 1;
        currentHealth = maxHealth;
        animatorYmir = GetComponent<Animator>();
        attackManager = GetComponent<AttacksManager>();
        animatorYmir.SetInteger("Stage", actualStage);
    }

    public void TakeDamage(float amount)
    {
        if (amount <= 0f) return;

        if (!attackManager.CanTakeDamage()) return;
        
        if (isInmune) return;
        
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
                if ((maxHealth - damageToStage2) >= currentHealth){
                    actualStage = 2;
                    Debug.Log("Cambio a stage 2");
                    animatorYmir.SetInteger("Stage", actualStage);
                    iceDome.SetActive(true);
                }
                break;
            
            case 2:
                if (maxHealth - damageToStage2 - damageToStage3 >= currentHealth){
                    actualStage = 3;
                    Debug.Log("Cambio a stage 3");
                    animatorYmir.SetInteger("Stage", actualStage);
                }
                break;
            
            case 3:
                if (maxHealth - damageToStage2 - damageToStage3 - damageToStage4 >= currentHealth){
                    actualStage = 4;
                    Debug.Log("Cambio a stage 4");
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

    public int getActualStage() => actualStage;
    public bool IsDead() => isDead;
}
