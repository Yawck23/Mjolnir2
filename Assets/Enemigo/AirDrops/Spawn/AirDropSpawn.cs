using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AirDropSpawn : MonoBehaviour
{

    #region Variables: Spawn
    [SerializeField] GameObject drop;
    private Bounds planeBounds;
    [SerializeField] int maxDrops;
    [SerializeField] float minY, maxY, waitForNextDrop;
    #endregion

    #region Variables: SpawnOverlap
    private List<Vector3> listOfSpawnedPoints;
    private Ray rayLeftBack, rayRightBack, rayFrontLeft, rayFrontRight;
    private Vector3 halfObjectExtents;
    private BoxCollider dropCollider;
    private string dropTag;
    private int spawnedObjects = 0;
    public int failures = 0;
    #endregion

    void Start()
    {
        planeBounds = GetComponent<MeshRenderer>().bounds;
        listOfSpawnedPoints = new List<Vector3>();
        dropCollider = drop.GetComponent<BoxCollider>();
        halfObjectExtents = Vector3.Scale(dropCollider.size * 0.5f, drop.transform.localScale);
        halfObjectExtents.y = drop.transform.position.y;
        dropTag = drop.tag;

        StartCoroutine(spawnCorutine());
    }

    private Vector3 getSpawnPoint()
    {
        float x = Random.Range(planeBounds.min.x, planeBounds.max.x);
        float z = Random.Range(planeBounds.min.z, planeBounds.max.z);
        float y = Random.Range(minY, maxY);

        Vector3 spawnPoint = new Vector3(x, y, z);

        return spawnPoint;
    }

    private bool validSpawnPoint(Vector3 spawnObjective)
    {
        bool valid = true;

        rayLeftBack  = new Ray(spawnObjective - Vector3.right * halfObjectExtents.x - Vector3.forward * halfObjectExtents.z, Vector3.down);
        rayRightBack = new Ray(spawnObjective + Vector3.right * halfObjectExtents.x - Vector3.forward * halfObjectExtents.z, Vector3.down);
        rayFrontLeft   = new Ray(spawnObjective - Vector3.right * halfObjectExtents.x + Vector3.forward * halfObjectExtents.z, Vector3.down);
        rayFrontRight  = new Ray(spawnObjective + Vector3.right * halfObjectExtents.x + Vector3.forward * halfObjectExtents.z, Vector3.down);

        if (Physics.Raycast(rayLeftBack, out RaycastHit hit))
        {
            if (hit.collider.CompareTag(dropTag))
            {
                valid = false;
            }
        }

        if (Physics.Raycast(rayRightBack, out RaycastHit hit2))
        {
            if (hit2.collider.CompareTag(dropTag))
            {
                valid = false;
            }
        }

        if (Physics.Raycast(rayFrontLeft, out RaycastHit hit3))
        {
            if (hit3.collider.CompareTag(dropTag))
            {
                valid = false;
            }
        }

        if (Physics.Raycast(rayFrontRight, out RaycastHit hit4))
        {
            if (hit4.collider.CompareTag(dropTag))
            {
                valid = false;
            }
        }

        return valid;
    }

    private void spawnRandomDrop(Vector3 spawnPoint)
    {
        Instantiate(drop, spawnPoint, Quaternion.identity);
    }

    private IEnumerator spawnCorutine()
    {
        while (spawnedObjects < maxDrops && failures < 100)
        {
            Vector3 spawnObjective = getSpawnPoint();
            if (validSpawnPoint(spawnObjective))
            {
                spawnRandomDrop(spawnObjective);
                spawnedObjects++;
                yield return new WaitForSeconds(waitForNextDrop);
            }
            else
            {
                failures++;
                yield return null;
            }
        }
    }
}
