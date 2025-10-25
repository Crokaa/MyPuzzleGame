using System;
using UnityEngine;

public class GunBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab; 
    [SerializeField] private Transform _spawnPoint; 
    private Vector2 _localScale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _localScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        HandleRotation();
        HandleShooting();
    }

    private void HandleRotation()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y).normalized;

        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
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
