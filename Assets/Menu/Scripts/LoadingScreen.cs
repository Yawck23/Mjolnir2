using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject LoadingScreenUI;
    [SerializeField] private Image LoadingBarFill;
    [SerializeField] private float loadSpeed = 0.5f;

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
        GameManager.GM.TogglePause(false);
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        LoadingScreenUI.SetActive(true);
        LoadingBarFill.fillAmount = 0;

        //Corremos mientras la carga no termine o mientras la barra no haya llegado al 100%
        while (!operation.isDone || LoadingBarFill.fillAmount < 1.0f)
        {
            // Calculamos el progreso real (0 a 1)
            float targetProgress = Mathf.Clamp01(operation.progress / 0.9f);

            // Forzamos que la barra avance de forma constante. 
            LoadingBarFill.fillAmount = Mathf.MoveTowards(
                LoadingBarFill.fillAmount,
                targetProgress,
                loadSpeed * Time.deltaTime
            );

            //Si la carga interna ya terminó (0.9), pero la barra no ha llegado a 1, seguimos animando la barra.
            if (operation.progress >= 0.9f && LoadingBarFill.fillAmount >= 0.99f)
            {
                operation.allowSceneActivation = true;
                LoadingBarFill.fillAmount = 1.0f; // Aseguramos el final
                yield return new WaitForSeconds(0.2f); // Un pequeño respiro visual
                
                break;
            }
            

            yield return null;
        }
        LoadingScreenUI.SetActive(false);
    }


}
