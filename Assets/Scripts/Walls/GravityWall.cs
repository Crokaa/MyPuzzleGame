using UnityEngine;

public class GravityWall : MonoBehaviour, IChangeableWall
{
    public void ApplyChange()
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();
        player.ChangeGravity(transform.right);
    }
}
