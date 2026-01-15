using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private GameObject manoDerechaYmir;
    [SerializeField] private GameObject bossFightCamera;
    private CinemachineImpulseSource manoDerechaYmirImpulse;
    private CameraManager cameraManager;
    void Start()
    {
        manoDerechaYmirImpulse = manoDerechaYmir.GetComponent<CinemachineImpulseSource>();
        cameraManager = bossFightCamera.GetComponent<CameraManager>();
    }

    // Update is called once per frame
    public void ShakeCamera()
    {
        manoDerechaYmirImpulse.GenerateImpulse();
    }

    public void GoToTarget(int target)
    {
        cameraManager.GoToTarget(target);
    }
}
