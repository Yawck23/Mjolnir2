using System.Collections;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [SerializeField] Animator animator;
    private PlayerController player;
    private CharacterController controller;


    private float dashSpeed;
    private float dashTime;
    private bool isDash;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<PlayerController>();
        controller = GetComponent<CharacterController>();

        dashSpeed = 20f;
        dashTime = 0.25f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Dash") && !isDash)
        {
            StartCoroutine(DashCoroutine());
            animator.SetTrigger("Dash");
        }
    }
    IEnumerator DashCoroutine()
    {
        isDash = true;
        float startTime = Time.time;
        Vector3 playerLook = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;

        while (Time.time < startTime + dashTime)
        {
            controller.Move(playerLook * dashSpeed * Time.deltaTime);
            yield return null;

        }
        isDash = false;
    }

    public bool isDashing()
    {
        return isDash;
    }
}
