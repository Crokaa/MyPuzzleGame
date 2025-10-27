using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] LayerMask _groundLayer;
    private Vector2 _gravityForce;
    private Vector2 _currentHorizontal;
    private Rigidbody2D _rb;
    private float _moveHorizontal;
    private bool _canJump;
    private bool _jump;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _canJump = false;
        _jump = false;
        _gravityForce = new Vector2(0, -9.8f);
        _currentHorizontal = new Vector2(1, 0);
    }

    void Update()
    {
        _moveHorizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && _canJump)
            _jump = true;

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Grounded check has to change since the player now changes gravity
        if ((_groundLayer & (1 << collision.gameObject.layer)) != 0)
            _canJump = true;
    }

    void FixedUpdate()
    {
        _rb.linearVelocity += (_currentHorizontal * _moveHorizontal * _speed * Time.deltaTime) + (_gravityForce * Time.deltaTime);

        // TODO: Rotate player based on gravity
        // Player slips too much.

        if (_jump)
        {
            _rb.linearVelocity += _jumpForce * -_gravityForce.normalized;
            _jump = false;
            _canJump = false;
        }
    }

    internal void ChangeGravity(Vector2 currentForce)
    {
        _gravityForce = currentForce * 9.8f;
        _currentHorizontal = Quaternion.AngleAxis(90, new Vector3(0, 0, 1)) * currentForce;

    }
}
