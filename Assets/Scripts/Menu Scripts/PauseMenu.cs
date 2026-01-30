using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName;
    [SerializeField] private GameObject pauseMenuUI;

    public void Resume()
    {
        // set game state to inGame and hide pause menu
        GameManager.Instance.SetState(LevelState.InGame);
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
    }

    public void QuitToMainMenu()
    {
        // set game state to preGame
        GameManager.Instance.SetState(LevelState.PreGame);
        // load the main menu scene
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
