using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MaskSelectionManager : MonoBehaviour
{
    public static MaskSelectionManager Instance { get; private set; }
    
    [SerializeField] private int maxMasks = 2;
    [SerializeField] private string gameSceneName = "GameScene";
    
    private MaskObject[] _selectedMasks;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Debug.LogError("More than one MaskSelectionManager in scene! (COULD HAVE DELETED GAME MANAGER!!)");
            Destroy(gameObject);
        }
    }

    //// MASK SELECTIONS
    public void SelectMask(int index, GameObject maskPrefab)
    {
        if (_selectedMasks.Length >= maxMasks) _selectedMasks = new MaskObject[maxMasks];
        _selectedMasks[index] = maskPrefab.GetComponent<MaskObject>();
    }
    
    public MaskObject[] GetSelectedMasks()
    {
        // fail safe if only have 1 mask or none
        if(_selectedMasks[1]) return _selectedMasks;
        if (!_selectedMasks[0]) return null;
        return new[]
        {
            _selectedMasks[0]
        };
    }
    
    public void ConfirmMaskSelection()
    {
        GameManager.Instance.SetState(LevelState.InGame);
        // Move to game scene
        SceneManager.LoadScene(gameSceneName);
    }
}