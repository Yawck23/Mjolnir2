using UnityEngine;

public class RBGravityMultiplier : MonoBehaviour
{
    [SerializeField] private float escalaGravedad = 8f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Desactivamos la gravedad global para este objeto
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        // Aplicamos una fuerza constante hacia abajo
        // La fórmula es: Masa * Gravedad Global * Nuestra Escala
        Vector3 gravedadCustom = Physics.gravity * escalaGravedad;
        rb.AddForce(gravedadCustom, ForceMode.Acceleration);
    }
}
