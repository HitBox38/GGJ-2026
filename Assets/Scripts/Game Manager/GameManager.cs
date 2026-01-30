using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum LevelState { PreGame, InGame, Win, Lose, Pause }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public event Action<LevelState, LevelState> OnStateChanged;
    
    [Header("Timer Settings")]
    [SerializeField, EditorReadOnly] private int currentRunTime;
    [SerializeField, Tooltip("Time in Seconds")] private int maxRunTime = 60;

    [Space, Header("Scoring Settings")]
    [SerializeField, EditorReadOnly] private int currentScore;
    
    // handle events subscribing
    private void OnEnable() => OnStateChanged += HandleStartGame;

    private void OnDisable() => OnStateChanged -= HandleStartGame;

    private InputAction _pauseAction;
    
    public LevelState State { get; private set; } = LevelState.PreGame;
    
    /// <summary>
    /// Singleton for this class
    /// </summary>
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
        _pauseAction = InputSystem.actions.FindAction("Pause");
        ResetLevelTimer();
        
        SetState(LevelState.InGame); // TODO: remove game start test from here
    }

    private void Update()
    {
        if (_pauseAction.triggered)
        {
            SetState(State == LevelState.InGame ? LevelState.Pause : LevelState.InGame);
        }
    }

    private void ResetLevelTimer()
    {
        currentRunTime = maxRunTime;
    }

    private void HandleStartGame(LevelState prev, LevelState next)
    {
        if(prev == LevelState.PreGame && next == LevelState.InGame)
            StartCoroutine(TimerCoroutine());
    }
    
    private void AddToRunTime()
    {
        currentRunTime--;
    }

    private IEnumerator TimerCoroutine()
    {
        if (State == LevelState.Pause) yield return null;
        if (State != LevelState.InGame) yield break;
        yield return new WaitForSeconds(1);
        AddToRunTime();
        // timer running out
        if (currentRunTime <= 0)
        {
            // Run Ends, YOU LOSE!
            SetState(LevelState.Lose);
            Debug.Log("Timer Run out.");
            yield break;
        }
        StartCoroutine(TimerCoroutine());
    }
    
    public int GetCurrentRunTime()
    {
        return currentRunTime;
    }
    
    public void SetState(LevelState next)
    {
        if (next == State) return;
        var prev = State;
        State = next;
        OnStateChanged?.Invoke(prev, next);
        if(next is LevelState.Lose or LevelState.Win) ResetLevelTimer();
    }

    /// <summary>
    /// Add to the total score
    /// TODO: based on item object class calculate var (its value to the player)
    /// </summary>
    public void CalculateScore(object[] items) // replace object type with item class type
    {
        currentScore = items.Length + 1; // replace with actual calculation
    }

    public void ReduceTime(float percentage)
    {
        currentRunTime = Mathf.RoundToInt(currentRunTime * (1 - percentage));
    }
}
