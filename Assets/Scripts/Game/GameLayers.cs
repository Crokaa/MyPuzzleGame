using UnityEngine;

public class GameLayers : MonoBehaviour
{
    [SerializeField] private LayerMask _jumpableLayers;
    [SerializeField] private LayerMask _redWallLayer;
    [SerializeField] private LayerMask _blueWallLayer;
    [SerializeField] private LayerMask _greenWallLayer;
    public LayerMask JumpableLayers { get { return _jumpableLayers; } }
    public LayerMask ColorRestrictiveJumpLayer { get { return _redWallLayer | _blueWallLayer | _greenWallLayer; } }
    public LayerMask RedWallLayer { get { return _redWallLayer; } }
    public LayerMask BlueWallLayer { get { return _blueWallLayer; } }
    public LayerMask GreenWallLayer { get { return _greenWallLayer; } }
    public static GameLayers instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }
}
