using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private CharacterController controller;
    private Animator animator;
    private Movement movement;
    private Knockback chocar;
    private Dash dash;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        movement = GetComponent<Movement>();
        chocar = GetComponent<Knockback>();
        dash = GetComponent<Dash>();
    }

    void Update()
    {

    }

    public Vector3 getInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector3 vectorInput = new Vector3(x, 0f, y).normalized;

        return vectorInput;
    }

    public bool isDashing()
    {
        return dash.isDashing();
    }

    public bool isKnockbacking()
    {
        return chocar.isKnockbacking();
    }

}
