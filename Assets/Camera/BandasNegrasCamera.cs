using UnityEngine;
using UnityEngine.UI;

public class BandasNegrasCamera : MonoBehaviour
{
    [SerializeField] private RectTransform topBar, bottomBar;
    public float barSize = 150f; // Altura de las bandas
    public float speed = 5f;    // Velocidad de la transición

    private bool isActive = false;
    private float targetSize = 0f;

    void Update()
    {
        // Detectar si presionamos una tecla para probar (ejemplo: Espacio)
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleBars();
        }

        // Interpolación suave del tamaño de las barras
        float currentHeight = Mathf.Lerp(topBar.sizeDelta.y, targetSize, Time.deltaTime * speed);
        
        topBar.sizeDelta = new Vector2(topBar.sizeDelta.x, currentHeight);
        bottomBar.sizeDelta = new Vector2(bottomBar.sizeDelta.x, currentHeight);
    }

    public void ToggleBars()
    {
        isActive = !isActive;
        targetSize = isActive ? barSize : 0f;
    }
}