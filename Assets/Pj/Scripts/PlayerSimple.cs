using System;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerSimple : MonoBehaviour

{
    [SerializeField] Transform visual;
    public CharacterController controller;
    public float speed;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    [SerializeField] Animator animator;

    //Knockback
    public float fuerzaRetroceso = 20f;
    public float duracionRetroceso = 0.2f;
    private Vector3 knockbackVector = Vector3.zero;
    private float knockbackTimer = 0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        speed = 20f;

    }

    // Update is called once per frame
    void Update()
    {
        ConseguirInput();
        Animaciones();

        if (knockbackTimer > 0)
        {
            controller.Move(knockbackVector * Time.deltaTime);
            knockbackTimer -= Time.deltaTime;
        }

        if (ConseguirInput().magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(ConseguirInput().x, ConseguirInput().z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            controller.Move(ConseguirInput() * speed * Time.deltaTime);
        }
    }
    public Vector3 ConseguirInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector3 vectorInput = new Vector3(x, 0f, y).normalized;

        return vectorInput;
    }
    void Animaciones()
    {
        animator.SetFloat("Velocidad", ((ConseguirInput() * speed).magnitude));
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Enemigo"))
        {
            Vector3 dir = transform.position - hit.point;
            dir.y = 0f;
            dir.Normalize();

            knockbackVector = dir * fuerzaRetroceso;
            knockbackTimer = duracionRetroceso;
        }
    }
}
