using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName;
    [SerializeField] private GameObject pauseMenuUI;

    private bool _isPaused;
    private GameManager _gameManager;

    private PlayerActions _playerActions;
    
    private void Awake()
    {
        _playerActions = new PlayerActions();
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    private void OnEsc(InputAction.CallbackContext context)
    {
        print("Esc detected!");
        // activate / deactivate pause menu
        _isPaused = _gameManager.State == LevelState.Pause;
        print("Paused? " + _isPaused);
        if (_isPaused) // resume the game and the timer
        {
            pauseMenuUI.SetActive(false);
            _gameManager.SetState(LevelState.InGame);
        }
        else // pause the game
        {
            pauseMenuUI.SetActive(true);
            _gameManager.SetState(LevelState.Pause);
        }
    }

    private void OnEnable()
    {
        // subscribe to pause event
        _playerActions.KeyboardControls.Pause.performed += OnEsc;
        _playerActions.KeyboardControls.Enable();
    }

    private void OnDisable()
    {
        //cleanup subscription
        _playerActions.KeyboardControls.Pause.performed -= OnEsc;
        _playerActions.KeyboardControls.Disable();
    }


    // private void Update()
    // {
    //     // if the game is paused, show the pause menu
    //     _isPaused = _gameManager.State == LevelState.Pause;
    //     if (_isPaused == pauseMenuUI.activeInHierarchy) return;
    //     pauseMenuUI.SetActive(_isPaused);
    // }
    
    public void Resume()
    {
        print("Resume Pressed!");
        // set game state to inGame and hide pause menu
        _gameManager.SetState(LevelState.InGame);
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
    }

    public void QuitToMainMenu()
    {
        // set game state to preGame
        _gameManager.SetState(LevelState.PreGame);

        // deactivate the pasuse screen ui
        pauseMenuUI.SetActive(false);
        // load the main menu scene
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
