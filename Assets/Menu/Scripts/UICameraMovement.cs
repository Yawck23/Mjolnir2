using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class UICameraMovement : MonoBehaviour
{

    [SerializeField] GameObject mainCamera, lvlSelectCamera;
    [SerializeField] float transitionDuration = 3f;

    private Vector3 mainCameraPos, lvlSelectCameraPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        mainCameraPos = mainCamera.transform.position;
        lvlSelectCameraPos = lvlSelectCamera.transform.position;
    }
    public void goToLvlSelectCamera()
    {
        
        
        StartCoroutine(CameraLerpCoroutine(mainCameraPos, lvlSelectCameraPos, transitionDuration));
    }

    public void goToMainMenuCamera()
    {
        if (mainCamera.transform.position.Equals(mainCameraPos)) return; //Evita que se mueva la cámara al iniciar el juego
        StartCoroutine(CameraLerpCoroutine(lvlSelectCameraPos, mainCameraPos, transitionDuration));
    }

    private IEnumerator CameraLerpCoroutine(Vector3 start, Vector3 target, float lerpDuration)
    {
        float timeElapsed = 0f;

        while (timeElapsed < lerpDuration)
        {
            mainCamera.transform.position = Vector3.Lerp(start, target, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = target;
    }

    public void DisableMainMenuCameras() //Se desactivan porque estan en el DontDestroyOnLoad
    {
        mainCamera.SetActive(false);
        lvlSelectCamera.SetActive(false);
    }
    
    public void EnableMainMenuCameras()
    {
        mainCamera.SetActive(true);
        lvlSelectCamera.SetActive(true);
    }
}
