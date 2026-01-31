using UnityEngine;
using UnityEngine.InputSystem;

public class ExampleSoundController : MonoBehaviour
{
    [SerializeField] private string targetClipName;
    [SerializeField] private AudioClipsPlaceholder clipsPlaceholder;
    
    private InputAction _interactAction;
    
    private void Start()
    {
        _interactAction = InputSystem.actions.FindAction("Interact"); // action to trigger sound

        // make sure we set the clips placeholder!!
        if (!clipsPlaceholder) Debug.LogError("Audio clips placeholder not set!");
    }
    
    private void Update()
    {
        if (_interactAction.triggered)
        {
            clipsPlaceholder.Play(targetClipName); // play with target clip name
        }
    }
}
