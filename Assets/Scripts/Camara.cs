using Unity.Cinemachine;
using UnityEngine;

public class Camara : MonoBehaviour
{
    [SerializeField] Transform[] targets;
    [SerializeField] float elasticity;
    [SerializeField] Transform targetForcus;

    CinemachineCamera Camera;
    Transform currentTarget;
    int targetIndex = 0;
    

    void Start()
    {
        Camera = GetComponent<CinemachineCamera>();
        currentTarget = targets[targetIndex];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            targetIndex++;
            if (targetIndex >= targets.Length)
            {
                targetIndex = 0;
            }

            currentTarget = targets[targetIndex];
        }

        targetForcus.position = Vector3.Lerp(targetForcus.position, currentTarget.position, elasticity);
        Camera.LookAt = targetForcus;
    }
}

