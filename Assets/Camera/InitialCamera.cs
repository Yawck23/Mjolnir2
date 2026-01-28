using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class InitialCamera : MonoBehaviour
{
    private CinemachineCamera initialCam;
    [SerializeField] private GameObject endPoint;
    [SerializeField] private float moveDuration = 2f;
    [SerializeField] private GameObject mjolnirInitial, mjolnirFinal;
    [SerializeField] private ParticleSystem landVfx;
    [SerializeField] private CinemachineImpulseSource cameraShake;
    [SerializeField] private Transform mjolnirInitialSpawnPoint, mjolnirFinalSpawnPoint;
    void Start()
    {
        initialCam = GetComponentInChildren<CinemachineCamera>();
        StartCoroutine(CameraMove());
    }

    private IEnumerator CameraMove()
    {
        //Movemos el mjolnir real a la carcel inicial
        mjolnirFinal.transform.position = mjolnirInitialSpawnPoint.position;
        Quaternion mjolnirRotation = mjolnirFinal.transform.rotation;
        yield return new WaitForSeconds(0.5f);

        Vector3 startPosition = transform.position;
        Vector3 endPosition = endPoint.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            cameraShake.GenerateImpulse();
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / moveDuration);
            yield return null;
        }
        landVfx.Play();
        mjolnirInitial.SetActive(false);
        
        //Devolvemos el mjolnir real a su posicion final
        mjolnirFinal.transform.position = mjolnirFinalSpawnPoint.position;
        mjolnirFinal.transform.rotation = mjolnirRotation;
        Physics.SyncTransforms(); //Esto no se bien que hace, pero es necesario para que no haya problemas de fisicas al mover al personaje animado.

        initialCam.Priority = 0;
    }
}
