using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName;
    [SerializeField] private GameObject pauseMenuUI;

    private bool _isPaused;
    private GameManager _gameManager;
    
    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    private void Update()
    {
        // if the game is paused, show the pause menu
        _isPaused = _gameManager.State == LevelState.Pause;
        if (_isPaused == pauseMenuUI.activeInHierarchy) return;
        pauseMenuUI.SetActive(_isPaused);
    }
    
    public void Resume()
    {
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
        // load the main menu scene
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
