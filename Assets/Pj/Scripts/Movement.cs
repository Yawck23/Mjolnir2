using System;
using UnityEngine;
using UnityEngine.VFX;

public class Movement : MonoBehaviour

{
    [SerializeField] Animator animator;
    private PlayerController player;
    private CharacterController controller;

    //Variables para el movimiento
    private float speed = 20f;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;


    void Start()
    {
        player = GetComponent<PlayerController>();
        controller = GetComponent<CharacterController>();

    }

    void Update()
    {
        Animaciones();
        Movimiento();
    }


    private void Movimiento()
    {
        if (player.getInput().magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(player.getInput().x, player.getInput().z) * Mathf.Rad2Deg; //Obtener el ángulo para moverse
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); //Aplicar un smooth para que no sea tan seco
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            controller.Move(player.getInput() * speed * Time.deltaTime);
        }
    }
    private void Animaciones()
    {
        animator.SetFloat("Velocidad", ((player.getInput() * speed).magnitude));
    }
}
