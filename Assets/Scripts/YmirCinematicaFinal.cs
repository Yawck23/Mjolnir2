using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class YmirCinematicaFinal : MonoBehaviour
{
    //Este script es una basura total, una aberración en el mundo de la programación. Perdón.
    //Tiene que estar en el Ymir para poder escuchar los animation events... Perdón de nuevo.

    [SerializeField] private GameObject colmilloInicial, colmilloCinematica;
    [SerializeField] private PlayableDirector CinematicaFinalPlayable;
    private bool cinematicTriggered = false;
    private bool colmilloDesactivado = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !cinematicTriggered)
        {
            StartCoroutine(TriggerCinematicaFinal());
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            ToggleColmillos();
        }

        if (colmilloDesactivado)
        {
            colmilloInicial.SetActive(false);
        }
    }

    public IEnumerator TriggerCinematicaFinal()
    {
        cinematicTriggered = true;
        CinematicaFinalPlayable.enabled = true;
        float duration = (float)CinematicaFinalPlayable.duration;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        Time.timeScale = 1f;
        Debug.Log("Se resetea el time scale a 1");
    }

    public void ToggleColmillos()
    {
        Debug.Log("Se hizo toggle de colmillos");
        colmilloDesactivado = true;
        //colmilloInicial.SetActive(false);
        //colmilloCinematica.SetActive(true);

    }

    public void SetTimeScale (float timeScale) //De verdad, perdón
    {
        Debug.Log("Llamada a SetTimeScale con valor: " + timeScale);
        Time.timeScale = timeScale;
    }
}
