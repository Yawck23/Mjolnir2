using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UIManager : MonoBehaviour
{
    [SerializeField] Text gameTimeText, winGameTimeText, deathsCountText, deathsCountTextInDeathScreen;
    [SerializeField] GameObject MainMenuPanel, InGameMenuPanel, TimePanel, LvlSelectPanel, WinPanel, CoomingSoonPanel, DeathScreenPanel;

    private UICameraMovement cameraMovScript;
    private UILevelSelect levelSelectScript;

    #region Singleton
    public static UIManager UIM { get; private set; }

    private void Awake()
    {
        // Se revisa si ya existe un objeto llamado UIM
        if (UIM != null && UIM != this)
        {
            // Si ya existe, este objeto se destruye a s� mismo ya que no puede haber dos instancias de un elemento est�tico
            Destroy(gameObject);
        }
        else
        {
            // Si este es el �nico elemento UIManager se asigna a la variable UIM
            UIM = this;
            // Se pone en un modo que evita ser destruido al cambiar de escena
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

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

    public void StartMjolnirRunMainMenu()
    {
        GameObject player = GameObject.Find("MjolnirMainMenu");
        if (player != null)
        {
            PlayerMovementMainMenu playerMovement = player.GetComponent<PlayerMovementMainMenu>();
            if (playerMovement != null)
            {
                playerMovement.StartMovement();
            }
        }
    }
}
