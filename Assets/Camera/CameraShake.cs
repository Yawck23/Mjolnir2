using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private GameObject manoDerechaYmir;
    private CinemachineImpulseSource manoDerechaYmirImpulse;
    void Start()
    {
        manoDerechaYmirImpulse = manoDerechaYmir.GetComponent<CinemachineImpulseSource>();
    }

    // Update is called once per frame
    public void ShakeCamera()
    {
        manoDerechaYmirImpulse.GenerateImpulse();
    }
}
