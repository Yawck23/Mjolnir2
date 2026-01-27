using System.Collections;
using UnityEngine;

public class PisoHieloSpawn : MonoBehaviour
{
    private Transform ymir;
    private Transform piso;

    [SerializeField] float offsetPiso = 1.5f;
    private float lifeTimer = 0f;
    [SerializeField] float damagePeriod = 0.3f;

    #region Variables: Shader Transition
    [Header("Shader Transition")]
    [SerializeField] float transitionDuration, startValue, endValue;
    private Material pisoHieloMaterial;
    #endregion


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ymir = GameObject.Find("Ymir").GetComponent<Transform>().parent;
        piso = GameObject.Find("Piso").GetComponent<Transform>();
        pisoHieloMaterial = GetComponentInChildren<Renderer>().material;

        Vector3 pisoHieloPosition = ymir.position;
        pisoHieloPosition.y = piso.position.y + offsetPiso; //Ajustamos el pisoHielo para que esté un poco por encima del piso
        float randomYOffset = Random.Range(0.001f, 0.1f);
        pisoHieloPosition.y += randomYOffset; //Pequeña variación para evitar Y-fighting
        transform.position = pisoHieloPosition;

        float ymirY = ymir.eulerAngles.y;
        Quaternion pisoHieloRotation = Quaternion.Euler(0f, ymirY -90f, 0f); //Ajustamos la rotación para que apunte a donde mira ymir
        transform.rotation = pisoHieloRotation;

        StartCoroutine(ShaderTransition());

        //Destroy(this.gameObject, timeDuration); Solo se rompe cuando el jefe lo golpea
    }

    void Update()
    {
        lifeTimer += Time.deltaTime;
    }

    public float GetLifeTimer()
    {
        return lifeTimer;
    }

    public float GetDamagePeriod(){
        return damagePeriod;
    }

    private IEnumerator ShaderTransition()
    {
        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            float newValue = Mathf.Lerp(startValue, endValue, elapsedTime / transitionDuration);
            pisoHieloMaterial.SetFloat("_TransparenciaCodeadble", newValue);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        pisoHieloMaterial.SetFloat("_TransparenciaCodeadble", endValue);
    }
}
