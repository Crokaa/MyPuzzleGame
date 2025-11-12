using UnityEngine;

public class Gravityblock : MonoBehaviour, IChangeableBlock
{
    public void ApplyChange()
    {
        GameManager.instance.ChangeGravity(transform.right);
    }
}
