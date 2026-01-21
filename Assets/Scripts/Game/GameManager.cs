using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    private enum GameState { MenuState, InGame, Pause }; // if needed these will be public
    private class UINames
    {
        public const string CONTINUETEXT = "ContinueText";
        public const string QUITTOMENUTEXT = "QuitToMenuText";
        public const string SETTINGSTEXT = "SettingsText";
        public const string MENUSCENENAME = "MenuScene";
        public const string TESTLEVELCENENAME = "TestLevelScene";
        public const string MENUCANVASTAG = "MenuCanvas";
        public const string STARTGAMETEXT = "StartGameText";
        public const string CHOOSELEVEL = "ChooseLevelText";
        public const string QUITGAMETEXT = "QuitGameText";

    }

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
                _pauseCanvas.SetActive(false);
                Time.timeScale = 1f;
            }
        }
    }

    private PlayerInputActions _inputActions;
    private GameState CurrentGameState { get; set; }

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
        CurrentGameState = GameState.MenuState;

        SetOnPauseButtons();
    }
    void Start()
    {
        SetMenuCanvas();
    }

    private void SetMenuCanvas()
    {
        GameObject menuCanvas = GameObject.FindGameObjectWithTag(UINames.MENUCANVASTAG);

        Button[] menuButtons = menuCanvas.GetComponentsInChildren<Button>();
        foreach (Button button in menuButtons)
        {
            button.navigation = new Navigation { mode = Navigation.Mode.None };
            if (button.name == UINames.STARTGAMETEXT)
                button.onClick.AddListener(() =>
                {
                    CurrentGameState = GameState.InGame;
                    SceneController.instance.LoadScene(UINames.TESTLEVELCENENAME);
                    _player.gameObject.SetActive(true);
                });
            else if (button.name == UINames.CHOOSELEVEL)
                button.onClick.AddListener(() => Debug.Log("Choose Level"));
            else if (button.name == UINames.SETTINGSTEXT)
                button.onClick.AddListener(() => Debug.Log("Settings"));
            else if (button.name == UINames.QUITGAMETEXT)
                button.onClick.AddListener(() => Debug.Log("Quitting the game..."));
        }
    }

    private void SetOnPauseButtons()
    {

        Button[] buttons = _pauseCanvas.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.navigation = new Navigation { mode = Navigation.Mode.None };
            if (button.name == UINames.CONTINUETEXT)
                button.onClick.AddListener(() => IsPaused = !IsPaused);
            else if (button.name == UINames.SETTINGSTEXT)
                button.onClick.AddListener(() => Debug.Log("Settings"));
            else if (button.name == UINames.QUITTOMENUTEXT)
                button.onClick.AddListener(() =>
                {
                    CurrentGameState = GameState.MenuState;
                    SceneManager.LoadScene(UINames.MENUSCENENAME);
                });
        }
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
        if (context.canceled && (CurrentGameState == GameState.InGame || CurrentGameState == GameState.Pause)) return;

        IsPaused = !IsPaused;

        if (IsPaused)
            CurrentGameState = GameState.Pause;
        else
            CurrentGameState = GameState.InGame;
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
        if (_inGameCanvas != null && _inGameCanvas.activeSelf) return;

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

        if (_inGameCanvas != null && !_inGameCanvas.activeSelf) return;

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
