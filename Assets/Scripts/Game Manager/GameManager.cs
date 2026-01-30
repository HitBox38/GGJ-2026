using System;
using System.Collections;
using System.ComponentModel;
using UnityEngine;

public enum LevelState { PreGame, InGame, Win, Lose, Pause }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public event Action<LevelState, LevelState> OnStateChanged;
    
    [SerializeField, EditorReadOnly] private int currentRunTime;
    
    [SerializeField, Tooltip("Time in Seconds")] private int maxRunTime = 60;

    // handle events subscribing
    private void OnEnable() => OnStateChanged += HandleStartGame;

    private void OnDisable() => OnStateChanged -= HandleStartGame;

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
        // TODO: remove game start test from here
        SetState(LevelState.InGame);
    }

    public void ResetLevelTimer()
    {
        currentRunTime = 0;
    }

    private void HandleStartGame(LevelState prev, LevelState next)
    {
        if(prev == LevelState.PreGame && next == LevelState.InGame)
            StartCoroutine(TimerCoroutine());
    }
    
    private void AddToRunTime()
    {
        currentRunTime++;
    }

    private IEnumerator TimerCoroutine()
    {
        if (State == LevelState.Pause) yield return null;
        if (State != LevelState.InGame) yield break;
        yield return new WaitForSeconds(1);
        AddToRunTime();
        // timer running out
        if (currentRunTime >= maxRunTime)
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
}
