using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UIManager : MonoBehaviour
{
    [SerializeField] Text gameTimeText, winGameTimeText, deathsCountText, deathsCountTextInDeathScreen;
    [SerializeField] GameObject MainMenuPanel, InGameMenuPanel, TimePanel, LvlSelectPanel, WinPanel, CoomingSoonPanel, DeathScreenPanel;

    private UICameraMovement cameraMovScript;
    private UILevelSelect levelSelectScript;

    void Start()
    {
        cameraMovScript = GetComponent<UICameraMovement>();
        levelSelectScript = LvlSelectPanel.GetComponent<UILevelSelect>();
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
        //LvlSelectPanel.SetActive(false);
    }

    /// <summary>
    /// Funcion publica para volver al menu
    /// </summary>
    public void GoToMainMenue()
    {
        MainMenuPanel.SetActive(true);
        TimePanel.SetActive(false);
        InGameMenuPanel.SetActive(false);
        //LvlSelectPanel.SetActive(false);
        WinPanel.SetActive(false);
        CoomingSoonPanel.SetActive(false);
        DeathScreenPanel.SetActive(false);

        cameraMovScript.goToMainMenuCamera();

    }

    public void GoToLvlSelect()
    {
        CoomingSoonPanel.SetActive(false);
        MainMenuPanel.SetActive(false);
        LvlSelectPanel.SetActive(true);
        cameraMovScript.goToLvlSelectCamera();
    }

    public void GoToComingSoon()
    {
        //LvlSelectPanel.SetActive(false);
        CoomingSoonPanel.SetActive(true);
    }

    public void GoToDeathScreen()
    {
        DeathScreenPanel.SetActive(true);
        StartCoroutine(DeathCountCoRoutineTest());
    }

    public void ExitDeathScreen(){
        DeathScreenPanel.SetActive(false);
    }

    public void Win()
    {
        WinPanel.SetActive(true);
        winGameTimeText.text = "Ganaste\n" + "Tiempo: " + GameManager.GM.GetGameTime().ToString("0") + "\nMuertes: " + GameManager.GM.GetDeathsCount().ToString("0");
    }

    public int GetSelectedLevel()
    {
        return levelSelectScript.getLevelSelected();
    }
    
    public void unlockLevel2()
    {
        levelSelectScript.unlockLevel2();
    }

    private IEnumerator DeathCountCoRoutineTest()
    {
        // Código antes de la espera
        yield return new WaitForSeconds(1f); // Espera para cambiar de número
        
        deathsCountTextInDeathScreen.text = "Muertes: " + GameManager.GM.GetDeathsCount().ToString("0");
    }
}
