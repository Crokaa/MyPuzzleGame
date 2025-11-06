using UnityEngine;

public class GameLayers : MonoBehaviour
{
    [SerializeField] private LayerMask _jumpableLayers;
    [SerializeField] private LayerMask _redWallLayer;
    [SerializeField] private LayerMask _blueWallLayer;
    [SerializeField] private LayerMask _greenWallLayer;
    [SerializeField] private LayerMask _bulletDestroyer;
    [SerializeField] private LayerMask _environmentChangeLayer;
    public LayerMask JumpableLayers { get { return _jumpableLayers; } }
    public LayerMask ColorRestrictiveJumpLayer { get { return _redWallLayer | _blueWallLayer | _greenWallLayer; } }
    public LayerMask RedWallLayer { get { return _redWallLayer; } }
    public LayerMask BlueWallLayer { get { return _blueWallLayer; } }
    public LayerMask GreenWallLayer { get { return _greenWallLayer; } }
    public LayerMask BulletDestroyerLayer { get { return _bulletDestroyer; } }

    public LayerMask EnvironmentChangeLayer { get { return _environmentChangeLayer; } }

    public static GameLayers instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }
}
