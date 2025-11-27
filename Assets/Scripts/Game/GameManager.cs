using UnityEngine.InputSystem;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerController _player;
    [Header("Canvas")]
    [SerializeField] private GameObject _inGameCanvas;
    [SerializeField] private GameObject _pauseCanvas;
    private bool IsPaused
    {
        get { return _isPaused; }
        set
        {
            _isPaused = value;
            if (IsPaused)
            {
                Time.timeScale = 0f;
                _pauseCanvas.SetActive(true);
            }
            else
            {
                Time.timeScale = 1f;
                _pauseCanvas.SetActive(false);
            }
        }
    }
    private PlayerInputActions _inputActions;

    private static readonly float GRAVITYFORCE = 9.8f; // Positive value, but force will be down (negative)
    public Vector2 Gravity
    {
        get { return Physics2D.gravity; }
        private set { Physics2D.gravity = value; }
    }
    public static GameManager instance;
    private bool _isPaused;

    void Awake()
    {
        if (instance == null)
            instance = this;

        Physics2D.gravity = new Vector2(0, -GRAVITYFORCE);
        _inGameCanvas.SetActive(false);
        _inputActions = new PlayerInputActions();
        IsPaused = false;
    }

    void OnEnable()
    {
        _inputActions.Enable();

        _inputActions.Game.Pause.performed += PauseUnpause;

        _inputActions.Game.Pause.canceled += PauseUnpause;
    }

    void OnDisable()
    {
        _inputActions.Disable();

        _inputActions.Game.Pause.performed -= PauseUnpause;

        _inputActions.Game.Pause.canceled -= PauseUnpause;
    }

    private void PauseUnpause(InputAction.CallbackContext context)
    {
        if (context.canceled) return;

        IsPaused = !IsPaused;
    }

    public void ChangeGravity(Vector2 currentForce)
    {
        Gravity = currentForce * GRAVITYFORCE;

        // Rotate the player
        Vector2 targetVector = Vector2.Perpendicular(currentForce);
        float angle = Vector2.SignedAngle(Vector2.right, targetVector);
        _player.Rotate(angle);
    }
    public void InteractShow()
    {
        // Not sure about this line yet
        _inGameCanvas.SetActive(true);

        foreach (Transform child in _inGameCanvas.transform)
        {
            if (child.CompareTag("InteractText"))
            {
                child.gameObject.SetActive(true);
                break;
            }
        }
    }

    public void InteractHide()
    {
        // This check is made in case the game is closed and the player is inside an interact zone
        if (_inGameCanvas != null)
        {
            foreach (Transform child in _inGameCanvas.transform)
            {
                if (child.CompareTag("InteractText"))
                {
                    child.gameObject.SetActive(false);
                    break;
                }
            }

            // Not sure about this line yet
            _inGameCanvas.SetActive(false);
        }
    }
}
