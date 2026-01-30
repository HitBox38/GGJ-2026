using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    [SerializeField] private string startGameSceneName;
    [SerializeField] private string mainMenuSceneName;

    public void Restart()
    {
        // set game state to inGame
        GameManager.Instance.SetState(LevelState.InGame);
        // load the start game scene
        SceneManager.LoadScene(startGameSceneName);
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
