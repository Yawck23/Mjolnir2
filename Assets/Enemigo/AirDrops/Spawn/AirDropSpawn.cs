using UnityEngine;

public class AirDropSpawn : MonoBehaviour
{

    [SerializeField] GameObject drop;
    private Bounds planeBounds;
    [SerializeField] int maxDrops;
    [SerializeField] float minY, maxY;

    void Start()
    {
        planeBounds = GetComponent<MeshRenderer>().bounds;

        for (int i = 0; i < maxDrops; i++)
        {
            SpawnRandom();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void SpawnRandom()
    {
        float x = Random.Range(planeBounds.min.x, planeBounds.max.x);
        float z = Random.Range(planeBounds.min.z, planeBounds.max.z);
        float y = Random.Range(minY, maxY);

        Vector3 spawnPoint = new Vector3(x, y, z);

        Instantiate(drop, spawnPoint, Quaternion.identity);
    }
}
