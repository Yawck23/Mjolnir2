using Unity.VisualScripting;
using UnityEngine;

public class HeadLookAt : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private Transform targetLookAt;
    [SerializeField] private Transform parentCharacter;
    [SerializeField] private float offsetZ = -90f;
    [SerializeField] private float minAngle = 0f;
    [SerializeField] private float maxAngle = 180f;
    public float angleToYmir;
    private Quaternion finalRotation;

    void Update()
    {
        if (targetLookAt == null) return;

        // Aplicar el offset de rotación en Z
        Quaternion offsetRotation = Quaternion.AngleAxis(offsetZ, Vector3.forward);

        if (IsInAngleRange())
        {
            // Crear la rotación inicial mirando hacia el objetivo
            Quaternion lookRotation = Quaternion.LookRotation(targetLookAt.position - transform.position);            
            finalRotation = lookRotation * offsetRotation;            
        }
        else
        {
            // Mantener la rotación actual si no está dentro del rango
            finalRotation = parentCharacter.rotation * offsetRotation;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, finalRotation, Time.deltaTime * rotationSpeed);

    }

    private bool IsInAngleRange()
    {
        angleToYmir = Vector3.Angle(parentCharacter.forward, targetLookAt.position - parentCharacter.position);
        if (angleToYmir >= minAngle && angleToYmir <= maxAngle)
        {
            return true;
        }
        else
        {
            return false;
        }
    }



}
