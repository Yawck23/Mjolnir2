using UnityEngine;

public class ExplosionIntoParts : MonoBehaviour
{
    public enum AudioClip { IceBreak, StoneBreak }
    [SerializeField] private float dissapearTimeMin, dissapearTimeMax;
    [SerializeField] private float explosionForce = 10000f;
    [SerializeField] private float explosionRadius = 450f;
    [SerializeField] private Transform explosionPoint;
    [SerializeField] private GameObject hitMarket;
    [SerializeField] private AudioClip audioClipName;
    private float dissapearTime;

    //Este script requiere un obejto padre con rigidbody y collider y varios hijos con rigidbody.
    //Los rigidbody de los hijos deben estar en isKinematic true.

    public void Explosion()
    {        
        BoxCollider bc = GetComponent<BoxCollider>();
        bc.enabled = false;
        AudioManager.AM.Play3DSound(AudioName(), AudioPosition());

        foreach (Transform child in transform) //Aplicamos la explosion a cada rigidbody hijo
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb == null) continue;

            dissapearTime = Random.Range(dissapearTimeMin, dissapearTimeMax);

            rb.isKinematic = false;
            rb.AddExplosionForce(explosionForce, explosionPoint.position, explosionRadius);
            Destroy(child.gameObject, dissapearTime);
        }

        if (hitMarket != null) //Si tiene hitMarket, lo destruimos para que no quede en escena
        {
            Destroy(hitMarket);
        }

        Destroy(this.gameObject, dissapearTimeMax + 1f); //Destruimos el objeto padre despues de que todos los hijos hayan desaparecido
    }

    private Vector3 AudioPosition()
    {
        if (audioClipName.Equals(AudioClip.StoneBreak))
        {
            return transform.parent.position;
        }
        else
        {
            return transform.position;
        }
    }

    private string AudioName()
    {
        string audioName = null;
        if (audioClipName.Equals(AudioClip.IceBreak)) audioName = AudioManager.AM.IceBreak;

        if (audioClipName.Equals(AudioClip.StoneBreak))
        {
            int randomBreak = Random.Range(1, 3);
            switch (randomBreak)
            {
                case 1:
                    audioName = AudioManager.AM.StoneBreak1;
                    break;
                
                case 2:
                    audioName = AudioManager.AM.StoneBreak2;
                    break;     
            }
        }

        return audioName;
    }
}
