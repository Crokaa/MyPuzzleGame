using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    private enum GameState { MenuState, ChangeSettings, InGame, Pause }; // if needed these will be public

    [Header("Player")]
    [SerializeField] private PlayerController _player;
    [Header("Canvas")]
    [SerializeField] private GameObject _inGameCanvas;
    [SerializeField] private GameObject _pauseCanvas;
    [SerializeField] private GameObject _settingsCanvas;
    private bool IsPaused
    {
        get { return _isPaused; }
        set
        {
            _isPaused = value;
            previousGameState = CurrentGameState;

            if (IsPaused)
            {
                Time.timeScale = 0f;
                _pauseCanvas.SetActive(true);
                CurrentGameState = GameState.Pause;
            }
            else
            {
                _pauseCanvas.SetActive(false);
                Time.timeScale = 1f;
                CurrentGameState = GameState.InGame;
            }
        }
    }

    private PlayerInputActions _inputActions;
    private GameState CurrentGameState { get; set; }
    private GameState previousGameState;

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
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        _inputActions = new PlayerInputActions();

        Physics2D.gravity = new Vector2(0, -GRAVITYFORCE);
        _inGameCanvas.SetActive(false);
        IsPaused = false;
    }

    void Start()
    {
        SceneController.instance.LoadScene(UINamesHelper.GetName(UIName.MenuScene));

        CurrentGameState = GameState.MenuState;
    }

    void OnEnable()
    {
        _inputActions.Enable();

        _inputActions.Game.Pause.performed += PauseOrCancel;

        _inputActions.Game.Pause.canceled += PauseOrCancel;
    }

    void OnDisable()
    {
        _inputActions.Disable();

        _inputActions.Game.Pause.performed -= PauseOrCancel;

        _inputActions.Game.Pause.canceled -= PauseOrCancel;
    }

    private void PauseOrCancel(InputAction.CallbackContext context)
    {
        if (context.performed && (CurrentGameState == GameState.InGame || CurrentGameState == GameState.Pause))
            PauseUnpause();

        if (context.performed && CurrentGameState == GameState.ChangeSettings)
            CloseSettings();
    }

    public void PauseUnpause()
    {
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
        if (_inGameCanvas != null && _inGameCanvas.activeSelf) return;

        // Not sure about this line yet
        _inGameCanvas.SetActive(true);

        foreach (Transform child in _inGameCanvas.transform)
        {
            if (child.name == UINamesHelper.GetName(UIName.InteractText))
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
                if (child.name == UINamesHelper.GetName(UIName.InteractText))
                {
                    child.gameObject.SetActive(false);
                    break;
                }
            }

            // Not sure about this line yet
            _inGameCanvas.SetActive(false);
        }
    }

    public void StartGame()
    {
        CurrentGameState = GameState.InGame;
        SceneController.instance.LoadScene(UINamesHelper.GetName(UIName.TestLevelScene));
        _player.gameObject.SetActive(true);
    }

    public void OpenSettings()
    {

        if (CurrentGameState == GameState.MenuState)
            HideMenu();
        else if (CurrentGameState == GameState.Pause)
            HidePause();


        _settingsCanvas.SetActive(true);
        previousGameState = CurrentGameState;
        CurrentGameState = GameState.ChangeSettings;
    }

    public void CloseSettings()
    {

        if (previousGameState == GameState.MenuState)
            ShowMenu();
        else if (previousGameState == GameState.Pause)
            ShowPause();

        _settingsCanvas.SetActive(false);
        CurrentGameState = previousGameState;

        // This doesn't really matter, but just for the sake of it
        previousGameState = GameState.ChangeSettings;
    }

    private void ShowPause()
    {
        _pauseCanvas.SetActive(true);
    }

    private void HidePause()
    {
        _pauseCanvas.SetActive(false);
    }

    private void ShowMenu()
    {
        GameObject menuCanvas = GameObject.FindGameObjectWithTag(UINamesHelper.GetName(UIName.MenuCanvasTag));
        MainMenuUI mainMenu = menuCanvas.GetComponent<MainMenuUI>();

        mainMenu.ShowMenuButtons();
    }

    private void HideMenu()
    {
        GameObject menuCanvas = GameObject.FindGameObjectWithTag(UINamesHelper.GetName(UIName.MenuCanvasTag));
        MainMenuUI mainMenu = menuCanvas.GetComponent<MainMenuUI>();

        mainMenu.HideMenuButtons();
    }


    public void GoToMenu()
    {
        SceneController.instance.LoadScene(UINamesHelper.GetName(UIName.MenuScene));

        // In case somehow the game goes to the menu without being from pauses (probably will be possible)
        if (IsPaused)
            IsPaused = !IsPaused;

        _player.gameObject.SetActive(false);

        CurrentGameState = GameState.MenuState;
    }
}
