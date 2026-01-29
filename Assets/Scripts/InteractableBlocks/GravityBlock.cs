using UnityEngine;

public class Gravityblock : MonoBehaviour, IChangeableBlock
{

    private static readonly float FLOATPRECISION = 0.00001f;
    public void ApplyChange()
    {
        Vector2 vecToApply = transform.right;

        vecToApply.x = Mathf.Abs(vecToApply.x) <= FLOATPRECISION ? 0 : vecToApply.x;
        vecToApply.y = Mathf.Abs(vecToApply.y) <= FLOATPRECISION ? 0 : vecToApply.y;

        GameManager.instance.ChangeGravity(vecToApply);
    }
}
