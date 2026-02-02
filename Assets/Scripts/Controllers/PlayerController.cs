using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _pushSpeed;
    [SerializeField] private float _jumpForce;
    [Header("Damping")]
    [SerializeField] private float _moveDamp;
    [SerializeField] private float _stopDamp;
    private PlayerInputActions _playerInputActions;
    private LayerMask _initialExcludeLayerMask;
    private Rigidbody2D _rb;
    private float _moveHorizontal = 0.0f;
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
    public bool IsGrounded
    {
        private set
        {
            _isGrounded = value;

            if (!_isGrounded || IsMoving)
                _rb.linearDamping = _moveDamp;
            else
                _rb.linearDamping = _stopDamp;

            if (_wasPushing)
                _push = true;

            if (_isGrounded && _pushableBox != null)
                GameManager.instance.InteractShow();
            else if (!_isGrounded)
                GameManager.instance.InteractHide();

        }
        get
        { return _isGrounded; }
    }

    private PushableObject _pushableBox;
    private float _currentSpeed;
    private bool _push;
    private bool _isMoving;
    private bool _isGrounded;
    private bool _jump;
    private bool _wasPushing;
    public static PlayerController instance;
    void Awake()
    {

        if (instance == null)
        {
            instance = this;
            _rb = GetComponent<Rigidbody2D>();
            _initialExcludeLayerMask = GetComponent<BoxCollider2D>().excludeLayers;
            _playerInputActions = new PlayerInputActions();
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        _currentSpeed = _moveSpeed;
        IsGrounded = false;
        _jump = false;
        IsMoving = false;
        _push = false;
        _wasPushing = false;
    }

    void OnEnable()
    {
        _playerInputActions.Enable();

        _playerInputActions.Player.Move.performed += MovePlayer;
        _playerInputActions.Player.Jump.performed += PlayerJump;
        _playerInputActions.Player.Push.performed += PlayerPush;

        _playerInputActions.Player.Move.canceled += MovePlayer;
        _playerInputActions.Player.Jump.canceled += PlayerJump;
        _playerInputActions.Player.Push.canceled += PlayerPush;
    }

    void OnDisable()
    {
        _playerInputActions.Disable();

        _playerInputActions.Player.Move.performed -= MovePlayer;
        _playerInputActions.Player.Jump.performed -= PlayerJump;
        _playerInputActions.Player.Push.performed -= PlayerPush;

        _playerInputActions.Player.Move.canceled -= MovePlayer;
        _playerInputActions.Player.Jump.canceled -= PlayerJump;
        _playerInputActions.Player.Push.canceled -= PlayerPush;
    }

    private void MovePlayer(InputAction.CallbackContext context)
    {
        _moveHorizontal = context.ReadValue<Vector2>().x;

        if (!context.performed)
            IsMoving = false;

    }
    private void PlayerJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (IsGrounded)
        {
            _jump = true;

            if (_push)
                _wasPushing = true;

            _push = false;
        }
    }

    private void PlayerPush(InputAction.CallbackContext context)
    {
        //if (!_canPush || !IsGrounded) return;

        if (context.performed)
            _push = true;
        else
        {
            StopPush();
            _push = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if ((GameLayers.instance.JumpableLayers & (1 << collision.gameObject.layer)) != 0)
            IsGrounded = true;

        if ((GameLayers.instance.ColorRestrictiveJumpLayer & (1 << collision.gameObject.layer)) != 0 &&
        collision.GetComponent<SpriteRenderer>().color != GetComponent<SpriteRenderer>().color)
            IsGrounded = true;

        if ((GameLayers.instance.InteractableLayer & (1 << collision.gameObject.layer)) != 0 && _pushableBox == null && collision.transform.right == transform.right)
        {
            if (IsGrounded)
                GameManager.instance.InteractShow();

            _pushableBox = collision.GetComponentInParent<PushableObject>();
        }
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

        if ((GameLayers.instance.InteractableLayer & (1 << collision.gameObject.layer)) != 0 && _pushableBox != null)
        {
            GameManager.instance.InteractHide();
            StopPush();
            _pushableBox = null;
        }
    }

    private void StopPush()
    {
        _currentSpeed = _moveSpeed;

        if (_pushableBox != null)
            _pushableBox.StopPush();
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        TurnPlayer();
    }

    void FixedUpdate()
    {
        HandlePush();
        HandleMoveSide();
        HandleJump();
    }

    private void TurnPlayer()
    {
        if (_moveHorizontal != 0.0f)
        {
            transform.localScale = new Vector3(Math.Sign(_moveHorizontal) * Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

            // There must be a cleaner way of doing this, for now it works
            GameObject gunPivotObject = GameObject.FindGameObjectWithTag("GunPivot");
            Vector2 gunPivotLocalScale = gunPivotObject.transform.localScale;

            gunPivotObject.transform.localScale = new Vector2(Math.Sign(_moveHorizontal) * Math.Abs(gunPivotLocalScale.x), gunPivotLocalScale.y);

        }
    }

    private bool IsGravityUpDown()
    {
        return GameManager.instance.Gravity.x == 0f;
    }

    private bool IsGravityUpsideDown()
    {
        return GameManager.instance.Gravity.x == 0f && GameManager.instance.Gravity.y > 0;
    }

    private void HandlePush()
    {
        if (_pushableBox == null || !_push) return;

        if (_moveHorizontal == 0)
        {
            _pushableBox.StopPush();
            return;
        }


        Vector2 boxCenter = _pushableBox.transform.position;
        Vector2 playerCenter = transform.position;

        if (IsGravityUpDown())
        {
            if ((boxCenter.x > playerCenter.x && _moveHorizontal * transform.right.x <= 0) || (boxCenter.x < playerCenter.x && _moveHorizontal * transform.right.x >= 0))
                return;
        }
        else
        {
            if ((boxCenter.y > playerCenter.y && _moveHorizontal * transform.right.x <= 0) || (boxCenter.y < playerCenter.y && _moveHorizontal * transform.right.x >= 0))
                return;
        }

        _pushableBox.Push();
        _currentSpeed = _pushSpeed;
    }

    private void HandleMoveSide()
    {
        if (_moveHorizontal != 0)
        {
            if (IsGravityUpDown())
            {
                if (IsGravityUpsideDown())
                    _rb.linearVelocity = new Vector2(_moveHorizontal * _currentSpeed * -transform.right.x, _rb.linearVelocity.y);
                else
                    _rb.linearVelocity = new Vector2(_moveHorizontal * _currentSpeed * transform.right.x, _rb.linearVelocity.y);
            }
            else
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _moveHorizontal * _currentSpeed * transform.right.y);

            IsMoving = true;
        }
    }
    private void HandleJump()
    {
        if (_jump)
        {
            if (IsGravityUpDown())
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce * -Physics2D.gravity.normalized.y);
            else
                _rb.linearVelocity = new Vector2(_jumpForce * -Physics2D.gravity.normalized.x, _rb.linearVelocity.y);
        }
    }
    public void Rotate(float angle)
    {
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
