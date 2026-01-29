using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private string gameSceneName;

    public void Play()
    {
        SceneManager.LoadScene(gameSceneName);

        // change state to inGame
        GameManager.Instance.SetState(LevelState.InGame);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
