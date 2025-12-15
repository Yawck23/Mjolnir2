using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField] Text gameTimeText, winGameTimeText, deathsCountText;
    [SerializeField] GameObject MainMenuPanel, InGameMenuPanel, TimePanel, LvlSelectPanel, WinPanel;

    private UICameraMovement cameraMovScript;

    void Start()
    {
        cameraMovScript = GetComponent<UICameraMovement>();
        GoToMainMenue();
    }

    void Update()
    {
        // Si el juego fue iniciado
        if (!GameManager.GM.GetGameStarted()) return;

        gameTimeText.text = "Time: " + GameManager.GM.GetGameTime().ToString("0");
        deathsCountText.text = "Deaths: " + GameManager.GM.GetDeathsCount().ToString("0");

            
        if (GameManager.GM.GetPause() && !InGameMenuPanel.activeSelf)
        {
            InGameMenuPanel.SetActive(true);
        }
        else if (!GameManager.GM.GetPause() && InGameMenuPanel.activeSelf)
        {
                InGameMenuPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Funcion publica que prepara la ui para el juego cuando se inicia
    /// </summary>
    public void StartGame()
    {
        MainMenuPanel.SetActive(false);
        TimePanel.SetActive(true);
        LvlSelectPanel.SetActive(false);

        cameraMovScript.DisableMainMenuCameras();
    }

    /// <summary>
    /// Funcion publica para volver al menu
    /// </summary>
    public void GoToMainMenue()
    {
        cameraMovScript.EnableMainMenuCameras();

        MainMenuPanel.SetActive(true);
        TimePanel.SetActive(false);
        InGameMenuPanel.SetActive(false);
        LvlSelectPanel.SetActive(false);
        WinPanel.SetActive(false);

        cameraMovScript.goToMainMenuCamera();

    }

    public void GoToLvlSelect()
    {
        MainMenuPanel.SetActive(false);
        LvlSelectPanel.SetActive(true);
        cameraMovScript.goToLvlSelectCamera();
    }
    
    public void Win()
    {
        WinPanel.SetActive(true);
        winGameTimeText.text = "Ganaste en " + GameManager.GM.GetGameTime().ToString("0") + " segundos y con " + GameManager.GM.GetDeathsCount().ToString("0") + " muertes";
    }
}
