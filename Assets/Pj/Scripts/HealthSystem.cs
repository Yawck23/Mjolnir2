using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    #region Variables: Health
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float reviveHeal = 30f;
    [SerializeField] float inmuneTime = 1f;

    private bool isInmune = false;
    public float currentHealth;
    private bool isDead = false;
    #endregion


    #region Variables: Components
    private PlayerController playerController;
    [SerializeField] GameObject gameObjCadera;
    [SerializeField] GameObject gameObjRoto;
    private Animator animatorRoto;
    private CharacterController charController;
    
    #endregion

    #region Player Particles
    private PlayerParticles playerParticles;
    #endregion

    void Start()
    {
        currentHealth = maxHealth;
        playerController = GetComponent<PlayerController>();
        animatorRoto = transform.Find("Roto").GetComponent<Animator>();
        charController = GetComponent<CharacterController>();
        playerParticles = GetComponent<PlayerParticles>();
    }

    public void TakeDamage(float amount)
    {
        if (!isInmune)
        {
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
        GameManager.GM.AddToDeathCount();

        gameObjCadera.SetActive(false);
        gameObjRoto.SetActive(true);
        isDead = true;

        StartCoroutine(dieCorutine());
    }

    IEnumerator dieCorutine()
    {
        while (playerController.IsGrounded() == false) //Si muere en el aire, esperamos a que toque el piso para desactivar el playerController
        {
            yield return null;
        }
        
        playerController.enabled = false;
        charController.enabled = false;

        while (currentHealth <= 0)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                Revive();
            }
            yield return null;
        }
    }

    private IEnumerator InmuneCoroutine()
    {
        isInmune = true;

        yield return new WaitForSeconds(inmuneTime);

        isInmune = false;
    }

    void Revive()
    {
        animatorRoto.SetTrigger("Revive"); //Animaci�n de revivir
        Heal(reviveHeal); //Curar

        gameObjRoto.SetActive(false);
        gameObjCadera.SetActive(true);
        isDead = false;
        //Rehabilitamos controller y movement
        charController.enabled = true;
        playerController.enabled = true;
        playerParticles.PlayRayoRevivir(); //Particulas de revivir
        StartCoroutine(InmuneCoroutine()); //Inmunidad temporal

    }
    
    public bool getIsDead()
    {
        return isDead;
    }
}
