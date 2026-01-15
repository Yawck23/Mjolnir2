using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class TutorialExit : MonoBehaviour
{
    private PlayerController player;
    private Animator enemyAnimator;

    [SerializeField] private GameObject bossFightCamGameObject;
    private CinemachineCamera bossFightCam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        enemyAnimator = GameObject.Find("Ymir").GetComponent<Animator>();
        bossFightCam = bossFightCamGameObject.GetComponent<CinemachineCamera>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (player.IsDashing())
        {
            Explosion();
            Debug.Log("Tutorial Exit Triggered");
            enemyAnimator.SetTrigger("ExitTutorial");
            bossFightCam.Priority = 2;
            AudioManager.AM.Play("BossMusic");
        }
    }

    private void Explosion()
    {
        BoxCollider bc = GetComponent<BoxCollider>();
        bc.enabled = false;

        Vector3 explosionPoint = Vector3.zero;

        foreach (Transform child in transform) //Buscamos el punto de explosion
        {
            if (child.name.Equals("ExplosionPoint"))
            {
                explosionPoint = child.position;
            }
        }

        foreach (Transform child in transform) //Aplicamos la explosion a cada rigidbody hijo
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb == null) continue;

            rb.isKinematic = false;
            rb.AddExplosionForce(10000f, explosionPoint, 450f);
        }
    }

}
