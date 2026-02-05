using UnityEngine;

public class MenuAudio : MonoBehaviour
{
    public void PlaySeleccionNivel()
    {
        AudioManager.AM.Play(AudioManager.AM.SeleccionNivel);
    }

    public void PlayClickPlay()
    {
        AudioManager.AM.Play(AudioManager.AM.ClickPlay);
    }

    public void PlayClickButton()
    {
        AudioManager.AM.Play(AudioManager.AM.ClickButton);
    }
}
