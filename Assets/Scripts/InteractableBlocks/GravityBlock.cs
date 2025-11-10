using UnityEngine;

public class Gravityblock : MonoBehaviour, IChangeableBlock
{
    public void ApplyChange()
    {
        // TODO: Change the gravity to the GameManager
        PlayerController player = FindFirstObjectByType<PlayerController>();
        player.ChangeGravity(transform.right);
    }
}
