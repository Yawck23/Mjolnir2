using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private GameObject[] trees;
    [SerializeField] private float treesTransitionTime, treesStartValue, treesEndValue;
    private List<Material> treesMaterial;

    #endregion

    void Start()
    {
        iceDomeMaterial = iceDome.GetComponent<Renderer>().material;
        iceDomeMaterial.SetFloat("_Dissolve", iceDomeStarValue);

        floorMaterial = floor.GetComponent<Renderer>().material;
        floorMaterial.SetFloat("_CambioTextura", floorStartValue);

        cloudSkyMaterial = cloudSky.GetComponent<Renderer>().material;
        cloudSkyMaterial.SetFloat("_CloudAlpha", cloudSkyStartValue);

        treesMaterial = new List<Material>();
        foreach (GameObject tree in trees)
        {
            Material treeMat = tree.GetComponent<Renderer>().sharedMaterial;
            treeMat.SetFloat("_Level", treesStartValue);
            treesMaterial.Add(treeMat);
        }
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
        float elapsedTime = 0f;
        while (elapsedTime < treesTransitionTime)
        {
            float newValue = Mathf.Lerp(startValue, endValue, elapsedTime / treesTransitionTime);
            foreach (Material mat in treesMaterial)
            {
                mat.SetFloat("_Level", newValue);   
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        foreach (Material mat in treesMaterial)
        {
            mat.SetFloat("_Level", endValue);
        }
    }



}
