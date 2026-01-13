using System.Collections;
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
        maxHealth = stage1Health + stage2Health + stage3Health + stage4Health;
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

        Debug.Log("Ymir recibe " + amount + " de daño.");
        
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
                    Debug.Log("Cambio a stage 2");
                    animatorYmir.SetInteger("Stage", actualStage);
                    iceDome.SetActive(true);
                }
                break;
            
            case 2:
                if (maxHealth - stage1Health - stage2Health >= currentHealth){
                    actualStage = 3;
                    Debug.Log("Cambio a stage 3");
                    animatorYmir.SetInteger("Stage", actualStage);
                }
                break;
            
            case 3:
                if (maxHealth - stage1Health - stage2Health - stage3Health >= currentHealth){
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
