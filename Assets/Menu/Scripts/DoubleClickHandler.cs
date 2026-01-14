using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems; // Requerido para detectar eventos de mouse/touch

public class DoubleClickHandler : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent onDoubleClick; // Aparecerá en el Inspector como un botón normal

    public void OnPointerClick(PointerEventData eventData)
    {
        // Verificamos si el conteo de clics es igual a 2
        if (eventData.clickCount == 2)
        {
            Debug.Log("¡Doble clic detectado!");
            onDoubleClick.Invoke(); // Ejecuta el método que asignes
        }
    }
}
