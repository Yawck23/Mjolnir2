using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    #region Variables: Health
    [SerializeField] float inmuneTime = 1f;

    private bool isInmune = false;
    private bool isDead = false;

    [SerializeField] KeyCode reviveKey = KeyCode.O;
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
        isDead = false;
        playerController = GetComponent<PlayerController>();
        animatorRoto = transform.Find("Roto").GetComponent<Animator>();
        charController = GetComponent<CharacterController>();
        playerParticles = GetComponent<PlayerParticles>();
    }

    public void TakeDamage()
    {
        if (!isInmune)
        {
            Die();
            StartCoroutine(InmuneCoroutine()); //Inmunidad temporal
        }   
    }

    void Die()
    {
        GameManager.GM.AddToDeathCount();

        gameObjCadera.SetActive(false);
        gameObjRoto.SetActive(true);
        isDead = true;

        StartCoroutine(dieCorutine());

        GameManager.GM.GoToDeathScreen();
    }

    IEnumerator dieCorutine()
    {
        while (playerController.IsGrounded() == false) //Si muere en el aire, esperamos a que toque el piso para desactivar el playerController
        {
            yield return null;
        }
        
        playerController.enabled = false;
        charController.enabled = false;

        while (isDead == true)
        {
            if (Input.GetKeyDown(reviveKey))
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

        gameObjRoto.SetActive(false);
        gameObjCadera.SetActive(true);
        isDead = false;
        //Rehabilitamos controller y movement
        charController.enabled = true;
        playerController.enabled = true;
        playerParticles.PlayRayoRevivir(); //Particulas de revivir

        GameManager.GM.ExitDeathScreen();

        StartCoroutine(InmuneCoroutine()); //Inmunidad temporal

    }
    
    public bool getIsDead()
    {
        return isDead;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("PisoHielo"))
        {
            PisoHieloSpawn pisoHielo = hit.collider.GetComponentInParent<PisoHieloSpawn>();
            float pisoHieloLifeTime = pisoHielo.GetLifeTimer();
            float damagePeriod = pisoHielo.GetDamagePeriod();
            if (pisoHieloLifeTime < damagePeriod)
            {
                TakeDamage(); //Daño si el piso de hielo es muy nuevo
            }
        }
    }
}
