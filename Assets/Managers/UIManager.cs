using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UIManager : MonoBehaviour
{
    [SerializeField] Text winGameTimeText, deathsCountTextInDeathScreen, deathScreenReviveText;
    [SerializeField] GameObject MainMenuPanel, InGameMenuPanel, WinPanel, DeathScreenPanel, CinematicTriggerPanel; //CoomingSoonPanel

    private UICameraMovement cameraMovScript;
    private UILevelSelect levelSelectScript;
    private PlayerMovementMainMenu playerMovementScript;
    private GameObject padreHumos; //Para hacer el trigger de las animaciones

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

        cameraMovScript = GetComponent<UICameraMovement>();
    }
    #endregion

    void Update()
    {
        // Si el juego fue iniciado
        if (!GameManager.GM.GetGameStarted()) return;

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
        //LvlSelectPanel.SetActive(false);
    }

    /// <summary>
    /// Funcion publica para volver al menu
    /// </summary>
    public void GoToMainMenue()
    {
        MainMenuPanel.SetActive(true);
        InGameMenuPanel.SetActive(false);
        //LvlSelectPanel.SetActive(false);
        WinPanel.SetActive(false);
        //CoomingSoonPanel.SetActive(false);
        DeathScreenPanel.SetActive(false);

        cameraMovScript.goToMainMenuCamera();

    }

    public void GoToLvlSelect()
    {
        //CoomingSoonPanel.SetActive(false);
        MainMenuPanel.SetActive(false);
        //LvlSelectPanel.SetActive(true);
        cameraMovScript.goToLvlSelectCamera();
        HumosAnimationTrigger();
        
    }

    /*public void GoToComingSoon()
    {
        //LvlSelectPanel.SetActive(false);
        CoomingSoonPanel.SetActive(true);
    }*/

    public void GoToDeathScreen()
    {
        DeathScreenPanel.SetActive(true);
        StartCoroutine(DeathCountCoRoutineTest());
    }

    public void ExitDeathScreen()
    {
        DeathScreenPanel.SetActive(false);
    }

    public void Win()
    {
        WinPanel.SetActive(true);
        winGameTimeText.text = "You win!\n" + "Time: " + GameManager.GM.GetGameTime().ToString("0") + "\nDeaths: " + GameManager.GM.GetDeathsCount().ToString("0");
    }

    public int GetSelectedLevel()
    {
        return levelSelectScript.getLevelSelected();
    }

    /*public void unlockLevel2()
    {
        levelSelectScript.unlockLevel2();
    }*/

    private IEnumerator DeathCountCoRoutineTest()
    {
        deathScreenReviveText.enabled = false;
        // Código antes de la espera
        yield return new WaitForSeconds(1.2f); // Espera para cambiar de número

        deathsCountTextInDeathScreen.text = "Deaths: " + GameManager.GM.GetDeathsCount().ToString("0");

        yield return new WaitForSeconds(0.7f);

        deathScreenReviveText.enabled = true; //Espera para mostrar texto revivir
    }

    public void StartMjolnirRunMainMenu()
    {
        playerMovementScript.StartMovement();
    }

    public void ToggleCinematicTriggerPanel(bool active)
    {
        CinematicTriggerPanel.SetActive(active);
    }

    public void registerWorldUILvlSelectScript(UILevelSelect script)
    {
        levelSelectScript = script;
    }

    public void registerPlayerMovementScript(PlayerMovementMainMenu script)
    {
        playerMovementScript = script;
    }

    private void HumosAnimationTrigger()
    {
        padreHumos = GameObject.FindGameObjectWithTag("HumosUI");
        foreach (Transform child in padreHumos.transform)
        {
            Animator animator = child.GetComponent<Animator>();
            if (animator != null) animator.SetTrigger("Movimiento");
        }
    }
}
