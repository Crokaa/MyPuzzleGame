using UnityEngine;

public class PushableObject : MonoBehaviour
{

    private Rigidbody2D _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.InteractShow();

            // This line is so I don't forget what I want to do
            _rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.InteractHide();

            // This line is so I don't forget what I want to do
            _rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }
}
