using UnityEngine;
using UnityEngine.VFX;

public class PlayerSimple : MonoBehaviour

{
    [SerializeField] Transform Visual;
    public CharacterController CharControl;
    public float X;
    public float Y;
    public float Speed;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public Vector3 VectorInput;
    [SerializeField] Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Speed = 20;
        
    }

    // Update is called once per frame
    void Update()
    {
        ConseguirInput();
        Animaciones();

        
        if (VectorInput.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(VectorInput.x, VectorInput.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            CharControl.Move(VectorInput * Speed * Time.deltaTime);
        }

    }
    void ConseguirInput()
    { 
        X = Input.GetAxisRaw("Horizontal");
        Y = Input.GetAxisRaw("Vertical");
        VectorInput = new Vector3 (X, 0f ,Y).normalized;
    }
    void Animaciones()
    {
        animator.SetFloat("Velocidad", ((VectorInput * Speed).magnitude));
    }
}
