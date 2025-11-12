using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Transform player;
    private Vector3 _offset = new Vector3(0, 2.0f, -10);

    void Update()
    {
        transform.position = player.transform.position + _offset;
    }
}
