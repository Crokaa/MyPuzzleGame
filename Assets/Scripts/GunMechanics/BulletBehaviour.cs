using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private float _speed = 15;
    [SerializeField] private float _destroyTimer = 5;
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        SetDestroyTimer();

        SetStraightVelocity();
    }

    private void SetDestroyTimer()
    {
        Destroy(gameObject, _destroyTimer);
    }

    private void SetStraightVelocity()
    {
        _rb.linearVelocity = transform.right * _speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((GameLayers.instance.BulletDestroyerLayer & (1 << collision.gameObject.layer)) != 0)
            Destroy(gameObject);

        if ((GameLayers.instance.EnvironmentChangeLayer & (1 << collision.gameObject.layer)) != 0)
            collision.GetComponent<IChangeableBlock>().ApplyChange();
    }
}
