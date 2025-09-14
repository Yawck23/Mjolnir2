using UnityEngine;

public class Gravity : MonoBehaviour
{

    #region Variables: gravity
    private Vector3 _velocity;
    [SerializeField] float gravityMultiplier = 2.0f;
    private float _gravity = -9.81f;
    [SerializeField] float groundSeparation = 1f;
    #endregion

    void Update()
    {
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        //Raycast desde la parte centro/abajo del objeto, un poco más arriba
        Vector3 origin = transform.position + Vector3.down * ((transform.localScale.y - 1f)/ 2);
        Ray ray = new Ray(origin, Vector3.down);

        //Si el raycast detecta un hit y ese hit está a poca distancia, frena el objeto
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.distance < groundSeparation)
            {
                _velocity.y = 0.0f;
                //Destroy(this.gameObject, 10f);
                return;
            }
        }
        
        //Aplicar gravedad
        _velocity.y += _gravity * gravityMultiplier * Time.deltaTime;
        transform.position += _velocity * Time.deltaTime;
    }
}
