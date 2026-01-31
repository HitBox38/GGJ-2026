using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Transform spriteHolder;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    
    private InputAction _moveAction;
    
    private void Awake()
    {
        _animator = spriteHolder.GetComponent<Animator>();
        _spriteRenderer = spriteHolder.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
    }

    private void Update()
    {
        var horizontalMovement = _moveAction.ReadValue<Vector2>().x;
        if (!Mathf.Approximately(horizontalMovement, 0)) 
            _spriteRenderer.flipX = horizontalMovement < 0;
    }
}