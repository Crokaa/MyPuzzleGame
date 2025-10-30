using UnityEngine;

public class ColorWall : MonoBehaviour
{
    private Color _currentColor;
    void Start()
    {
        _currentColor = GetComponent<SpriteRenderer>().color;
    }

    // TODO: make the player not bump into the wall first before changing its excludeLayers 
    // Probably have a trigger that updates the player's excludeLayers so it doesn't bump into the wall before changing the excludeLayers LayerMask
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") || collision.gameObject.GetComponent<SpriteRenderer>().color != _currentColor)
            return;

        GameObject player = collision.gameObject;

        player.GetComponent<BoxCollider2D>().excludeLayers |= 1 << gameObject.layer;

    }
}
