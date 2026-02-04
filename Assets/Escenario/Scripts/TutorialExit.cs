using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class TutorialExit : MonoBehaviour
{
    private Animator enemyAnimator;

    [SerializeField] private GameObject bossFightCamGameObject;
    private CinemachineCamera bossFightCam;
    private bool tutorialFinished = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyAnimator = GameObject.Find("Ymir").GetComponent<Animator>();
        bossFightCam = bossFightCamGameObject.GetComponent<CinemachineCamera>();
    }

    private void TutorialExitTrigger()
    {
        tutorialFinished = true;
        enemyAnimator.SetTrigger("ExitTutorial");
        bossFightCam.Priority = 2;
        MusicManager.Instance.PlayMusic(MusicManager.Instance.Ymir1);
    }

    void OnTriggerEnter(Collider other)
    {
        if (tutorialFinished) return;
        
        if (other.CompareTag("Player") || other.CompareTag("PlayerHitBox")) TutorialExitTrigger();
    }

    public bool IsTutorialFinished()
    {
        return tutorialFinished;
    }
}
