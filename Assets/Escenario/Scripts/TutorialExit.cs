using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class TutorialExit : MonoBehaviour
{
    private PlayerController player;
    private Animator enemyAnimator;

    [SerializeField] private GameObject bossFightGameObject;
    private CinemachineCamera bossFightCam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        enemyAnimator = GameObject.Find("Ymir").GetComponent<Animator>();
        bossFightCam = bossFightGameObject.GetComponent<CinemachineCamera>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (player.IsDashing())
        {
            enemyAnimator.SetTrigger("ExitTutorial");
            Destroy(this.gameObject);
            bossFightCam.Priority = 2;
            AudioManager.AM.Play("BossMusic");
        }
    }

}
