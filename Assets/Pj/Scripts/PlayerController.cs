using System;
using System.Collections;
using UnityEditor;

//using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Components
    private Animator _animator;
    private HealthSystem _healthSystem;
    #endregion

    #region Variables: Movement
    [Header("Movement")]
    [SerializeField] private float accelerationRate = 1f; // Qué tan rápido acelera
    [SerializeField] private float maxSpeed = 50f;
    [SerializeField] private float decelerationRate = 2f; // Qué tan rápido desacelera
    private Vector2 _input;
    private CharacterController _characterController;
    private Vector3 _direction;
    private bool ignoreInput = false;

    private float minSpeed = 0.003f; // Velocidad mínima inicial (no puede ser 0 para evitar divisiones por 0)
    private float _currentSpeedMultiplier = 0f; // Multiplicador que va de minSpeed/maxSpeed a 1.0
    public float currentSpeed; // Velocidad actual considerando aceleración
    #endregion

    #region Variables: Rotation
    [SerializeField] private float rotationSpeed = 500f;
    private Camera _mainCamera;
    #endregion
    
    #region Variables: Gravity
    [SerializeField] private float gravityMultiplier = 3.0f;
    private float _gravity = -9.81f;
    private float _velocity;

    #endregion
    
    #region Variables: Derrape
    [Header("Derrape")]
    [SerializeField] private float stopDetectionDelay = 0.15f; // Delay para detectar si el jugador realmente se detuvo
    [SerializeField] private float derrapeDuration = 0.5f;
    private bool isDerrapando = false;
    #endregion
    
    #region Variables: IceSlide
    [Header("Ice Ground")]
    [SerializeField] private float iceMaxSpeed = 30f;
    [SerializeField] private float iceAccelerationRate = 2f;
    [SerializeField] private float iceDecelerationRate = 2f;
    private bool groundIsIce = false;
    private float normalAccelerationRate;
    private float normalDecelerationRate;
    private float normalSpeed;
    #endregion
    
    #region Variables: Jumping
    [Header("Jump")]
    [SerializeField] private float jumpPower;
    [SerializeField] private bool ignoreInputOnJump = false;
    [SerializeField] private float speedAfterJump = 20f; // Velocidad inicial después de aterrizar
    private bool isJumping = false;
 
    #endregion
    
    #region Variables: Dash
    [Header("Dash")]
    [SerializeField] private float dashPower = 18f;
    [SerializeField] private float dashDuration = 0.5f;
    public float dashCooldown = 3.0f;
    private bool dashOnCooldown = false;
    private bool isDashing = false;

    #endregion

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _healthSystem = GetComponent<HealthSystem>();
        _mainCamera = Camera.main;
        _animator = GetComponent<Animator>();

        normalAccelerationRate = accelerationRate;
        normalDecelerationRate = decelerationRate;
        normalSpeed = maxSpeed;
    }

    private void Update()
    {
        ApplyRotation();
        ApplyGravity();
        ApplyMovement();
        DetectIceGround(); 
    }

    #region Movement methods
    private void ApplyGravity()
    {
        if (IsGrounded() && _velocity < 0.0f)
        {
            _velocity = -1.0f;
        }
        else
        {
            _velocity += _gravity * gravityMultiplier * Time.deltaTime;
        }

        _direction.y = _velocity;
    }

    private void ApplyRotation()
    {
        if (_input.sqrMagnitude == 0) return;

        _direction = Quaternion.Euler(0.0f, _mainCamera.transform.eulerAngles.y, 0.0f) * new Vector3(_input.x, 0.0f, _input.y);
        var targetRotation = Quaternion.LookRotation(_direction, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void ApplyMovement()
    {
        if (isDashing) return; //Para no aplicar doble movimiento en el dash

        CalcularAceleracion();

        if (!IsGrounded())
        {
            currentSpeed = maxSpeed; // Velocidad normal en el aire
        }

        if (_input.sqrMagnitude < 0.1f && !ignoreInput && !isJumping) //Si no hay input, moverse hacia adelante al current speed, salvo que se ignore input (en salto, por ejemplo)
        {
            _direction.x = transform.forward.x;
            _direction.z = transform.forward.z;
        }

        _characterController.Move(_direction * currentSpeed * Time.deltaTime);
        _animator.SetFloat("Movement", (_input * currentSpeed).magnitude);
    }

    private void CalcularAceleracion()
    {
        
        if (groundIsIce)
        {
            accelerationRate = iceAccelerationRate;
            decelerationRate = iceDecelerationRate;
            maxSpeed = iceMaxSpeed;
        }
        else
        {
            accelerationRate = normalAccelerationRate;
            decelerationRate = normalDecelerationRate;
            maxSpeed = normalSpeed;
        }

        if (_input.sqrMagnitude > 0.1f) // Si hay input, acelerar
        {
            _currentSpeedMultiplier = Mathf.Lerp(_currentSpeedMultiplier, 1f, accelerationRate * Time.deltaTime);
        }
        else // Si no hay input, desacelerar
        {
            _currentSpeedMultiplier = Mathf.Lerp(_currentSpeedMultiplier, minSpeed, decelerationRate * Time.deltaTime);
        }

        
        
        if (_currentSpeedMultiplier < 0.001f){ //Evitamos, por las dudas, que llegue a 0
            _currentSpeedMultiplier = 0.001f;
        }

        // Aplicar aceleración al movimiento
        currentSpeed = maxSpeed * _currentSpeedMultiplier;
    }
    
    private void DetectIceGround()
    {
        Vector3 origin = _characterController.bounds.center;
        float groundCheckDistance = _characterController.bounds.extents.y + 0.2f;
        RaycastHit hit;

        if (Physics.Raycast(origin, Vector3.down, out hit, groundCheckDistance))
        {
            if (hit.collider.CompareTag("PisoHielo"))
            {
                groundIsIce = true;
            }
            else
            {
                groundIsIce = false;
            }
        }
    }

    #endregion

    #region Inputs
    public void Move(InputAction.CallbackContext context)
    {
        if (!ignoreInput)
        {
            _input = context.ReadValue<Vector2>();
            _direction = new Vector3(_input.x, 0.0f, _input.y); //Tomo el input y modifico _direction, que luego la uso en ApplyMovement
            
            //_currentSpeedMultiplier = minSpeed / speed; // Comenzar desde velocidad mínima
        }

        if (context.canceled)
        {
            StartCoroutine(DetectStopWithDelay());
        }
        
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started) return; //Solo si la tecla se acaba de presionar
        if (!IsGrounded()) return;
        if (isDashing) return;

        _animator.SetTrigger("JumpStart");
        StartCoroutine(DoJump());
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (!context.started) return; //Solo si la tecla se acaba de presionar
        if (!IsGrounded()) return;
        if (isDashing || dashOnCooldown) return;

        _animator.SetTrigger("DashStart");
        _currentSpeedMultiplier = 1f; // Resetear aceleración al dashear
        StartCoroutine(DoDash());
    }
    #endregion

    #region Corroutines
    private IEnumerator DetectStopWithDelay()
    {
        yield return new WaitForSeconds(stopDetectionDelay);
        
        // Verificar que sigue sin input después del delay
        if (_input.sqrMagnitude < 0.1f && !isDashing)
        {
            if (currentSpeed > maxSpeed * 0.5f){
                StartCoroutine(DoDerrape());
            } // Si la velocidad es mayor al 50% de la velocidad máxima, derrapamos
            
        }
    }

    private IEnumerator DoDerrape()
    {
        isDerrapando = true;
        _animator.SetBool("Derrapando", true);

        yield return new WaitForSeconds(derrapeDuration); //Duración del derrape
        isDerrapando = false;
        _animator.SetBool("Derrapando", false);
    }

    private IEnumerator DoJump()
    {
        isJumping = true;
        ignoreInput = true; //Ignoramos input para detectar si salta en el lugar o no

        if (_input.sqrMagnitude < 0.1f) //Si no se está apretando WASD, el salto es en el lugar
        {
            _direction.x = 0f;
            _direction.z = 0f;
        }

        _currentSpeedMultiplier = 1f; // Resetear aceleración al saltar
        _velocity = jumpPower;

        if (!ignoreInputOnJump) ignoreInput = false; //Si no se quiere ignorar el input al saltar, lo volvemos a permitir

        yield return null; //Esperar un frame para que detecte que no está en el suelo

        //Esperamos que toque el suelo para volver a tomar el input
        while (!IsGrounded())
        {
            yield return null;
        }

        ResetInput();

        if (_input.sqrMagnitude < 0.1f)
        {
            _currentSpeedMultiplier = 0.0f; //Si no hay input, comenzar desde 0 al aterrizar
        }
        else
        {
            _currentSpeedMultiplier = speedAfterJump/maxSpeed; //Si hay input, comenzar desde una velocidad inicial al aterrizar
        }

        ignoreInput = false;
        isJumping = false;
    }
    private IEnumerator DoDash()
    {
        isDashing = true;
        dashOnCooldown = true;
        ignoreInput = true;

        float elapsedTime = 0f;

        if (_input == Vector2.zero) //Si no se está apretando WASD, la direction será a donde mira el jugador
        {
            _direction = new Vector3(transform.forward.x, 0f, transform.forward.z);
        }

        while (elapsedTime < dashDuration)
        {
            if (_healthSystem.getIsDead() == true)
            {
                break;
            }

            elapsedTime += Time.deltaTime;
            _characterController.Move(_direction * maxSpeed * dashPower * Time.deltaTime);
            yield return null;
        }

        ResetInput();
        ignoreInput = false;
        isDashing = false;
        

        yield return new WaitForSeconds(dashCooldown);
        dashOnCooldown = false;
    }
    #endregion

    private void ResetInput()
    {//Resetea el input para no depender del InputContext que solo se ejecuta al presionar teclas
        _input.x = Input.GetAxisRaw("Horizontal");
        _input.y = Input.GetAxisRaw("Vertical");

        if (_input.sqrMagnitude < 0.1f)
        {
            _direction.x = 0f;
            _direction.z = 0f;
        }
    }

    #region Getters
    public bool IsGrounded() => _characterController.isGrounded; //Para no usar _characterController.isGrounded
    public bool IsDashing() => isDashing;
    public bool IsJumping() => isJumping;
    public bool IsMoving()
    {
        bool isMoving = false;
        if (currentSpeed > 1f)
        {
            isMoving = true;
        }

        return isMoving;
    }

    public bool IsDerrapando() => isDerrapando;
    #endregion
}
