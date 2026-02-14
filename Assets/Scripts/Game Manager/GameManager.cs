using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

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

    public string gameSceneName = "MainTest";
    public string deathSceneName = "DeathMenu";

    [SerializeField] private InventoryManager _inventoryManager;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        print("Scene " + scene.name + "has been loaded");
        // if we are in the game scene, set the game state to inGame
        if (scene.name == gameSceneName)
        {
            print("Setting game manager to play after scene loaded...");
            GameManager.Instance.SetState(LevelState.InGame);
        }
    }
    
    /// <summary>
    /// Singleton for this class
    /// </summary>
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        
        // _pauseAction = InputSystem.actions.FindAction("Pause");
    }

    // handle events subscribing
    private void OnEnable() 
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnStateChanged += HandleStartGame;
        InventoryManager.OnInventoryChanged += HandleItemsChange;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        OnStateChanged -= HandleStartGame;
        InventoryManager.OnInventoryChanged -= HandleItemsChange;
    }
    
    private void Start()
    {
        ResetLevelTimer();
    }

    private void Update()
    {
        // if (_pauseAction.triggered)
        // {
        //     SetState(State == LevelState.InGame ? LevelState.Pause : LevelState.InGame);
        // }
    }

    private void ResetLevelTimer()
    {
        CurrentRunTime = maxRunTime;
    }

    private void HandleStartGame(LevelState prev, LevelState next)
    {
        if (next == LevelState.InGame && prev != LevelState.Pause)
        {
            print("Starting game...");
            StartCoroutine(TimerCoroutine());
        }
    }
    
    private void AddToRunTime()
    {
        CurrentRunTime--;
    }

    private IEnumerator TimerCoroutine()
    {
        // loop until not pause
        while (State == LevelState.Pause)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1);

        if (State == LevelState.InGame)
        {
            AddToRunTime();
        }

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
        if(next is LevelState.Lose) 
        {
            // stop the timer and reset it
            StopAllCoroutines();
            ResetLevelTimer();
            // reset score
            CurrentScore = 0;
            // reset inventory
            if (_inventoryManager != null)
            {
                // TODO: make sure inventory gets reset
                _inventoryManager.ResetInventory();
            }
            // switch scenes to death scene
            SceneManager.LoadScene(deathSceneName);
        }
        if (next is LevelState.Win)
        {
            StopAllCoroutines();
            ResetLevelTimer();
            // TODO: Load winning Scene
            SceneManager.LoadScene(gameSceneName);
        }
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
