using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class YmirCinematicaFinal : MonoBehaviour
{
    //Este script es una basura total, una aberración en el mundo de la programación. Perdón.
    //Tiene que estar en el Ymir para poder escuchar los animation events... Perdón de nuevo.

    [SerializeField] private GameObject colmilloInicial, colmilloCinematica;
    [SerializeField] private PlayableDirector CinematicaFinalPlayable;
    [SerializeField] private CinemachineCamera cinematicCamera;
    [SerializeField] private BandasNegrasCamera bandasNegras;
    [SerializeField] private KeyCode keyCodeTriggerCinematic = KeyCode.Q;
    private bool cinematicTriggered = false;

    private EnemyHealthSystem healthSystem;

    void Start()
    {
        colmilloCinematica.SetActive(false);
        healthSystem = GetComponent<EnemyHealthSystem>();
    }

    void Update()
    {
        if (Input.GetKeyDown(keyCodeTriggerCinematic) && !cinematicTriggered && healthSystem.IsDead())
        {
            StartCoroutine(TriggerCinematicaFinal());
        }
    }

    public IEnumerator TriggerCinematicaFinal()
    {
        UIManager.UIM.ToggleCinematicTriggerPanel(false);
        cinematicTriggered = true;
        cinematicCamera.Priority = 4; //Prioridad alta para que el cinemachine la tome
        bandasNegras.ToggleBars(); //Ponemos las bandas negras
        CinematicaFinalPlayable.enabled = true; //Habilitamos la timeline con el play on wake
        float duration = (float)CinematicaFinalPlayable.duration;
        float timer = 0f;

        while (timer < duration) //Iteramos mientras dura la animación
        {
            timer += Time.deltaTime;
            yield return null;
        }

        bandasNegras.ToggleBars(); //Sacamos las bandas negras
        Time.timeScale = 1f; //Volvemos el time scale a 1
        //Lógica de Win (fade a negro)
        GameManager.GM.Win();
        //cinematicCamera.Priority = 0; //Volvemos a la cámara que corresponda
    }

    public void SetTimeScale (float timeScale) //De verdad, perdón
    {
        Debug.Log("Llamada a SetTimeScale con valor: " + timeScale);
        Time.timeScale = timeScale;
    }

    public void EnableColmilloCinematica()
    {
        colmilloCinematica.SetActive(true);
    }
}
