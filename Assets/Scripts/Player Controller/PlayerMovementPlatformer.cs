using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private GroundChecker groundCheck;

    [SerializeField] private PhysicsMaterial2D airFrictionPhysicsMaterial;
    [SerializeField] private PhysicsMaterial2D groundFrictionPhysicsMaterial;
    
    [SerializeField] private Animator _playerAnimator;

    private Rigidbody2D _rb2d;
    private InputAction _moveAction;
    private InputAction _jumpAction;

    private float _speedModifier = 1;
    private float _jumpModifier = 1;

    private float _prevHorizontalSpeed = 0f;
    
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
       
       
        if (horizontalSpeed != 0)
        {
            // trigger animation switch
            _playerAnimator.SetBool("isRunning", true);
            // change directiom based on speed
            transform.localScale = new Vector3(Mathf.Sign(horizontalSpeed), 1f, 1f);
            // accelarate (set) up up to (or down to) horizontal speed
            _rb2d.linearVelocity = new Vector2(horizontalSpeed, currentYSpeed);
            
        }
        else
        {
            _playerAnimator.SetBool("isRunning", false);
            // decelerate to zero
            float newHorizontalSpeed = Mathf.SmoothDamp(_rb2d.linearVelocity.x, 0, ref horizontalSpeed, 0.3f);
        }
    }

    // normal tick update better for input handling
    private void Update()
    {
        // jumping
        if (_jumpAction.triggered && groundCheck.IsGrounded)
        {
            _rb2d.AddForce(new Vector2(0f, jumpForce * _jumpModifier), ForceMode2D.Impulse);
        }

        _rb2d.sharedMaterial = groundCheck.IsGrounded ?
            groundFrictionPhysicsMaterial : airFrictionPhysicsMaterial;
    }

    public void SetSpeedModifier(float modifier)
    {
        _speedModifier = modifier;
    }

    public void SetJumpModifier(float modifier)
    {
        _jumpModifier = modifier;
    }

    public float GetSpeedModifier()
    {
        return _speedModifier;
    }
}
