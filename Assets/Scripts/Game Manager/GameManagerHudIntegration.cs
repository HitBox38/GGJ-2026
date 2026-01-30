using TMPro;
using UnityEngine;

public class GameManagerHudIntegration : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    
    private void Update()
    {
        if(!GameManager.Instance) Debug.LogError("Game Manager instance not found!");
        
        scoreText.text = $"{GameManager.Instance.CurrentScore}";
        timerText.text = $"{GameManager.Instance.CurrentRunTime}";
    }
}
