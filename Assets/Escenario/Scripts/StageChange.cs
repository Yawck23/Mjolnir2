using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class StageChange : MonoBehaviour
{
    
    #region Variables and Objects
    [Header("Ice Dome")]
    [SerializeField] private GameObject iceDome;
    [SerializeField] private float iceDomeTransitionTime, iceDomeStarValue, iceDomeEndValue;
    private Material iceDomeMaterial;

    [Header("Floor")]
    [SerializeField] private GameObject floor;
    [SerializeField] private float floorTransitionTime, floorStartValue, floorEndValue;
    private Material floorMaterial;

    [Header("Cloud Sky")]
    [SerializeField] private GameObject cloudSky;
    [SerializeField] private float cloudSkyTransitionTime, cloudSkyStartValue, cloudSkyEndValue;
    private Material cloudSkyMaterial;

    [Header("Trees")]
    [SerializeField] private GameObject trees;
    [SerializeField] private float treesTransitionTime, treesStartValue, treesEndValue;
    private Material treesMaterial;

    #endregion

    void Start()
    {
        iceDomeMaterial = iceDome.GetComponent<Renderer>().material;
        floorMaterial = floor.GetComponent<Renderer>().material;
        cloudSkyMaterial = cloudSky.GetComponent<Renderer>().material;
        treesMaterial = trees.GetComponent<Renderer>().material;
    }

    public void TransitionToIce()
    {
        StartCoroutine(IceDomeTransition(iceDomeStarValue, iceDomeEndValue));
        StartCoroutine(FloorTransition(floorStartValue, floorEndValue));
        StartCoroutine(CloudSkyTransition(cloudSkyStartValue, cloudSkyEndValue));
        StartCoroutine(TreesTransition(treesStartValue, treesEndValue));
    }

    public void TransitionToNormal()
    {
        StartCoroutine(IceDomeTransition(iceDomeEndValue, iceDomeStarValue));
        StartCoroutine(FloorTransition(floorEndValue, floorStartValue));
        StartCoroutine(CloudSkyTransition(cloudSkyEndValue, cloudSkyStartValue));
        StartCoroutine(TreesTransition(treesEndValue, treesStartValue));
    }

    private IEnumerator IceDomeTransition(float startValue, float endValue)
    {
        iceDome.SetActive(true);
        float elapsedTime = 0f;
        while (elapsedTime < iceDomeTransitionTime)
        {
            float newValue = Mathf.Lerp(startValue, endValue, elapsedTime / iceDomeTransitionTime);
            iceDomeMaterial.SetFloat("_Dissolve", newValue);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        iceDomeMaterial.SetFloat("_Dissolve", endValue);
    }

    private IEnumerator FloorTransition(float startValue, float endValue)
    {
        float elapsedTime = 0f;
        while (elapsedTime < floorTransitionTime)
        {
            float newValue = Mathf.Lerp(startValue, endValue, elapsedTime / floorTransitionTime);
            floorMaterial.SetFloat("_CambioTextura", newValue);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        floorMaterial.SetFloat("_CambioTextura", endValue);
    }

    private IEnumerator CloudSkyTransition(float startValue, float endValue)
    {
        float elapsedTime = 0f;
        while (elapsedTime < cloudSkyTransitionTime)
        {
            float newValue = Mathf.Lerp(startValue, endValue, elapsedTime / cloudSkyTransitionTime);
            cloudSkyMaterial.SetFloat("_CloudAlpha", newValue);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cloudSkyMaterial.SetFloat("_CloudAlpha", endValue);
    }

    private IEnumerator TreesTransition(float startValue, float endValue)
    {
        /*float elapsedTime = 0f;
        while (elapsedTime < treesTransitionTime)
        {
            float newValue = Mathf.Lerp(startValue, endValue, elapsedTime / treesTransitionTime);
            treesMaterial.SetFloat("_Level", newValue);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        treesMaterial.SetFloat("_Level", endValue);*/

        //Obtener cada hijo del objeto "Arboles" y aplicar la transición individualmente
        yield return null;
    }



}
