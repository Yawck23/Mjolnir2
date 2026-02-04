using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] _3DSounds;
    [SerializeField] private Sound[] _2DSounds;

    #region Scene sounds
    [field: Header ("Scene sounds")]
    [field: SerializeField] public string IceBreak { get; private set; }
    [field: SerializeField] public string StoneBreak1 { get; private set; }
    [field: SerializeField] public string StoneBreak2 { get; private set; }
    #endregion

    #region Ymir Sounds
    [field: Header ("Ymir sounds")]
    [field: SerializeField] public string YmirDialogoInicial { get; private set; }
    [field: SerializeField] public string YmirDialogoStage2 { get; private set; }
    [field: SerializeField] public string YmirHurt1 { get; private set; }
    [field: SerializeField] public string YmirHurt2 { get; private set; }
    [field: SerializeField] public string YmirHurt3 { get; private set; }
    [field: SerializeField] public string YmirLaugh1 { get; private set; }
    [field: SerializeField] public string YmirLaugh2 { get; private set; }
    [field: SerializeField] public string YmirLaugh3 { get; private set; }
    [field: SerializeField] public string YmirJadeo1 { get; private set; }
    [field: SerializeField] public string YmirJadeo2 { get; private set; }
    #endregion

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

        foreach (Sound s in _3DSounds)
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

        foreach (Sound s in _2DSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = 0f;
        }

    }
    #endregion

    public void Play(string name)
    {
        Sound s = System.Array.Find(_2DSounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void Play3DSound(string name, Vector3 position)
    {
        Sound s = System.Array.Find(_3DSounds, sound => sound.name == name);
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
}
