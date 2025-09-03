using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlesEjemplo : MonoBehaviour
{
    [SerializeField] Transform CamaraTransform, Visual;
    [SerializeField] float VEL_MOVIMIENTO, GRAVEDAD, MAX_VEL_CAIDA, FUERZA_SALTO;

    Vector3 VectorInput;
    CharacterController charController;
    float VelocidadVertical;
    bool EstadoGroundedPasado;
    // Start is called before the first frame update
    void Start()
    {
        charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        ConseguirInput();
        CaluclarVelocidadVertical();
        MoverPersonaje();
        RotarPersonaje();
    }

    void ConseguirInput()
    {
        Vector3 camaraForwardPlanar = new Vector3(CamaraTransform.forward.x, 0f, CamaraTransform.forward.z);
        Vector3 camaraRightPlanar = new Vector3(CamaraTransform.right.x, 0f, CamaraTransform.right.z);
        VectorInput = (camaraRightPlanar * Input.GetAxis("Horizontal") + camaraForwardPlanar * Input.GetAxis("Vertical")).normalized;
        if (Input.GetButtonDown("Jump"))
        {
            Salto();
        }
    }

    void CaluclarVelocidadVertical()
    {
        if (!charController.isGrounded)
        {
            VelocidadVertical -= GRAVEDAD * Time.deltaTime;
            if (VelocidadVertical < -MAX_VEL_CAIDA)
            {
                VelocidadVertical = -MAX_VEL_CAIDA;
            }
        }
        else if(!EstadoGroundedPasado)
        {
            VelocidadVertical = -2f;
        }
        EstadoGroundedPasado = charController.isGrounded;
    }

    void MoverPersonaje()
    {
        charController.Move((VectorInput * VEL_MOVIMIENTO + Vector3.up * VelocidadVertical) * Time.deltaTime);
    }

    void Salto()
    {
        if (charController.isGrounded)
        {
            VelocidadVertical = FUERZA_SALTO;
            EstadoGroundedPasado = true;
        }
    }

    void RotarPersonaje()
    {
        if (VectorInput.magnitude != 0f)
        {
            Visual.rotation = Quaternion.LookRotation(VectorInput, Vector3.up);
        }
    }
}
