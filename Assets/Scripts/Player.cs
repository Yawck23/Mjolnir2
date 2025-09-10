using UnityEngine;
using UnityEngine.VFX;

public class Player : MonoBehaviour

{
    [SerializeField] Transform Visual;
    public CharacterController CharControl;
    public float X;
    public float Y;
    public float Speed;
    Vector3 VectorInput;
    [SerializeField] Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Speed = 10;
        
    }

    // Update is called once per frame
    void Update()
    {
        ConseguirInput();
        RotarPersonaje();
        Animaciones();


        //Vector3 Move = new Vector3((X * Speed) * Time.deltaTime, 0, (Y*Speed)*Time.deltaTime);
        //CharControl.Move(Move);
        CharControl.Move((VectorInput * Speed /*+ Vector3.up*/) * Time.deltaTime);

    }
    void ConseguirInput()
    { 
        X = Input.GetAxisRaw("Horizontal");
        Y = Input.GetAxisRaw("Vertical");
        VectorInput = (transform.right * X + transform.forward * Y).normalized;
        if (Input.GetButtonDown("Jump"))
        
        {
            Salto();
        }
    }
    void Salto() 
    {
        if (CharControl.isGrounded)
        {

        }
    }
    void RotarPersonaje()
    {
        if (VectorInput.magnitude != 0f)
        {
            Visual.rotation = Quaternion.LookRotation(VectorInput, Vector3.up);
        }
    }
    void Animaciones()
    {
        animator.SetFloat("Velocidad", ((VectorInput * Speed).magnitude));
    }
}
