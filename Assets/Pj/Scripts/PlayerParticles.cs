using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    [SerializeField] GameObject rayoRevivir;

    public void PlayRayoRevivir()
    {
        if (rayoRevivir != null)
        {
            rayoRevivir.transform.position = transform.position;
            ParticleSystem ps = rayoRevivir.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
            }
            else
            {
                Debug.LogError("ParticleSystem not found on rayoRevivir GameObject");
            }
        }
        else
        {
            Debug.LogError("rayoRevivir GameObject is not assigned");
        }
    }
}
