using Unity.Cinemachine;
using UnityEngine;

public class NewCameraTest : MonoBehaviour
{
    [SerializeField] Transform enemy;
    [SerializeField] Transform player;
    [SerializeField] float smoothSpeed = 180f;
    [SerializeField] float minRadius = 90f;
    [SerializeField] float maxRadius = 180f;
    [SerializeField] float minDistance = 5f;
    [SerializeField] float maxDistance = 25f;

    private CinemachineOrbitalFollow orbital;

    void Awake()
    {
        orbital = GetComponent<CinemachineOrbitalFollow>();
    }

    void LateUpdate()
    {
        HorizontalCamRotation();
        RadiusCamAdjust();
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
            smoothSpeed * Time.deltaTime
        );
    }

    private void RadiusCamAdjust()
    {
        float distance = Vector3.Distance(player.position, enemy.position);

        float t = Mathf.InverseLerp(minDistance, maxDistance, distance);
        float targetRadius = Mathf.Lerp(minRadius, maxRadius, t);

        orbital.Radius = targetRadius;
    }

}
