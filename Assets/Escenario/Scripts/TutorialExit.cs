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
    private ExplosionIntoParts explosionIntoParts;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        enemyAnimator = GameObject.Find("Ymir").GetComponent<Animator>();
        bossFightCam = bossFightCamGameObject.GetComponent<CinemachineCamera>();
        explosionIntoParts = GetComponent<ExplosionIntoParts>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (player.IsDashing())
        {
            explosionIntoParts.Explosion();

            enemyAnimator.SetTrigger("ExitTutorial");
            bossFightCam.Priority = 2;
            AudioManager.AM.Play("BossMusic");
        }
    }
}
