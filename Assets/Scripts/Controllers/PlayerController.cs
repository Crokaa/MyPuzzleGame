using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] LayerMask _jumpableLayer;
    [SerializeField] private float _moveDamp;
    [SerializeField] private float _stopDamp;
    private bool IsMoving
    {
        set
        {
            _isMoving = value;

            if (_isMoving)
                _rb.linearDamping = _moveDamp;
            else
                _rb.linearDamping = _stopDamp;

        }
    }
    private LayerMask _initialExcludeLayerMask;
    private static float GRAVITY = 9.8f;
    private Vector2 _currentHorizontal;
    private Rigidbody2D _rb;
    private float _moveHorizontal;
    private bool _canJump;
    private bool _jump;
    private bool _isMoving;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _canJump = false;
        _jump = false;
        _isMoving = false;
        _currentHorizontal = new Vector2(1, 0);
        _initialExcludeLayerMask = GetComponent<BoxCollider2D>().excludeLayers;
    }

    void Update()
    {
        _moveHorizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && _canJump)
        {
            _jump = true;
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Grounded check has to change since the player now changes gravity
        if ((_jumpableLayer & (1 << collision.gameObject.layer)) != 0)
            _canJump = true;

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // Grounded check has to change since the player now changes gravity
        if ((_jumpableLayer & (1 << collision.gameObject.layer)) != 0)
        {
            _canJump = false;
            _jump = false;
        }
    }

    void FixedUpdate()
    {

        if (_moveHorizontal == 0 && _canJump)
        {
            IsMoving = false;
        }
        else
            IsMoving = true;

        _rb.AddForce(_currentHorizontal * _moveHorizontal * _speed);

        if (_jump)
        {
            _rb.AddForce(_jumpForce * -Physics2D.gravity);
            // Ideally the player hits the groudn and these values become false, but for now these have to be like this
            _canJump = false;
            _jump = false;
        }
    }

    public void ChangeGravity(Vector2 currentForce)
    {
        Physics2D.gravity = currentForce * GRAVITY;
        _currentHorizontal = Quaternion.AngleAxis(90, new Vector3(0, 0, 1)) * currentForce;
        transform.right = Quaternion.AngleAxis(90, new Vector3(0, 0, 1)) * currentForce;
    }

    public void ChangeColor(Color color)
    {
        if (color != GetComponent<SpriteRenderer>().color)
        {
            ResetLayers();
            GetComponent<SpriteRenderer>().color = color;
        }
    }

    private void ResetLayers()
    {
        GetComponent<BoxCollider2D>().excludeLayers = _initialExcludeLayerMask;
    }
}
