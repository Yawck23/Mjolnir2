using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    float gameTime;
    [SerializeField] Text gameTimeText;
    [SerializeField] GameObject MainMenuPanel, InGameMenuPanel, TimePanel, LvlSelectPanel;

    private UICameraMovement cameraMovScript;

    void Start()
    {
        cameraMovScript = GetComponent<UICameraMovement>();
        GoToMainMenue();
    }

    void Update()
    {
        // Si el juego fue iniciado
        if (GameManager.GM.GetGameStarted())
        {
            // Counsulto el estado de Pausa en el objeto GM
            // Si el juego está en transcurso cuento el tiempo y lo muestro en pantalla.
            if (!GameManager.GM.GetPause())
            {
                gameTime = gameTime + Time.deltaTime;
                gameTimeText.text = "Time: " + gameTime.ToString("0");
            }

            // Counsulto el estado de Pausa.
            // Si el juego está en pausa activo el menú.
            // Me aseguro de hacer esta acción una vez consultando el estado del menú
            if (GameManager.GM.GetPause() && !InGameMenuPanel.activeSelf)
            {
                InGameMenuPanel.SetActive(true);
            }
            // De lo controario lo apago.
            // Me aseguro de hacer esta acción una vez consultando el estado del menú
            else if (!GameManager.GM.GetPause() && InGameMenuPanel.activeSelf)
            {
                InGameMenuPanel.SetActive(false);
            }
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
        GameManager.GM.StartGame();
    }

    /// <summary>
    /// Funcion publica para volver al menu
    /// </summary>
    public void GoToMainMenue()
    {
        MainMenuPanel.SetActive(true);
        TimePanel.SetActive(false);
        InGameMenuPanel.SetActive(false);
        LvlSelectPanel.SetActive(false);

        cameraMovScript.goToMainMenuCamera();

        Restart();

        GameManager.GM.GoToMainMenu();
    }

    public void Restart()
    {
        GameManager.GM.Restart();
        GameManager.GM.Resume();
        gameTime = 0;
    }

    public void Resume()
    {
        GameManager.GM.Resume();
    }

    public void ExitGame()
    {
        GameManager.GM.Exit();
    }
    
    public void GoToLvlSelect()
    {
        MainMenuPanel.SetActive(false);
        LvlSelectPanel.SetActive(true);
        cameraMovScript.goToLvlSelectCamera();
    }
}
