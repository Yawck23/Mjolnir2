using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    [SerializeField] private GameObject rayoRevivir;
    [SerializeField] private ParticleSystem polvoDerrape;
    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        TogglePolvoDerrape();
    }

    public void PlayRayoRevivir()
    {
        rayoRevivir.transform.position = transform.position;

        foreach (Transform child in rayoRevivir.transform)
        {
            ParticleSystem ps = child.GetComponent<ParticleSystem>();
            ps.Play();
        }
    }

    private void TogglePolvoDerrape()
    {
        if (playerController.IsGrounded() && playerController.IsMoving())
        {
            if (polvoDerrape.isPlaying) return;
            polvoDerrape.Play();
        }
        else
        {
            polvoDerrape.Stop();
        }
    }
    
}
