using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [Header("Damping")]
    [SerializeField] private float _moveDamp;
    [SerializeField] private float _stopDamp;
    private static float FLOATPRECISION = 0.000001f;
    private PlayerInputActions playerInputActions;
    private LayerMask _initialExcludeLayerMask;
    private static float GRAVITY = 9.8f;
    private Rigidbody2D _rb;
    private float _moveHorizontal;
    private bool IsMoving
    {
        set
        {
            _isMoving = value;

            if (_isMoving || !IsGrounded)
                _rb.linearDamping = _moveDamp;
            else
                _rb.linearDamping = _stopDamp;
        }
        get { return _isMoving; }
    }
    private bool IsGrounded
    {
        set
        {
            _isGrounded = value;

            if (!_isGrounded || IsMoving)
                _rb.linearDamping = _moveDamp;
            else
                _rb.linearDamping = _stopDamp;
        }
        get
        { return _isGrounded; }
    }
    private bool _isMoving;
    private bool _isGrounded;
    private bool _jump;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        IsGrounded = false;
        _jump = false;
        IsMoving = false;
        _initialExcludeLayerMask = GetComponent<BoxCollider2D>().excludeLayers;
        playerInputActions = new PlayerInputActions();
    }
    void OnEnable()
    {
        playerInputActions.Enable();

        playerInputActions.Player.Move.performed += MovePlayer;
        playerInputActions.Player.Jump.performed += PlayerJump;

        playerInputActions.Player.Move.canceled += MovePlayer;
        playerInputActions.Player.Jump.canceled += PlayerJump;
    }

    void OnDisable()
    {
        playerInputActions.Disable();

        playerInputActions.Player.Move.performed -= MovePlayer;
        playerInputActions.Player.Jump.performed -= PlayerJump;

        playerInputActions.Player.Move.canceled -= MovePlayer;
        playerInputActions.Player.Jump.canceled -= PlayerJump;
    }

    private void MovePlayer(InputAction.CallbackContext context)
    {
        _moveHorizontal = context.ReadValue<Vector2>().x;
        if (context.performed)
        {
            transform.localScale = new Vector3(Math.Sign(_moveHorizontal) * Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

            // There must be a cleaner way of doing this, for now it works
            GameObject gunPivotObject = GameObject.FindGameObjectWithTag("GunPivot");
            Vector2 gunPivotLocalScale = gunPivotObject.transform.localScale;

            gunPivotObject.transform.localScale = new Vector2(Math.Sign(_moveHorizontal) * Math.Abs(gunPivotLocalScale.x), gunPivotLocalScale.y);

        }
        else
        {
            IsMoving = false;
        }
    }
    private void PlayerJump(InputAction.CallbackContext context)
    {

        if (!context.performed) return;

        if (IsGrounded)
            _jump = true;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if ((GameLayers.instance.JumpableLayers & (1 << collision.gameObject.layer)) != 0)
            IsGrounded = true;

        if ((GameLayers.instance.ColorRestrictiveJumpLayer & (1 << collision.gameObject.layer)) != 0 &&
        collision.GetComponent<SpriteRenderer>().color != GetComponent<SpriteRenderer>().color)
            IsGrounded = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if ((GameLayers.instance.JumpableLayers & (1 << collision.gameObject.layer)) != 0)
        {
            IsGrounded = false;
            _jump = false;
        }

        if ((GameLayers.instance.ColorRestrictiveJumpLayer & (1 << collision.gameObject.layer)) != 0 &&
        collision.GetComponent<SpriteRenderer>().color != GetComponent<SpriteRenderer>().color)
        {
            IsGrounded = false;
            _jump = false;
        }
    }

    void FixedUpdate()
    {
        HandleMoveSide();
        HandleJump();
    }

    private void HandleMoveSide()
    {
        if (_moveHorizontal != 0)
        {
            if (-FLOATPRECISION <= transform.right.y && transform.right.y <= FLOATPRECISION)
                _rb.linearVelocity = new Vector2(_moveHorizontal * _speed * transform.right.x, _rb.linearVelocity.y);
            else
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _moveHorizontal * _speed * transform.right.y);

            IsMoving = true;
        }
    }
    private void HandleJump()
    {
        if (_jump)
        {
            if (-FLOATPRECISION <= transform.right.y && transform.right.y <= FLOATPRECISION)
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce * -Physics2D.gravity.normalized.y);
            else
                _rb.linearVelocity = new Vector2(_jumpForce * -Physics2D.gravity.normalized.x, _rb.linearVelocity.y);
        }
    }

    // TODO: Change this so it's based on the GameManager's Gravity variable and the player doesn't know gravity
    public void ChangeGravity(Vector2 currentForce)
    {
        Physics2D.gravity = currentForce * GRAVITY;

        Vector2 targetVector = Vector2.Perpendicular(currentForce);

        float angle = Vector2.SignedAngle(Vector2.right, targetVector);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    public void ChangeColor(Color color)
    {
        if (color != GetComponent<SpriteRenderer>().color)
        {
            ResetLayers();
            GetComponent<SpriteRenderer>().color = color;
            GetComponent<BoxCollider2D>().excludeLayers |= ColorLayerDB.ColorLayers[color];
        }
    }

    private void ResetLayers()
    {
        GetComponent<BoxCollider2D>().excludeLayers = _initialExcludeLayerMask;
    }
}
