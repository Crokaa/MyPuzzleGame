using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _spawnPoint;
    private Vector2 _localScale;
    void Start()
    {
        _localScale = transform.localScale;
    }
    void Update()
    {
        HandleRotation();
        HandleShooting();
    }

    private void HandleRotation()
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 dir = worldPos - (Vector2)transform.position;
        transform.right = dir;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Vector2 newLocalScale = _localScale;
        if (Mathf.Abs(angle) > 90)
            newLocalScale.y *= -1;

        transform.localScale = newLocalScale;
    }

    private void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            Instantiate(_bulletPrefab, _spawnPoint.transform.position, transform.rotation);

    }
}
