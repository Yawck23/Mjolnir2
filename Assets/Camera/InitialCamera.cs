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
    void Start()
    {
        initialCam = GetComponentInChildren<CinemachineCamera>();
        StartCoroutine(CameraMove());
    }

    private IEnumerator CameraMove()
    {
        yield return new WaitForSeconds(1f);

        mjolnirFinal.SetActive(false);

        Vector3 startPosition = transform.position;
        Vector3 endPosition = endPoint.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / moveDuration);
            yield return null;
        }
        landVfx.Play();
        mjolnirInitial.SetActive(false);
        mjolnirFinal.SetActive(true);

        initialCam.Priority = 0;
    }
}
