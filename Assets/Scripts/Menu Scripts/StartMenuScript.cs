using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private string gameSceneName;

    public void Play()
    {
        // change state to inGame
        GameManager.Instance.SetState(LevelState.InGame);
        // load the game scene
        SceneManager.LoadScene(gameSceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
