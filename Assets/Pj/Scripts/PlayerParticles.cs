using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    [SerializeField] private GameObject rayoRevivir;
    [SerializeField] private ParticleSystem polvoMovement, polvoDerrape;
    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        TogglePolvoMovement();
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

    private void TogglePolvoMovement()
    {
        if (playerController.IsGrounded() && playerController.IsMoving())
        {
            if (polvoMovement.isPlaying) return;
            polvoMovement.Play();
        }
        else
        {
            polvoMovement.Stop();
        }
    }

    private void TogglePolvoDerrape()
    {
        if (playerController.IsDerrapando())
        {
            polvoDerrape.Play();
        }
    }

}
