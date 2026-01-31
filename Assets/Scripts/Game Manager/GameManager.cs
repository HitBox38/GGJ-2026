using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public enum LevelState { PreGame, InGame, Win, Lose, Pause }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public event Action<LevelState, LevelState> OnStateChanged;
    
    public int CurrentRunTime { get; private set; }
    [SerializeField, Tooltip("Time in Seconds")] private int maxRunTime = 60;
    
    public int CurrentScore { get; private set; }

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
        
        _pauseAction = InputSystem.actions.FindAction("Pause");
    }
    
    // handle events subscribing
    private void OnEnable() 
    {
        OnStateChanged += HandleStartGame;
        InventoryManager.OnInventoryChanged += HandleItemsChange;
    }

    private void OnDisable()
    {
        OnStateChanged -= HandleStartGame;
        InventoryManager.OnInventoryChanged -= HandleItemsChange;
    }
    
    private void Start()
    {
        ResetLevelTimer();
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
        CurrentRunTime = maxRunTime;
    }

    private void HandleStartGame(LevelState prev, LevelState next)
    {
        if(prev == LevelState.PreGame && next == LevelState.InGame)
            StartCoroutine(TimerCoroutine());
    }
    
    private void AddToRunTime()
    {
        CurrentRunTime--;
    }

    private IEnumerator TimerCoroutine()
    {
        if (State == LevelState.Pause) yield return null;
        if (State != LevelState.InGame) yield break;
        yield return new WaitForSeconds(1);
        AddToRunTime();
        // timer running out
        if (CurrentRunTime <= 0)
        {
            // Run Ends, YOU LOSE!
            SetState(LevelState.Lose);
            Debug.Log("Timer Run out.");
            yield break;
        }
        StartCoroutine(TimerCoroutine());
    }
    
    public void SetState(LevelState next)
    {
        if (next == State) return;
        var prev = State;
        State = next;
        OnStateChanged?.Invoke(prev, next);
        if(next is LevelState.Lose or LevelState.Win) ResetLevelTimer();
    }
    
    private void HandleItemsChange(ItemData[] itemsData) // replace object type with item class type
    {
        CurrentScore = ScoreCalculatorHelper.CalculateScore(itemsData); // replace with actual calculation
    }

    public void ReduceTime(float percentage)
    {
        CurrentRunTime = Mathf.RoundToInt(CurrentRunTime * (1 - percentage));
    }
}
