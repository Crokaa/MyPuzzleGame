using UnityEngine;

public class PushableObject : MonoBehaviour
{

    private Rigidbody2D _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.bodyType = RigidbodyType2D.Static;
    }

    public void Push()
    {
        // This line is so I don't forget what I want to do
        _rb.bodyType = RigidbodyType2D.Dynamic;
    }

    public void StopPush()
    {
        // This line is so I don't forget what I want to do
        _rb.bodyType = RigidbodyType2D.Static;
    }
}
