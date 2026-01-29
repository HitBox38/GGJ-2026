using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneName;
    [SerializeField] private string mainMenuSceneName;

    public void Resume()
    {
        SceneManager.LoadScene(gameSceneName);

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
