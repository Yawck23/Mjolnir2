using UnityEngine;

public class Saltar : MonoBehaviour
{
    [SerializeField] float GRAVEDAD, MAX_VEL_CAIDA, FUERZA_SALTO;
    float VelocidadVertical;
    private CharacterController controller;
    bool EstadoGroundedPasado;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        CaluclarVelocidadVertical();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Salto();
        }
    }
    void CaluclarVelocidadVertical()
    {
        if (!controller.isGrounded)
        {
            VelocidadVertical -= GRAVEDAD * Time.deltaTime;
            if (VelocidadVertical < -MAX_VEL_CAIDA)
            {
                VelocidadVertical = -MAX_VEL_CAIDA;
            }
        }
        else if (!EstadoGroundedPasado)
        {
            VelocidadVertical = -2f;
        }
        EstadoGroundedPasado = controller.isGrounded;
    }
    void Salto()
    {
        if (controller.isGrounded)
        {
            VelocidadVertical = FUERZA_SALTO;
            EstadoGroundedPasado = true;
        }
    }    
}
