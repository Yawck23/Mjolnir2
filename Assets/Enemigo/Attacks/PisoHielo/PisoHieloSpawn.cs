using UnityEngine;

public class PisoHieloSpawn : MonoBehaviour
{
    private Transform ymir;
    private Transform piso;

    [SerializeField] float offsetPiso = 1.5f;
    [SerializeField] float timeDuration = 20f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ymir = GameObject.Find("Ymir").GetComponent<Transform>().parent;
        piso = GameObject.Find("Piso").GetComponent<Transform>();

        Vector3 pisoHieloPosition = ymir.position;
        pisoHieloPosition.y = piso.position.y + offsetPiso; //Ajustamos el pisoHielo para que esté un poco por encima del piso
        transform.position = pisoHieloPosition;

        float ymirY = ymir.eulerAngles.y;
        Quaternion pisoHieloRotation = Quaternion.Euler(0f, ymirY -90f, 0f); //Ajustamos la rotación para que apunte a donde mira ymir
        transform.rotation = pisoHieloRotation;

        Destroy(this.gameObject, timeDuration);
    }
}
