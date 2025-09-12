using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Animator
    private Animator _animator;
    #endregion

    #region Variables: Movement
    private Vector2 _input;
    private CharacterController _characterController;
    private Vector3 _direction;
    private bool ignoreInput = false;

    [SerializeField] private float speed;

    #endregion
    #region Variables: Rotation

    [SerializeField] private float rotationSpeed = 500f;
    private Camera _mainCamera;

    #endregion
    #region Variables: Gravity

    private float _gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    private float _velocity;

    #endregion
    #region Variables: Jumping

    [SerializeField] private float jumpPower;

    #endregion
    #region Variables: Dash

    [SerializeField] private float dashPower = 18f;
    [SerializeField] private float dashDuration = 0.5f;
    public float dashCooldown = 3.0f;
    private bool dashOnCooldown = false;
    private bool isDashing = false;

    #endregion

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _mainCamera = Camera.main;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        ApplyRotation();
        ApplyGravity();
        ApplyMovement();
    }

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
        _characterController.Move(_direction * speed * Time.deltaTime);
        _animator.SetFloat("Velocidad", (_input * speed).magnitude);
    }


    public void Move(InputAction.CallbackContext context)
    {
        if (!ignoreInput)
        {
            _input = context.ReadValue<Vector2>();
            _direction = new Vector3(_input.x, 0.0f, _input.y); //Tomo el input y modifico _direction, que luego la uso en ApplyMovement
        }
        
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started) return; //Solo si la tecla se acaba de presionar
        if (!IsGrounded()) return;
        if (isDashing) return;

        _velocity = jumpPower;
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (!context.started) return; //Solo si la tecla se acaba de presionar
        if (!IsGrounded()) return;
        if (isDashing || dashOnCooldown) return;

        _animator.SetTrigger("Dash");
        StartCoroutine(DoDash());
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
            elapsedTime += Time.deltaTime;
            _characterController.Move(_direction * speed * dashPower * Time.deltaTime);
            yield return null;
        }

        _direction = new Vector3(_input.x, 0.0f, _input.y); //La direction toma nuevamente el input

        isDashing = false;
        ignoreInput = false;

        yield return new WaitForSeconds(dashCooldown);
        dashOnCooldown = false;
    }

    private bool IsGrounded() => _characterController.isGrounded; //Para no usar _characterController.isGrounded
}
