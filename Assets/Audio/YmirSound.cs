using UnityEngine;

public class YmirSound : MonoBehaviour
{
    private EnemyHealthSystem healthSystem;

    void Start()
    {
        healthSystem = GetComponent<EnemyHealthSystem>();
    }

    public void PlayDialogoInicial()
    {
        AudioManager.AM.Play(AudioManager.AM.YmirDialogoInicial);
    }

    public void PlayDialogoStage2()
    {
        AudioManager.AM.Play(AudioManager.AM.YmirDialogoStage2);
    }

    public void PlayHurt()
    {
        int hurtSelect = Random.Range(1, 4);
        switch (hurtSelect)
        {
            case 1:
                AudioManager.AM.Play(AudioManager.AM.YmirHurt1);
                break;

            case 2:
                AudioManager.AM.Play(AudioManager.AM.YmirHurt2);
                break;

            case 3:
                AudioManager.AM.Play(AudioManager.AM.YmirHurt3);
                break;
        }
    }

    public void PlayJadeo()
    {
        switch (healthSystem.getActualStage())
        {
            case 1 or 2:
                AudioManager.AM.Play(AudioManager.AM.YmirJadeo1);
                break;

            case 3:
                AudioManager.AM.Play(AudioManager.AM.YmirJadeo2);
                break;
        }
    }

    public void PlayRisa()
    {
        int risaSelect = Random.Range(1, 4);
        switch (risaSelect)
        {
            case 1:
                AudioManager.AM.Play(AudioManager.AM.YmirLaugh1);
                break;

            case 2:
                AudioManager.AM.Play(AudioManager.AM.YmirLaugh2);
                break;

            case 3:
                AudioManager.AM.Play(AudioManager.AM.YmirLaugh3);
                break;
        }
    }
}
