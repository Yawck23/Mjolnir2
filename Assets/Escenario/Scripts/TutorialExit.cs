using System;
using UnityEngine;

public class TutorialExit : MonoBehaviour
{
    private PlayerController player;
    private Animator enemyAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        enemyAnimator = GameObject.Find("Ymir").GetComponent<Animator>();
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
        }
    }

}
