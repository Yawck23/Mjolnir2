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
    private UIManager uiManager;
    private float gameTime;
    private float deathsCount = 0;

    void Start()
    {
        Pause = true;
        Time.timeScale = 1f; // 1 representa el tiempo normal del juego

        uiManager = transform.Find("CanvasMenu").GetComponent<UIManager>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            TogglePause();
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

    public void TogglePause()
    {
        Pause = !Pause;

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
        switch (uiManager.GetSelectedLevel())
        {
            case 1:
                SceneManager.LoadScene("Level_1");
                GameStarted = true;
                Pause = false;
                uiManager.StartGame();
                gameTime = 0;
                deathsCount = 0;
                break;

            case 2:
                GoToComingSoon();
                break;

            case 3:
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
            SceneManager.LoadScene("MainMenu");
        }

        uiManager.GoToMainMenue();
    }

    public void Resume()
    {
        TogglePause();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameTime = 0;
        deathsCount = 0;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void GoToLvlSelect()
    {
        uiManager.GoToLvlSelect();
    }

    public void Win()
    {
        uiManager.Win();
        uiManager.unlockLevel2();
    }

    public void GoToComingSoon()
    {
        uiManager.GoToComingSoon();
    }
    #endregion
}
