using UnityEngine;

public class Principal : MonoBehaviour
{

    public CharacterController controller;
    public Animator animator;
    public BoxCollider collider;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        getInput();
    }
    
    public Vector3 getInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector3 vectorInput = new Vector3(x, 0f, y).normalized;

        return vectorInput;
    }
}
