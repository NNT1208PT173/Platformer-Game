using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private float _walkSpeed = 150f;
    private float _airSpeed = 100f;
    private float _runSpeed = 250f;
    private float jumpImpulse = 8f;
    public float CurrentMoveSpeed
    {
        get
        {
            if (IsMoving && !touchingDirections.IsOnWall)
            {
                Debug.Log(touchingDirections.IsGrounded);
                if (touchingDirections.IsGrounded)
                {
                    
                    if (IsRunning) return _runSpeed;
                    else return _walkSpeed;
                }
                else
                {
                    return _airSpeed;
                }
            }
            else return 0;
        }
    }

    private bool _isMoving = false;
    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(CONSTANT.isMoving, value);
        }
    }

    private bool _isRunning = false;
    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }

        private set
        {
            _isRunning = value;
            animator.SetBool(CONSTANT.isRunning, value);
        }
    }

    private bool _isFacingRight = true;
    public bool IsFacingRight
    {
        get
        {
            return _isFacingRight;
        }
        private set
        {
            if(_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }


    private Vector2 _moveInput;
    Rigidbody2D rb;
    Animator animator;
    TouchingDirections touchingDirections;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
    }

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(_moveInput.x * CurrentMoveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        animator.SetFloat(CONSTANT.yVelocity, rb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();

        IsMoving = _moveInput != Vector2.zero;

        SetFacingDirection(_moveInput);

    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if(moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if(IsFacingRight && moveInput .x< 0)
        {
            IsFacingRight = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            animator.SetTrigger(CONSTANT.jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }
}
