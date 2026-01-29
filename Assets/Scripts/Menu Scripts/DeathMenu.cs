using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    [SerializeField] private string startGameSceneName;
    [SerializeField] private string mainMenuSceneName;

    public void Restart()
    {
        SceneManager.LoadScene(startGameSceneName);
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
