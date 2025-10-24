using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] LayerMask groundLayer;
    private Rigidbody2D _rb;
    private float _moveHorizontal;
    private bool _canJump;
    private bool _jump;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _canJump = false;
        _jump = false;
    }

    // Update is called once per frame
    void Update()
    {
        _moveHorizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && _canJump)
            _jump = true;
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (groundLayer == (1 << collision.gameObject.layer))
            _canJump = true;
    }

    void FixedUpdate()
    {
        _rb.linearVelocityX = _moveHorizontal * _speed;
        if (_jump)
        {
            _rb.linearVelocityY = _jumpForce;
            _jump = false;
            _canJump = false;
        }    
    }
}
