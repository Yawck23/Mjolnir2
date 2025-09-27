using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    #region Variables: Health
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float inmuneTime = 1f;

    private bool isInmune = false;
    public float currentHealth;
    #endregion


    #region Variables: Components
    private Animator animatorYmir;
    private AttacksManager attackManager;
    #endregion

    void Start()
    {
        currentHealth = maxHealth;
        animatorYmir = GetComponent<Animator>();
        attackManager = GetComponent<AttacksManager>();
    }

    public void TakeDamage(float amount)
    {
        if (!attackManager.CanTakeDamage()) return;
        
        if (!isInmune)
        {
            //Animation take damage falta agregar
            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); //Mantener la vida entre 0 y max
            
            if (currentHealth <= 0)
            {
                Die();
            }

            StartCoroutine(InmuneCoroutine()); //Inmunidad temporal
        }   
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); //Mantener la vida entre 0 y max
    }

    void Die()
    {
        //StartCoroutine(dieCorutine());
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
