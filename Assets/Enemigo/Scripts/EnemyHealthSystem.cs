using System.Collections;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    #region Variables: Health
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float inmuneTime = 1f;

    private bool isInmune = false;
    public float currentHealth;
    #endregion

    #region Variables: Stages
    [SerializeField] float damageToStage2 = 30f;
    [SerializeField] float damageToStage3 = 40f;
    private int actualStage;

    #endregion


    #region Variables: Components
    private Animator animatorYmir;
    private AttacksManager attackManager;
    #endregion

    void Start()
    {
        actualStage = 1;
        currentHealth = maxHealth;
        animatorYmir = GetComponent<Animator>();
        attackManager = GetComponent<AttacksManager>();
    }

    public void TakeDamage(float amount)
    {
        if (amount <= 0f) return;

        if (!attackManager.CanTakeDamage()) return;
        
        if (isInmune) return;
        
        animatorYmir.SetTrigger("Hurt");
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); //Mantener la vida entre 0 y max
        Debug.Log("Stage actual =" +actualStage);
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
        //StartCoroutine(dieCorutine());
    }

    private void StageManager (){

        switch (actualStage){
            case 1:
                if ((maxHealth - damageToStage2) >= currentHealth){
                    actualStage = 2;
                    Debug.Log("Cambio a stage 2");
                    animatorYmir.SetInteger("Stage", actualStage);
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
                
                break;
        }

    }

    /*IEnumerator dieCorutine()
    {
        //Falta implementar
    }*/

    private IEnumerator InmuneCoroutine()
    {
        isInmune = true;

        yield return new WaitForSeconds(inmuneTime);

        isInmune = false;
    }
}
