using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    #region Variables: Health
    [SerializeField] float inmuneTime = 1f;

    private bool isInmune = false;
    private bool isDead = false;

    [SerializeField] KeyCode reviveKey = KeyCode.O;
    [SerializeField] float waitTimeForReviveAnimation, waitForReviveInput;
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
        MusicManager.Instance.PlayMusic(MusicManager.Instance.Muerte);
        AudioManager.AM.Play(AudioManager.AM.MjolnirDeath);
        AudioManager.AM.Play(SelectRisaYmir());

        
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

        yield return new WaitForSeconds(waitForReviveInput); //Pequeña espera para evitar que se pueda revivir instantaneamente

        while (isDead == true)
        {
            if (Input.GetKeyDown(reviveKey))
            {
                StartCoroutine(Revive());
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

    private IEnumerator Revive()
    {
        isDead = false;
        GameManager.GM.ExitDeathScreen();
        MusicManager.Instance.PlayLastMusicBeforeActual();
        animatorRoto.SetTrigger("Revive"); //Animación de revivir
        playerParticles.PlayRayoRevivir(); //Particulas de revivir
        AudioManager.AM.Play(AudioManager.AM.MjolnirRevivir);
        
        yield return new WaitForSeconds(waitTimeForReviveAnimation); //Esperamos a que termine la animacion

        gameObjRoto.SetActive(false);
        gameObjCadera.SetActive(true);
        
        //Rehabilitamos controller y movement
        charController.enabled = true;
        playerController.enabled = true;


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

    private string SelectRisaYmir()
    {
        string risa = null;
        int randomRisa = Random.Range(1,4);
        switch (randomRisa)
        {
            case 1:
                risa = AudioManager.AM.YmirLaugh1;
                break;

            case 2:
                risa = AudioManager.AM.YmirLaugh2;
                break;

            case 3:
                risa = AudioManager.AM.YmirLaugh3;
                break;
        }

        return risa;
    }
}
