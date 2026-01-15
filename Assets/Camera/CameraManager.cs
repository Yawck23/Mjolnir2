using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform enemy;
    [SerializeField] Transform player;
    [SerializeField] float rotationSmoothSpeed = 180f;
    [SerializeField] float minRadius = 90f;
    [SerializeField] float maxRadius = 180f;
    [SerializeField] float minDistance = 5f;
    [SerializeField] float maxDistance = 25f;

    [SerializeField] Transform[] lookAtTargets;
    [SerializeField] Transform targetFocusDummy;
    [SerializeField] KeyCode targetChangeKey = KeyCode.R;
    private int targetIndex = 0;
    [SerializeField] float focusSmoothSpeed = 5f;

    private CinemachineOrbitalFollow orbital;
    private CinemachineCamera cinemachineCamera;

    void Awake()
    {
        orbital = GetComponent<CinemachineOrbitalFollow>();
        cinemachineCamera = GetComponent<CinemachineCamera>();
        targetFocusDummy.position = lookAtTargets[targetIndex].position;
    }

    void LateUpdate()
    {
        HorizontalCamRotation();
        RadiusCamAdjust();
        FocusTargetSelect();
    }

    private void HorizontalCamRotation()
    {
        // Vector desde el enemigo hacia el player (plano XZ)
        Vector3 dir = player.position - enemy.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.001f)
            return;

        // Ángulo polar
        float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        targetAngle += 180f;

        // Suavizado
        orbital.HorizontalAxis.Value = Mathf.MoveTowardsAngle(
            orbital.HorizontalAxis.Value,
            targetAngle,
            rotationSmoothSpeed * Time.deltaTime
        );
    }

    private void RadiusCamAdjust()
    {
        float distance = Vector3.Distance(player.position, enemy.position);

        float t = Mathf.InverseLerp(minDistance, maxDistance, distance);
        float targetRadius = Mathf.Lerp(minRadius, maxRadius, t);

        orbital.Radius = targetRadius;
    }

    private void FocusTargetSelect()
    {
        if (Input.GetKeyDown(targetChangeKey))
        {
            targetIndex++;
            if (targetIndex >= lookAtTargets.Length)
            {
                targetIndex = 0;
            }
        }
        targetFocusDummy.position = Vector3.Lerp(targetFocusDummy.position, lookAtTargets[targetIndex].position, focusSmoothSpeed * Time.deltaTime);
    }

    public void GoToTarget(int target)
    {
        Debug.Log("Going to target: " + target);
        targetIndex = target;
        targetFocusDummy.position = Vector3.Lerp(targetFocusDummy.position, lookAtTargets[targetIndex].position, focusSmoothSpeed * Time.deltaTime);
    }
}
