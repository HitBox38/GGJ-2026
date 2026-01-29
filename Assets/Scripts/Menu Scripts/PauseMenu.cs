using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneName;
    [SerializeField] private string mainMenuSceneName;

    public void Resume()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
