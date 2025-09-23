using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float reviveHeal = 30f;
    public float currentHealth;
    private PlayerController playerController;
    [SerializeField] GameObject cadera;
    [SerializeField] GameObject roto;
    private Animator animatorRoto;

    void Start()
    {
        currentHealth = maxHealth;
        playerController = GetComponent<PlayerController>();
        animatorRoto = transform.Find("Roto").GetComponent<Animator>();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); //Mantener la vida entre 0 y max

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); //Mantener la vida entre 0 y max
    }

    void Die()
    {
        cadera.SetActive(false);
        roto.SetActive(true);
        playerController.enabled = false;
        StartCoroutine(dieCorutine());
    }

    IEnumerator dieCorutine()
    {
        while (currentHealth <= 0)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                Revive();
            }
            yield return null;
        }
    }


    void Revive()
    {
        animatorRoto.SetTrigger("Revive");
        Heal(reviveHeal);
        roto.SetActive(false);
        cadera.SetActive(true);
        playerController.enabled = true;
        
    }
}
