using UnityEngine;

public class AttacksManager : MonoBehaviour
{
    private Animator animator;
    private HealthSystem playerHealth;

    private bool canApplyDamage;
    private bool canTakeDamage;
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<HealthSystem>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetTrigger("AplastarLejos");
    }

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canApplyDamage)
        {
            playerHealth.TakeDamage(30);
        }
    }
}
