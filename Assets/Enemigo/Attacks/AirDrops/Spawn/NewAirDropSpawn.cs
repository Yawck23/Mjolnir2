using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class NewAirDropSpawn : MonoBehaviour
{

    #region Variables: Spawn
    [SerializeField] GameObject drop; //Objeto a dropear
    private Bounds planeBounds; //Los limites donde dropean
    [SerializeField] int maxDrops; //Drops máximos
    [SerializeField] float minY, maxY; //Altura minima y máxima para el drop
    [SerializeField] float waitForNextDrop; //Tiempo de espera entre drops
    [SerializeField] private float playerProximityWeight = 0.8f; // Qué tan "cerca" del player caerán (en radianes)
    #endregion

    #region Variables: SpawnOverlap
    private Vector3 halfObjectExtents; //Para calcular la mitad del objeto
    private BoxCollider dropCollider;
    private string dropTag;
    private int spawnedObjects = 0; //Para contar la cantidad de items spawneados
    private int failures = 0; //Si falla muchas veces, deja de spawnerar objetos, por más que no se llegue al maxDrops
    #endregion

    #region Variables: playerFollow
    private Transform playerTransform;
    #endregion
    private Animator enemyAnimator;
    //private bool animationEnded = false;
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        enemyAnimator = GameObject.Find("Ymir").GetComponent<Animator>();

        planeBounds = GetComponent<MeshRenderer>().bounds;

        dropCollider = drop.GetComponent<BoxCollider>();
        halfObjectExtents = Vector3.Scale(dropCollider.size * 0.5f, drop.transform.localScale);
        halfObjectExtents.y = drop.transform.position.y;
        dropTag = drop.tag;
        StartCoroutine(spawnCorutine());
    }

    private Vector3 getSpawnPoint()
    {
        // 1. Radios (igual que antes)
        float maxRadius = planeBounds.extents.x;
        float minRadius = maxRadius * 0.5f;

        // 2. Calcular el ángulo base (hacia donde está el jugador)
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        // Atan2 nos da el ángulo en radianes desde el centro hacia el jugador
        float angleToPlayer = Mathf.Atan2(directionToPlayer.z, directionToPlayer.x);

        // 3. Añadir una variación aleatoria para que no sea 100% exacto
        // Entre más pequeño sea playerProximityWeight, más cerca del player caerán
        float randomOffset = Random.Range(-playerProximityWeight, playerProximityWeight);
        float finalAngle = angleToPlayer + randomOffset;

        // 4. Radio aleatorio con distribución uniforme
        float r = Mathf.Sqrt(Random.Range(minRadius * minRadius, maxRadius * maxRadius));

        // 5. Conversión a coordenadas cartesianas
        float x = r * Mathf.Cos(finalAngle);
        float z = r * Mathf.Sin(finalAngle);
        float y = Random.Range(minY, maxY);

        return transform.position + new Vector3(x, y, z);
    }

    private bool validSpawnPoint(Vector3 spawnObjective)
    {
        //Valida si no hay otro objeto debajo o por encima con el mismo tag
        bool valid = true;

        //Raycast en cada esquina en dirección hacia abajo y hacia arriba
        Ray[] rays = {
            new Ray(spawnObjective - Vector3.right * halfObjectExtents.x - Vector3.forward * halfObjectExtents.z, Vector3.down),
            new Ray(spawnObjective + Vector3.right * halfObjectExtents.x - Vector3.forward * halfObjectExtents.z, Vector3.down),
            new Ray(spawnObjective - Vector3.right * halfObjectExtents.x + Vector3.forward * halfObjectExtents.z, Vector3.down),
            new Ray(spawnObjective + Vector3.right * halfObjectExtents.x + Vector3.forward * halfObjectExtents.z, Vector3.down),
            new Ray(spawnObjective - Vector3.right * halfObjectExtents.x - Vector3.forward * halfObjectExtents.z, Vector3.up),
            new Ray(spawnObjective + Vector3.right * halfObjectExtents.x - Vector3.forward * halfObjectExtents.z, Vector3.up),
            new Ray(spawnObjective - Vector3.right * halfObjectExtents.x + Vector3.forward * halfObjectExtents.z, Vector3.up),
            new Ray(spawnObjective + Vector3.right * halfObjectExtents.x + Vector3.forward * halfObjectExtents.z, Vector3.up)
        };

        foreach (Ray ray in rays)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, LayerMask.GetMask("Escombros"), QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.CompareTag(dropTag))
                {
                    valid = false;
                }
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

        enemyAnimator.SetTrigger("LluviaHieloEnd");

        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
}
