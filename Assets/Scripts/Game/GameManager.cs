using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    private static readonly float GRAVITYFORCE = 9.8f; // Positive value, but force will be down (negative)
    public Vector2 Gravity
    {
        get { return Physics2D.gravity; }
        private set { Physics2D.gravity = value; }
    }
    public static GameManager instance;
    void Awake()
    {
        if (instance == null)
            instance = this;

        Physics2D.gravity = new Vector2(0, -GRAVITYFORCE);
    }

    public void ChangeGravity(Vector2 currentForce)
    {
        Gravity = currentForce * GRAVITYFORCE;

        // Rotate the player
        Vector2 targetVector = Vector2.Perpendicular(currentForce);
        float angle = Vector2.SignedAngle(Vector2.right, targetVector);
        _player.Rotate(angle);
    }
}
