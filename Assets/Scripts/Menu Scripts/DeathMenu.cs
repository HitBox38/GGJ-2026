using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    [SerializeField] private string startGameSceneName;
    [SerializeField] private string mainMenuSceneName;

    public void Restart()
    {
        SceneManager.LoadScene(startGameSceneName);

        // set game state to inGame
        GameManager.Instance.SetState(LevelState.InGame);
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);

        // set game state to preGame
        GameManager.Instance.SetState(LevelState.PreGame);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
