using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] Animator animator;
    private PlayerController player;
    private CharacterController controller;


    private Vector3 velocidad;

    [SerializeField] float jumpForce, gravity;

    void Start()
    {
        player = GetComponent<PlayerController>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        JumpAction();
    }

    private void JumpAction()
    {
        if (controller.isGrounded && velocidad.y < 0)
        {
            velocidad.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocidad.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        velocidad.y += gravity * Time.deltaTime;
        controller.Move(velocidad * Time.deltaTime);
    }
}
