using UnityEngine;
using UnityEngine.Audio;


[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 0.7f;

    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    public bool loop = false;
    
    [Range(0.0f, 1.0f)]
    public float spatialBlend = 0f;

    [HideInInspector]
    public AudioSource source;
}
