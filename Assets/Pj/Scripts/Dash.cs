using System.Collections;
using UnityEngine;

public class Dash : MonoBehaviour
{
    PlayerSimple player;
    public float dashSpeed;
    public float dashTime;
    [SerializeField] Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<PlayerSimple>();
        dashSpeed = 20f;
        dashTime = 0.25f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Dash"))
        {
            StartCoroutine(DashCoroutine());
            animator.SetTrigger("Dash");
        }
    }
    IEnumerator DashCoroutine()
    {
        float startTime = Time.time;
        while (Time.time < startTime+ dashTime)
        {
            player.controller.Move(player.ConseguirInput() *  dashSpeed * Time.deltaTime);
            //animator.SetTrigger("Dash");
            yield return null;
            
        }
    }
}
