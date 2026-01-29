using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class ShowTextPlayerNear : MonoBehaviour
{
    private TextMeshProUGUI text;
    [SerializeField] private float appearTime, dissapearTime;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.DOFade(0f,0.1f); //Desaparecerlo al inicio
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.DOFade(1f, appearTime);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.DOFade(0f, dissapearTime);
        }
    }
}
