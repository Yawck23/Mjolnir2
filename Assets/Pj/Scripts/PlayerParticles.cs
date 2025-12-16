using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    [SerializeField] GameObject rayoRevivir;

    public void PlayRayoRevivir()
    {
        rayoRevivir.transform.position = transform.position;
        ParticleSystem ps = rayoRevivir.GetComponent<ParticleSystem>();
        ps.Play();
    }
}
