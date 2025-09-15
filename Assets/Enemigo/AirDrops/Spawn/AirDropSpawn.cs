using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AirDropSpawn : MonoBehaviour
{

    #region Variables: Spawn
    [SerializeField] GameObject drop; //Objeto a dropear
    private Bounds planeBounds; //Los limites donde dropean
    [SerializeField] int maxDrops; //Drops máximos
    [SerializeField] float minY, maxY; //Altura minima y máxima para el drop
    [SerializeField] float waitForNextDrop; //Tiempo de espera entre drops
    #endregion

    #region Variables: SpawnOverlap
    private Ray rayLeftBack, rayRightBack, rayLeftFront, rayRightFront; //Raycast en las esquinas del spawn
    private Vector3 halfObjectExtents; //Para calcular la mitad del objeto
    private BoxCollider dropCollider;
    private string dropTag;
    private int spawnedObjects = 0; //Para contar la cantidad de items spawneados
    public int failures = 0; //Si falla muchas veces, deja de spawnerar objetos, por más que no se llegue al maxDrops
    #endregion

    #region Variables: playerFollow
    [SerializeField] private Transform player;
    #endregion
    void Start()
    {
        //Movemos el spawn al player al start y en cada update
        transform.position = new Vector3(player.position.x, 0.0f, player.position.z);
        planeBounds = GetComponent<MeshRenderer>().bounds;

        dropCollider = drop.GetComponent<BoxCollider>();
        halfObjectExtents = Vector3.Scale(dropCollider.size * 0.5f, drop.transform.localScale);
        halfObjectExtents.y = drop.transform.position.y;
        dropTag = drop.tag;
        StartCoroutine(spawnCorutine());
    }

    void Update()
    {
        transform.position = new Vector3(player.position.x, 0.0f, player.position.z);
        planeBounds = GetComponent<MeshRenderer>().bounds;
        
    }

    private Vector3 getSpawnPoint()
    {
        //Devuelve un posible punto de Spawn

        float x = Random.Range(planeBounds.min.x, planeBounds.max.x);
        float z = Random.Range(planeBounds.min.z, planeBounds.max.z);
        float y = Random.Range(minY, maxY);

        Vector3 spawnPoint = new Vector3(x, y, z);

        return spawnPoint;
    }

    private bool validSpawnPoint(Vector3 spawnObjective)
    {
        //Valida si no hay otro objeto debajo con el mismo tag
        bool valid = true;

        //Raycast en cada esquina
        rayLeftBack  = new Ray(spawnObjective - Vector3.right * halfObjectExtents.x - Vector3.forward * halfObjectExtents.z, Vector3.down);
        rayRightBack = new Ray(spawnObjective + Vector3.right * halfObjectExtents.x - Vector3.forward * halfObjectExtents.z, Vector3.down);
        rayLeftFront   = new Ray(spawnObjective - Vector3.right * halfObjectExtents.x + Vector3.forward * halfObjectExtents.z, Vector3.down);
        rayRightFront  = new Ray(spawnObjective + Vector3.right * halfObjectExtents.x + Vector3.forward * halfObjectExtents.z, Vector3.down);

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

        if (Physics.Raycast(rayLeftFront, out RaycastHit hit3))
        {
            if (hit3.collider.CompareTag(dropTag))
            {
                valid = false;
            }
        }

        if (Physics.Raycast(rayRightFront, out RaycastHit hit4))
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
        //Spawnea un objeto en una posisción dada
        Instantiate(drop, spawnPoint, Quaternion.identity);
    }

    private IEnumerator spawnCorutine()
    {
        //Spawnea los objetos hasta el maxDrops o hasta que falle más de x veces
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
