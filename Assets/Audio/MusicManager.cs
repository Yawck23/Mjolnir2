using DG.Tweening;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;

    [Header("Configuración de Escenas")]
    [field: SerializeField] public string MenuInicio { get; private set; }
    [field: SerializeField] public string Tutorial { get; private set; }
    [field: SerializeField] public string Ymir1 { get; private set; }
    [field: SerializeField] public string Ymir2 { get; private set; }
    [field: SerializeField] public string Ymir3 { get; private set; }
    [field: SerializeField] public string Muerte { get; private set; }
    [field: SerializeField] public string PostCinematica { get; private set; }

    private Sound currentlyPlaying;
    private Sound playedBefore;
    [SerializeField] private float fadeDuration = 2f;

    #region Singleton y Seteo inicial de sonidos
    public static MusicManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = 1f;
            s.source.loop = s.loop;
            s.source.spatialBlend = 0f;
        }

    }
    #endregion

    public void PlayMusic(string soundName)
    {
        //Buscamos el sonido, y si lo encontramos hacemos la transición
        Sound toBePlayed = System.Array.Find(sounds, sound => sound.name == soundName);
        if (toBePlayed == null)
        {
            Debug.Log($"Sonido {soundName} no encontrado");
            return;
        }

        float targetVolume = toBePlayed.volume; //Esto devuelve el valor inicial configurado


        if (soundName.Equals(Muerte)) //El sonido de muerte queremos que se reproduzca a volumen total desde el inicio, y se corte la musica instantaneamente
        {
            toBePlayed.source.volume = targetVolume;
            if (currentlyPlaying != null) currentlyPlaying.source.volume = 0f;
        }
        else
        {
            if (currentlyPlaying != null) currentlyPlaying.source.DOFade(0, fadeDuration);
            toBePlayed.source.volume = 0f;
            toBePlayed.source.DOFade(targetVolume, fadeDuration);
        }

        toBePlayed.source.Play();

        playedBefore = currentlyPlaying;
        currentlyPlaying = toBePlayed;
    }

    public void PlayLastMusicBeforeActual()
    {
        //Buscamos el sonido, y si lo encontramos hacemos la transición
        Sound toBePlayed = playedBefore;
        if (toBePlayed == null)
        {
            Debug.Log("Sonido para reproducir anterior no encontrado");
            return;
        }

        float targetVolume = toBePlayed.volume; //Esto devuelve el valor inicial configurado

        if (currentlyPlaying != null) currentlyPlaying.source.DOFade(0, fadeDuration);

        toBePlayed.source.volume = 0f;
        toBePlayed.source.Play();
        toBePlayed.source.DOFade(targetVolume, fadeDuration);

        playedBefore = currentlyPlaying;
        currentlyPlaying = toBePlayed;
    }
}
