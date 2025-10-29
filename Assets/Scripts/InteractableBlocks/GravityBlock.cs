using UnityEngine;

public class Gravityblock : MonoBehaviour, IChangeableBlock
{
    public void ApplyChange()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();
        player.ChangeGravity(transform.right);
    }
}
