using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UICameraMovement : MonoBehaviour
{

    [Header("Scene / Names")]
    [SerializeField] string mainMenuSceneName = "MainMenu";
    [SerializeField] string mainCameraName = "MainCamera";
    [SerializeField] string lvlSelectCameraName = "LvlSelectCamera";

    [SerializeField] float transitionDuration = 3f;

    private GameObject mainCamera, lvlSelectCamera;
    private Vector3 mainCameraPos, lvlSelectCameraPos;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        StopAllCoroutines();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == mainMenuSceneName)
        {
            mainCamera = GameObject.Find(mainCameraName);
            lvlSelectCamera = GameObject.Find(lvlSelectCameraName);

            if (mainCamera != null && lvlSelectCamera != null)
            {
                mainCameraPos = mainCamera.transform.position;
                lvlSelectCameraPos = lvlSelectCamera.transform.position;
            }
        }
    }

    public void goToLvlSelectCamera()
    {
        if (mainCamera == null || lvlSelectCamera == null) return;
        StartCoroutine(CameraLerpCoroutine(mainCameraPos, lvlSelectCameraPos, transitionDuration));
    }

    public void goToMainMenuCamera()
    {
        if (mainCamera == null || lvlSelectCamera == null) return;

        if (Vector3.Distance(mainCamera.transform.position, mainCameraPos) < 0.001f) return;
        StartCoroutine(CameraLerpCoroutine(lvlSelectCameraPos, mainCameraPos, transitionDuration));
    }

    private IEnumerator CameraLerpCoroutine(Vector3 start, Vector3 target, float lerpDuration)
    {
        float timeElapsed = 0f;

        while (timeElapsed < lerpDuration)
        {
            if (mainCamera == null) yield break; //Por si se cambia de escena en medio de la transicion

            mainCamera.transform.position = Vector3.Lerp(start, target, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = target;
    }
}
