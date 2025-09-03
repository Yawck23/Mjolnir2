using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Como crear un elemento único
    // Se crea una instancia única (elemento estático) de la clase GameManager con el nombre GM
    public static GameManager GM { get; private set; }

    // Awake se ejecuta en el primer instante en el que el objeto es creado (sucede antes de start)
    private void Awake()
    {
        // Se revisa si ya existe un objeto llamado GM
        if (GM != null && GM != this)
        {
            // Si ya existe, este objeto se destruye a sí mismo ya que no puede haber dos instancias de un elemento estático
            Destroy(gameObject);
        }
        else
        {
            // Si este es el único elemento GameManager se asigna a la variable GM
            GM = this;
            // Se pone en un modo que evita ser destruido al cambiar de escena
            DontDestroyOnLoad(this);
        }
    }
    #endregion

    bool Pause; // Variable de pausa
    bool GameStarted; // Representa si el juego fue iniciado

    UIManager uiManager;

    void Start()
    {
        // Inicializo las variables de tiempo y timescale
        Pause = true;
        // Timescale refiere a la velocidad de todo lo que esté controlado por tiempo
        // esto incluye la fisica y todas las acciones que se calculen con Time.DeltaTime
        Time.timeScale = 1f; // 1 representa el tiempo normal del juego
        // Consigo el el UIManager
        uiManager = transform.Find("CanvasMenu").GetComponent<UIManager>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            TogglePause();
        }
    }

    #region Funciones publicas accesibles por todos los elementos del programa

    /// <summary>
    /// Función pública para consultar el estado de Pausa
    /// </summary>
    /// <returns></returns>
    public bool GetPause()
    {
        return Pause;
    }

    /// <summary>
    /// Función pública para consultar el juego fue iniciado
    /// </summary>
    /// <returns></returns>
    public bool GetGameStarted()
    {
        return GameStarted;
    }

    /// <summary>
    /// Función pública para cambiar el estado de pausa al opuesto de su estado actual
    /// </summary>
    public void TogglePause()
    {
        // Cambio puase al valor opuesto
        Pause = !Pause;
        // Cuando cambio pausa actualizo el estado de TimeScale
        // Si el juego está en pausa TimeScale es cero
        if (Pause)
        {
            Time.timeScale = 0;
        }
        // Si el juego no está en pausa el TimeScale es uno
        else
        {
            Time.timeScale = 1;
        }
    }

    /// <summary>
    /// Función pública que inicia el juego y carga el primer nivel
    /// </summary>
    public void StartGame()
    {
        GameStarted = true;
        Pause = false;
        // Llamo una funcion del uiManager
        uiManager.StartGame();
        // Se carga la escena del nivel 1. Esta funcion se llama referenciando al nombre de la escena
        // ( La escena se debe ser previamente agregada la lista de escenas en build settings)
        SceneManager.LoadScene("Level_1");
    }

    /// <summary>
    /// Función publica llamada por un boton de menú para volver a la escena inicial
    /// </summary>
    public void GoToMainMenu()
    {
        GameStarted = false;
        Pause = true;
        uiManager.GoToMainMenue();
        Time.timeScale = 1;
        // Llamo una funcion del uiManager
        // Se carga la escena del nivel 1. Esta funcion se llama referenciando al nombre de la escena
        // ( La escena se debe ser previamente agregada la lista de escenas en build settings)
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Función publica llamada por boton de menú para reanudar el juego
    /// </summary>
    public void Resume()
    {
        TogglePause();
    }

    /// <summary>
    /// Función publica llamada por boton de menú para reiniciar la escena
    /// </summary>
    public void Restart()
    {
        // Función para cargar escena
        // (Requiere que se escriba "using UnityEngine.SceneManagement;"
        // en la parte superior del scrpit para funcionar 
        // Se consulta el nombre de la escena y se recarga
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Función publica llamada por el boton de menú para cerrar la aplicación (Funciona sólo si el juego está compilado).
    /// </summary>
    public void Exit()
    {
        Application.Quit();
    }

    #endregion
}
