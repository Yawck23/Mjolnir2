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
            DontDestroyOnLoad(this);
        }
    }
    #endregion

    bool Pause;
    bool GameStarted;
    //UIManager uiManager;

    void Start()
    {
        // Inicializo las variables de tiempo y timescale
        Pause = true;
        // Timescale refiere a la velocidad de todo lo que est� controlado por tiempo
        // esto incluye la fisica y todas las acciones que se calculen con Time.DeltaTime
        Time.timeScale = 1f; // 1 representa el tiempo normal del juego
        // Consigo el el UIManager
        //uiManager = transform.Find("CanvasMenu").GetComponent<UIManager>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            TogglePause();
        }
    }

    #region Funciones publicas accesibles por todos los elementos del programa

    public bool GetPause()
    {
        return Pause;
    }

    public bool GetGameStarted()
    {
        return GameStarted;
    }

    public void TogglePause()
    {
        // Cambio puase al valor opuesto
        Pause = !Pause;
        // Cuando cambio pausa actualizo el estado de TimeScale
        // Si el juego est� en pausa TimeScale es cero
        if (Pause)
        {
            Time.timeScale = 0;
        }
        // Si el juego no est� en pausa el TimeScale es uno
        else
        {
            Time.timeScale = 1;
        }
    }

    /// <summary>
    /// Funci�n p�blica que inicia el juego y carga el primer nivel
    /// </summary>
    public void StartGame()
    {
        GameStarted = true;
        Pause = false;
        // Llamo una funcion del uiManager
        //uiManager.StartGame();
        // Se carga la escena del nivel 1. Esta funcion se llama referenciando al nombre de la escena
        // ( La escena se debe ser previamente agregada la lista de escenas en build settings)
        SceneManager.LoadScene("Level_1");
    }

    /// <summary>
    /// Funci�n publica llamada por un boton de men� para volver a la escena inicial
    /// </summary>
    public void GoToMainMenu()
    {
        GameStarted = false;
        Pause = true;
        //uiManager.GoToMainMenue();
        Time.timeScale = 1;
        // Llamo una funcion del uiManager
        // Se carga la escena del nivel 1. Esta funcion se llama referenciando al nombre de la escena
        // ( La escena se debe ser previamente agregada la lista de escenas en build settings)
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Funci�n publica llamada por boton de men� para reanudar el juego
    /// </summary>
    public void Resume()
    {
        TogglePause();
    }

    /// <summary>
    /// Funci�n publica llamada por boton de men� para reiniciar la escena
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Funci�n publica llamada por el boton de men� para cerrar la aplicaci�n (Funciona s�lo si el juego est� compilado).
    /// </summary>
    public void Exit()
    {
        Application.Quit();
    }

    #endregion
}
