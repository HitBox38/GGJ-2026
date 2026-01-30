using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private GroundChecker groundCheck;
    
    private Rigidbody2D _rb2d;
    private InputAction _moveAction;
    private InputAction _jumpAction;

    private float _speedModifier = 1;
    
    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }
    
    private void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _jumpAction = InputSystem.actions.FindAction("Jump");
    }

    // fixed update better for physics calculations
    private void FixedUpdate()
    {
        // side movement
        var horizontalSpeed = _moveAction.ReadValue<Vector2>().x * speed * _speedModifier;
        var currentYSpeed = _rb2d.linearVelocity.y; // keeping gravity and jumps
        _rb2d.angularVelocity = 0f;
        _rb2d.linearVelocity = new Vector2(horizontalSpeed, currentYSpeed);
    }

    // normal tick update better for input handling
    private void Update()
    {
        // jumping
        if (_jumpAction.triggered && groundCheck.IsGrounded)
        {
            _rb2d.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    public void SetSpeedModifier(float modifier)
    {
        _speedModifier = modifier;
    }
}
