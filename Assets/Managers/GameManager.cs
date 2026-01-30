using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager GM { get; private set; }

    private void Awake()
    {
        // Se revisa si ya existe un objeto llamado GM
        if (GM != null && GM != this)
        {
            // Si ya existe, este objeto se destruye a s� mismo ya que no puede haber dos instancias de un elemento est�tico
            Destroy(gameObject);
        }
        else
        {
            // Si este es el �nico elemento GameManager se asigna a la variable GM
            GM = this;
            // Se pone en un modo que evita ser destruido al cambiar de escena
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    private bool Pause;
    private bool GameStarted;
    private float gameTime;
    private float deathsCount = 0;
    private int lvlSelected;
    [SerializeField] private LoadingScreen loadingScreen;

    void Start()
    {
        Pause = true;
        Time.timeScale = 1f; // 1 representa el tiempo normal del juego
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            TogglePause(!Pause);
        }

        if (!GetPause())
        {
            gameTime += Time.deltaTime;
        }
    }

    #region Funciones publicas accesibles por todos los elementos del programa


    #region Getters & Setters
    public bool GetPause()
    {
        return Pause;
    }

    public bool GetGameStarted()
    {
        return GameStarted;
    }

    public float GetGameTime()
    {
        return gameTime;
    }

    public float GetDeathsCount()
    {
        return deathsCount;
    }

    public void AddToDeathCount()
    {
        deathsCount++;
    }


    #endregion

    public void TogglePause(bool _pause)
    {
        Pause = _pause;

        if (Pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void StartGame()
    {
        switch (UIManager.UIM.GetSelectedLevel())
        {
            case 1:
                loadingScreen.LoadScene("Level_1");
                GameStarted = true;
                Pause = false;
                UIManager.UIM.StartGame();
                gameTime = 0;
                deathsCount = 0;
                lvlSelected = 1;
                break;

            case 2:
                //GoToComingSoon();
                lvlSelected = 2;
                break;

            case 3:
                lvlSelected = 3;
                break;
        }
    }

    public void GoToMainMenu()
    {
        GameStarted = false;
        Pause = true;
        Time.timeScale = 1;

        // ( La escena se debe ser previamente agregada la lista de escenas en build settings)
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MainMenu"))
        {
            loadingScreen.LoadScene("MainMenu");
        }

        UIManager.UIM.GoToMainMenue();
    }

    public void Resume()
    {
        TogglePause(false);
    }

    public void Restart()
    {
        loadingScreen.LoadScene(SceneManager.GetActiveScene().name);
        gameTime = 0;
        deathsCount = 0;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void GoToLvlSelect()
    {
        UIManager.UIM.GoToLvlSelect();
    }

    public void Win()
    {
        UIManager.UIM.Win();
        if (lvlSelected == 1) PersistenceManager.Instance.EvaluateRun(Mathf.RoundToInt(gameTime), Mathf.RoundToInt(deathsCount));

        UIManager.UIM.unlockLevel2();
    }

    public void GoToDeathScreen()
    {
        UIManager.UIM.GoToDeathScreen();
    }

    public void ExitDeathScreen(){
        UIManager.UIM.ExitDeathScreen();
    }
    /*public void GoToComingSoon()
    {
        UIManager.UIM.GoToComingSoon();
    }*/
    #endregion
}
