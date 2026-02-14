using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "MainTest";

    // public global::System.String GameSceneName { get => gameSceneName; set => gameSceneName = value; }

    public void Play()
    {
        // load the game scene
        print(GameManager.Instance.State);
        SceneManager.LoadScene(gameSceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
