using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject LoadingScreenUI;
    [SerializeField] private Image LoadingBarFill;

    public void LoadScene (string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        LoadingScreenUI.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            LoadingBarFill.fillAmount = progress;
            yield return null;
        }

        LoadingScreenUI.SetActive(false);
    }


}
