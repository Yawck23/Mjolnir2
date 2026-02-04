using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;

    #region Singleton y Seteo inicial de sonidos
    public static AudioManager AM { get; private set; }

    private void Awake()
    {
        // Se revisa si ya existe un objeto llamado AM
        if (AM != null && AM != this)
        {
            // Si ya existe, este objeto se destruye a s� mismo ya que no puede haber dos instancias de un elemento est�tico
            Destroy(gameObject);
        }
        else
        {
            // Si este es el �nico elemento AudioManager se asigna a la variable AM
            AM = this;
            // Se pone en un modo que evita ser destruido al cambiar de escena
            DontDestroyOnLoad(gameObject);
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = s.spatialBlend;
            s.source.minDistance = s.minDistance;
            s.source.maxDistance = s.maxDistance;
            s.source.rolloffMode = AudioRolloffMode.Linear; //Para atenuar el sonido 3D
        }

    }
    #endregion

    /*public void Play(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }*/

    /*public void Play(string name, float volume)
    {
        float realVolume = Mathf.Clamp(volume, 0, 1); //Lo mantenemos entre 0 y 1
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        float anteriorVolume = s.source.volume; //Guardamos el valor del volumen configurado del clip
        s.source.volume = realVolume;
        s.source.Play();
        s.source.volume = anteriorVolume; //Devolvemos el volumen real al clip
    }*/

    public void Play3DSound(string name, Vector3 position)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        // Crea un objeto vacío, le pone un AudioSource, lo configura y lo destruye al terminar
        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.position = position;
        AudioSource source = tempGO.AddComponent<AudioSource>();

        // Configuración rápida
        source.clip = s.clip;
        source.volume = s.volume;
        source.spatialBlend = s.spatialBlend; // 3D
        source.minDistance = s.minDistance;
        source.maxDistance = s.maxDistance;
        source.Play();

        Destroy(tempGO, s.clip.length); // Se limpia solo
    }

    /*public void Stop(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    public void StopAllSounds()
    {
        foreach (Sound s in sounds)
        {
            s.source.Stop();
        }
    }*/
}
