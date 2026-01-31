using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Vector3 _offset = new Vector3(0, 2.0f, -10);

    void Update()
    {
        transform.position = PlayerController.instance.transform.position + _offset;
    }
}
